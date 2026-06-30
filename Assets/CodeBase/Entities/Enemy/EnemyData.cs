using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Entities.Enemy
{
    [CreateAssetMenu(fileName = "newEnemyData", menuName = "Game/Enemy/Data")]
    public class EnemyData : EntityData
    {
        public Material enemyMaterial;
        public List<EnemyAction> actions;
    }
}