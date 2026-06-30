using System.Collections.Generic;
using CodeBase.Effects;
using CodeBase.Entities;
using Sirenix.OdinInspector;
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
        [InlineEditor]public List<EffectData> effects;
        [Header("Visual")]
        public GameObject effectPrefab;
        public float effectDuration;
        public float impactOffset;
        
        public CardInstance CreateInstance()
        {
            return new CardInstance(this);
        }
    }
}