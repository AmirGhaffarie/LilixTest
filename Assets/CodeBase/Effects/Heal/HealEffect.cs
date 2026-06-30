using CodeBase.Entities;

namespace CodeBase.Effects.Heal
{
    public class HealEffect : IEffect
    {
        private int amount;
        public HealEffect(int amount) => this.amount = amount;
        public void Execute(ITargetable target)
        {
            target.Heal(amount);
        }
        public string GetDescription() => $"Heals Target for {amount}.";
    }
}