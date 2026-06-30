using UnityEngine;

namespace CodeBase.Effects.Damage
{
    [CreateAssetMenu(fileName = "NewDamageEffect", menuName = "Game/Effects/Damage Effect")]
    public class DamageEffectData : EffectData
    {
        [SerializeField] private int baseDamage;

        public override IEffect CreateEffect()
        {
            return new DamageEffect(baseDamage);
        }
    }
}