using System;
using System.Collections.Generic;
using CodeBase.Cards;
using CodeBase.Entities.CodeBase;
using UnityEngine;

namespace CodeBase.Entities.Player
{
    public class PlayerEntity : BasicEntity
    {
        public static PlayerEntity Instance;
        [SerializeField] private PlayerData playerData;
        [SerializeField] private Transform effectTarget;
        public int MaxMana => playerData.maxMana;
        public int Mana { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        protected override EntityData EntityData => playerData;

        public override Transform Transform => effectTarget;

        public List<CardData> StartingDeck => playerData.startingDeck;
        
        public void SpendMana(int cost)
        {
            if (Mana < cost)
                throw new ArgumentException("Not enough energy to spend.");
            Mana -= cost;
            OnStatsChanged?.Invoke();
        }

        public void RefillMana()
        {
            Mana = playerData.maxMana;
            OnStatsChanged?.Invoke();
        }
        
    }
}