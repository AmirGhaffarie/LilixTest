using System;
using System.Collections;
using CodeBase.Entities.CodeBase;
using CodeBase.Entities.Player;
using PrimeTween;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Entities.Enemy
{
    public class EnemyEntity : BasicEntity
    {
        [SerializeField] private EnemyData enemyData;
        
        [SerializeField] private MeshRenderer meshRenderer;
        public EnemyAction CurrentAction {get; private set;}

        public Action onActionChange;
        
        private Vector3 startingPosition;
        private readonly Vector3 vfxOffset = new(0,-0.1f,-0.03f);

        protected override EntityData EntityData => enemyData;

        protected override void Start()
        {
            base.Start();
            SetMaterial();
            startingPosition = transform.position;
        }

        private void SetMaterial()
        {
            meshRenderer.material = enemyData.enemyMaterial;
        }

        public void SetNewAction()
        {
            var actions = enemyData.actions;
            CurrentAction = actions[Random.Range(0, actions.Count)];
            onActionChange?.Invoke();
        }

        public IEnumerator DoActionCoroutine()
        {
            ITargetable target =  CurrentAction.target switch
            {
                Target.self => this,
                Target.enemy => PlayerEntity.Instance,
                _ => null
            };
            
            if(target == null)
                throw new ArgumentException("Invalid target for enemy action");

            yield return Tween.PositionZ(transform, startingPosition.z - .1f, .15f, Ease.OutCubic).
                ToYieldInstruction();

            GameObject vfx = null;
            if (CurrentAction.effectPrefab != null)
            {
                vfx = Instantiate(CurrentAction.effectPrefab,
                    target.Transform.position + vfxOffset, CurrentAction.effectPrefab.transform.rotation);
            }

            if (CurrentAction.effectDuration > 0)
            {
                Tween.Delay(CurrentAction.impactOffset, () => { ApplyEffects(target); });
            }
            else
            {
                ApplyEffects(target);
            }
     
            
            yield return new WaitForSeconds(CurrentAction.effectDuration);

            if(vfx != null)
                Destroy(vfx.gameObject);
            
            RemoveIntentionIcon();

            yield return Tween.PositionZ(transform, startingPosition.z, .15f, Ease.OutCubic).
                ToYieldInstruction();
            
        }

        private void ApplyEffects(ITargetable target)
        {
            foreach (var effect in CurrentAction.effects)
            {
                effect.Execute(target);
            }
        }

        private void RemoveIntentionIcon()
        {
            CurrentAction = null;
            onActionChange?.Invoke();
        }
    }
}