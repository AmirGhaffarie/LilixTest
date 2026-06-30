using System.Collections.Generic;
using System.Linq;
using CodeBase.Effects;

namespace CodeBase.Cards
{
    public class CardInstance
    {
        public CardData Data { get; private set; }
        public int CurrentCost { get; set; }
        public List<IEffect> Effects { get; private set; }

        public List<CardView> Views;

        public CardInstance(CardData data)
        {
            Data = data;
            CurrentCost = data.baseCost;
            Effects = data.effects.Select(e => e.CreateEffect()).ToList();
            Views = new List<CardView>();
        }
    }
}