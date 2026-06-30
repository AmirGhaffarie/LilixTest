using CodeBase.Effects.Heal;
using UnityEngine;

namespace CodeBase.Effects.Block
{
    [CreateAssetMenu(fileName = "NewBlockEffect", menuName = "Game/Effects/Block Effect")]
    public class BlockEffectData : EffectData
    {
        [SerializeField] private int baseBlock;

        public override IEffect CreateEffect()
        {
            return new BlockEffect(baseBlock);
        }
    }
}