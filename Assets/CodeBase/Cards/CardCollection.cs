using System;
using System.Collections.Generic;
using CodeBase.Utils;

namespace CodeBase.Cards
{
    public class CardCollection
    {
        public event Action<CardInstance> OnCardAdded;
        public event Action<CardInstance> OnCardRemoved;
        public int Count => Cards.Count;

        public List<CardInstance> Cards { get; } = new();

        public void Add(CardInstance card) { Cards.Add(card); OnCardAdded?.Invoke(card); }
        public void Remove(CardInstance card) { Cards.Remove(card); OnCardRemoved?.Invoke(card); }

        public CardInstance Draw()
        {
            var card = Cards[^1];
            Remove(card);
            return card;
        }

        public void Shuffle()
        {
            Cards.Shuffle();
        }

        public bool Contains(CardInstance card)
        {
            return Cards.Contains(card);
        }
    }
}