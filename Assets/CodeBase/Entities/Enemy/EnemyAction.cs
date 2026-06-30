using System.Collections.Generic;
using CodeBase.Effects;
using UnityEngine;

namespace CodeBase.Entities.Enemy
{
    [CreateAssetMenu(fileName = "newEnemyAction", menuName = "Game/Enemy/Action")]
    public class EnemyAction : ScriptableObject
    {
        public Target target;
        public Sprite icon;
        public int amount;
        public List<EffectData> effects;
    }
}
