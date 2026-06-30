using UnityEngine;

namespace CodeBase.Effects.Heal
{
    [CreateAssetMenu(fileName = "NewHealEffect", menuName = "Game/Effects/Heal Effect")]
    public class HealEffectData : EffectData
    {
        [SerializeField] private int baseHeal;

        public override IEffect CreateEffect()
        {
            return new HealEffect(baseHeal);
        }
    }
}