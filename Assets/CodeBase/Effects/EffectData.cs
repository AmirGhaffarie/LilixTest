using CodeBase.Entities;
using UnityEngine;

namespace CodeBase.Effects
{
    public abstract class EffectData : ScriptableObject
    {
        public abstract IEffect CreateEffect();

        public void Execute(ITargetable target)
        {
            var effect = CreateEffect();
            effect.Execute(target);
        }
    }
}