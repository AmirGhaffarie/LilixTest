using System;
using System.Collections.Generic;
using CodeBase.Cards;
using CodeBase.Entities.Enemy;
using CodeBase.Entities.Player;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        [Header("References")]
        [SerializeField] private PlayerEntity player;
        [SerializeField] private CardManager cardManager;
        [SerializeField] private List<EnemyEntity> enemies;
        
        [SerializeField] private Button endTurnButton;
        
        public CombatState CurrentCombatState {get; private set;}

        public static Action OnGameWin;
        public static Action OnGameLose;
        
        private void Awake()
        {
            Instance = this;
            CurrentCombatState = CombatState.PlayerDraw;
        }

        private void OnEnable()
        {
            foreach (var enemy in enemies)
            {
                enemy.OnDeath += () =>
                {
                    enemies.Remove(enemy);
                    Destroy(enemy.gameObject);
                    CheckWin();
                };
            }

            player.OnDeath += OnPlayerDeath;
        }

        private void OnDisable()
        {
            if(player != null)
                player.OnDeath -= OnPlayerDeath;
        }

        private void OnPlayerDeath()
        {
            OnGameLose?.Invoke();
        }

        private void CheckWin()
        {
            if(enemies.Count <= 0)
                OnGameWin?.Invoke();
        }

        private void Start()
        {
            ProcessState();
        }

        public void ProcessState()
        {
            switch (CurrentCombatState)
            {
                case CombatState.PlayerDraw:
                    player.RemoveBlock();
                    player.RefillMana();
                    cardManager.DrawHand();
                    CurrentCombatState = CombatState.PlayerInput;
                    break;
                case CombatState.PlayerInput:
                    CurrentCombatState = CombatState.PlayerEnd;
                    ProcessState();
                    break;
                case CombatState.PlayerEnd:
                    cardManager.DiscardHand();
                    CurrentCombatState = CombatState.EnemyExecute;
                    ProcessState();
                    break;
                case CombatState.EnemyExecute:
                    CurrentCombatState = CombatState.EnemyEnd;
                    RemoveEnemyBlocks();
                    DoEnemyActions();
                    ProcessState();
                    break;
                case CombatState.EnemyEnd:
                    CurrentCombatState = CombatState.PlayerDraw;
                    ProcessState();
                    break;
                case CombatState.BattleEnd:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            endTurnButton.interactable = CurrentCombatState == CombatState.PlayerInput;
        }

        private void RemoveEnemyBlocks()
        {
            foreach (var enemy in enemies)
            {
                enemy.RemoveBlock();
            }        
        }

        private void DoEnemyActions()
        {
            foreach (var enemy in enemies)
            {
                enemy.DoAction();
            }
        }

        public void HighlightEnemies()
        {
            foreach (var enemy in enemies)
            {
                enemy.Highlight();
            }
        }

        public void HighlightPlayer()
        {
            player.Highlight();
        }

        public void DisableAllSelections()
        {
            foreach (var enemy in enemies)
            {
                enemy.RemoveHighlight();
            }
            player.RemoveHighlight();
        }
    }
}