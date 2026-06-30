using System.Collections.Generic;
using CodeBase.Effects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.Entities.Enemy
{
    [CreateAssetMenu(fileName = "newEnemyAction", menuName = "Game/Enemy/Action")]
    public class EnemyAction : ScriptableObject
    {
        public Target target;
        public Sprite icon;
        public int amount;
        [InlineEditor]public List<EffectData> effects;
        [Header("Visual")]
        public GameObject effectPrefab;
        public float effectDuration;
        public float impactOffset;
    }
}
