using System;
using System.Collections;
using CodeBase.Entities.CodeBase;
using CodeBase.Entities.Player;
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

        protected override EntityData EntityData => enemyData;

        protected override void Start()
        {
            base.Start();
            SetMaterial();
            SetNewAction();
        }

        private void SetMaterial()
        {
            meshRenderer.material = enemyData.enemyMaterial;
        }

        private void SetNewAction()
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

            yield return new WaitForSeconds(0.5f);
            
            foreach (var effect in CurrentAction.effects)
            {
                effect.Execute(target);
            }
            SetNewAction();
        }

    }
}