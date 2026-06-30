using UnityEngine;

namespace CodeBase.Entities
{
    public interface ITargetable
    {
        void ModifyBlock(int block);
        void TakeDamage(int amount);
        void Heal(int amount);

        void Highlight();
        void RemoveHighlight();
        void Hover();
        void Unhover();
        
        bool IsAlive { get; }
        Transform Transform { get; }
    }
}