using System.Collections.Generic;
using CodeBase.Cards;
using UnityEngine;

namespace CodeBase.Entities.Player
{
    [CreateAssetMenu(fileName = "newPlayerData", menuName = "Game/Player/Data")]
    public class PlayerData : EntityData
    {
        public int maxMana;
        public List<CardData> startingDeck;
    }
}