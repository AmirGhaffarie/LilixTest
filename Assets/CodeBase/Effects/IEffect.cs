using CodeBase.Entities;

namespace CodeBase.Effects
{
    public interface IEffect
    {
        void Execute(ITargetable target);
        string GetDescription();
    }
}