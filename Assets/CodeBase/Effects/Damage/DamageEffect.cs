using CodeBase.Entities;

namespace CodeBase.Effects.Damage
{
    public class DamageEffect : IEffect
    {
        private int amount;
        public DamageEffect(int amount) => this.amount = amount;
        public void Execute(ITargetable target)
        {
            target.TakeDamage(amount);
        }
        public string GetDescription() => $"Deal {amount} damage.";
    }
}