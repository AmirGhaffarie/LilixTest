using System.Collections.Generic;
using CodeBase.Effects;
using CodeBase.Entities;
using UnityEngine;

namespace CodeBase.Cards
{
    [CreateAssetMenu(fileName = "NewCard", menuName = "Game/Card Data")]
    public class CardData : ScriptableObject
    {
        public string cardName;
        public Target target;
        public int baseCost;
        public Sprite artwork;
        [TextArea] public string description;
        public List<EffectData> effects;
        
        public CardInstance CreateInstance()
        {
            return new CardInstance(this);
        }
    }
}