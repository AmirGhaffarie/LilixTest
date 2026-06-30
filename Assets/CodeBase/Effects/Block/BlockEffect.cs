using CodeBase.Entities;

namespace CodeBase.Effects.Block
{
    public class BlockEffect : IEffect
    {
        private int amount;
        public BlockEffect(int amount) => this.amount = amount;
        public void Execute(ITargetable target)
        {
            target.ModifyBlock(amount);
        }
        public string GetDescription() => $"Adds {amount} Block.";
    }
}