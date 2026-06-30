using System;
using System.Collections.Generic;
using CodeBase.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace CodeBase.Cards
{
    public class CardAimController : MonoBehaviour
    {
        [SerializeField] private LineRenderer aimLine;
        [SerializeField] private GraphicRaycaster canvasRaycaster;

        private Camera mainCamera;
        private bool isAiming;

        private ITargetable currentTarget;
        private Action<ITargetable> onConfirm;
        private Action onCancel;

        private Vector3 cardOrigin;

        private PointerEventData pointerEventData;
        private readonly List<RaycastResult> uiResults = new();

        private void Awake()
        {
            mainCamera = Camera.main;
            pointerEventData = new PointerEventData(EventSystem.current);

            aimLine.positionCount = 2;
            aimLine.enabled = false;
        }

        private void Update()
        {
            if (!isAiming)
                return;
            
            // Confirm
            if (IsConfirmReleased())
            {
                StopAim();

                if (currentTarget != null)
                    onConfirm?.Invoke(currentTarget);
                else
                    onCancel?.Invoke();
            }
            
            if (!TryGetPointerPosition(out Vector2 pointerPos))
                return;

            var ray = mainCamera.ScreenPointToRay(pointerPos);

            var gameplayPlane =
                new Plane(Vector3.up, Vector3.down * .35f);

            var targetPoint = cardOrigin;

            if (gameplayPlane.Raycast(ray, out var distance))
            {
                targetPoint = ray.GetPoint(distance);
            }

            // Draw line
            aimLine.SetPosition(0, cardOrigin);
            aimLine.SetPosition(1, targetPoint);

            currentTarget = null;

            // 3D target detection
            if (Physics.Raycast(ray, out var hit))
            {
                var target =
                    hit.collider.transform.parent?.GetComponent<ITargetable>();

                if (target is { IsAlive: true })
                    currentTarget = target;
            }
            else
            {
                // UI target detection
                pointerEventData.position = pointerPos;
                pointerEventData.button = PointerEventData.InputButton.Left;

                uiResults.Clear();
                canvasRaycaster.Raycast(pointerEventData, uiResults);

                foreach (var result in uiResults)
                {
                    var target =
                        result.gameObject.GetComponent<ITargetable>();

                    if (target == null &&
                        result.gameObject.transform.parent != null)
                    {
                        target = result.gameObject.transform.parent
                            .GetComponent<ITargetable>();
                    }

                    if (target is { IsAlive: true })
                    {
                        currentTarget = target;
                        break;
                    }
                }
            }

        }

        public void StartAiming(
            Action<ITargetable> confirm,
            Action cancel)
        {
            onConfirm = confirm;
            onCancel = cancel;

            isAiming = true;
            aimLine.enabled = true;

            GetOriginPoint();
        }

        private void GetOriginPoint()
        {
            if (!TryGetPointerPosition(out Vector2 pointerPos))
                return;

            var ray = mainCamera.ScreenPointToRay(pointerPos);

            var gameplayPlane =
                new Plane(Vector3.up, Vector3.down * .35f);

            cardOrigin =
                gameplayPlane.Raycast(ray, out var distance)
                    ? ray.GetPoint(distance)
                    : Vector3.zero;
        }

        private bool TryGetPointerPosition(out Vector2 position)
        {
            // Touch has priority on mobile
            if (Touchscreen.current != null &&
                Touchscreen.current.primaryTouch.press.isPressed)
            {
                position =
                    Touchscreen.current.primaryTouch.position.ReadValue();

                return true;
            }

            // Mouse fallback
            if (Mouse.current != null)
            {
                position = Mouse.current.position.ReadValue();
                return true;
            }

            position = default;
            return false;
        }

        private bool IsConfirmReleased()
        {
            // Touch released
            if (Touchscreen.current != null)
            {
                var touch = Touchscreen.current.primaryTouch;
                if (touch.press.wasReleasedThisFrame)
                    return true;
            }

            // Mouse released
            return Mouse.current != null &&
                   Mouse.current.leftButton.wasReleasedThisFrame;
        }

        private void StopAim()
        {
            isAiming = false;
            aimLine.enabled = false;
        }
    }
}