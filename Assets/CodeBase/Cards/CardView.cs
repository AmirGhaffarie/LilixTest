using System;
using System.Linq;
using CodeBase.Entities;
using CodeBase.Entities.Player;
using CodeBase.Game;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PrimeTween;

namespace CodeBase.Cards
{
    public class CardView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image artworkImage;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        private CardInstance card;
        private CardManager manager;

        private void OnEnable()
        {
            PlayerEntity.Instance.OnStatsChanged += UpdateVisuals;
        }

        private void OnDisable()
        {
            if(PlayerEntity.Instance != null)
                PlayerEntity.Instance.OnStatsChanged -= UpdateVisuals;
        }

        public void Bind(CardInstance cardInstance, CardManager cardManager)
        {
            card = cardInstance;
            manager = cardManager;
            cardInstance.Views.Add(this);
            UpdateVisuals();
        }
        
        private void UpdateVisuals()
        {
            var canAfford = manager.CanAfford(card);
            
            artworkImage.sprite = card.Data.artwork;
            costText.text = card.CurrentCost.ToString();
            
            costText.color = canAfford? Color.green : Color.red;
            
            nameText.text = card.Data.cardName;
            descriptionText.text = string.Join("\n", card.Effects.Select(e => e.GetDescription()));
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!manager.CanAfford(card)) return;

            var targetPlayer = card.Data.target == Target.self;
            
            if(targetPlayer)
                GameManager.Instance.HighlightPlayer();
            else
                GameManager.Instance.HighlightEnemies();
            
            Tween.Scale(transform, Vector3.one * 1.2f, 0.14f);
            
            manager.StartAimingForCard(card);
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GameManager.Instance.DisableAllSelections();
            Tween.StopAll(transform);
            transform.localScale = Vector3.one;
        }
    }
}