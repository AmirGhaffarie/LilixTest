using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeBase.Entities
{
    namespace CodeBase
    {
        public abstract class BasicEntity : MonoBehaviour, ITargetable, IPointerEnterHandler , IPointerExitHandler
        {
            [SerializeField] private GameObject selectionObject;

            public int MaxHealth => EntityData.maxHealth;
            public int Health { get; protected set; }
            public int Block { get; protected set; }
            public float HealthPercentage => (float)Health / MaxHealth;

            public Action OnStatsChanged;
            public Action OnDeath;

            protected abstract EntityData EntityData { get; }

            public bool IsAlive => Health > 0;
            public virtual Transform Transform => transform;

            protected virtual void InitializeStats()
            {
                Block = 0;
                Health = MaxHealth;
            }

            protected virtual void Start()
            {
                InitializeStats();
                OnStatsChanged?.Invoke();
            }


            public virtual void ModifyBlock(int block)
            {
                Block = Mathf.Max(0, Block + block);
                OnStatsChanged?.Invoke();
            }

            public virtual void TakeDamage(int damage)
            {
                if(!IsAlive)
                    return;
                
                if (Block > 0)
                {
                    int blockedDamage = Mathf.Min(damage, Block);
                    damage -= blockedDamage;
                    Block -= blockedDamage;
                }
                Health -= damage;
                Health =  Mathf.Clamp(Health, 0, EntityData.maxHealth);
                OnStatsChanged?.Invoke();
                if (Health <= 0)
                    Die();
            }

            public virtual void Heal(int heal)
            {
                Health = Mathf.Clamp(Health + heal, 0, EntityData.maxHealth);
                OnStatsChanged?.Invoke();
            }

            public void Hover()
            {
                selectionObject.transform.localScale = Vector3.one * 1.2f;
            }
            public void Unhover()
            {
                selectionObject.transform.localScale = Vector3.one;
            }

            public void RemoveHighlight()
            {
                selectionObject.transform.localScale = Vector3.one;
                selectionObject.SetActive(false);
            }

            public void Highlight()
            {
                if(!IsAlive)
                    return;
                selectionObject.transform.localScale = Vector3.one;
                selectionObject.SetActive(true);
            }

            public virtual void RemoveBlock()
            {
                Block = 0;
                OnStatsChanged?.Invoke();
            }

            protected virtual void Die()
            {
                OnDeath?.Invoke();
            }

            public void OnPointerEnter(PointerEventData eventData)
            {
                Hover();
            }

            public void OnPointerExit(PointerEventData eventData)
            {
                Unhover();
            }
        }
    }
}