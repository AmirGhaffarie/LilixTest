using System;
using System.Collections;
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

        public CombatState CurrentCombatState { get; private set; }

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
        
        private void Start()
        {
            StartCoroutine(ProcessState());
        }
        
        private void OnPlayerDeath()
        {
            CurrentCombatState = CombatState.BattleEnd;
            OnGameLose?.Invoke();
        }

        private void CheckWin()
        {
            if (enemies.Count <= 0)
            {
                OnGameWin?.Invoke();
                CurrentCombatState = CombatState.BattleEnd;
            }
        }


        private IEnumerator ProcessState()
        {
            endTurnButton.interactable = false;

            while (CurrentCombatState != CombatState.BattleEnd)
            {
                switch (CurrentCombatState)
                {
                    case CombatState.PlayerDraw:

                        player.RemoveBlock();
                        player.RefillMana();

                        yield return cardManager.DrawHandCoroutine();

                        CurrentCombatState = CombatState.PlayerInput;
                        break;

                    case CombatState.PlayerInput:

                        endTurnButton.interactable = true;

                        // Wait until player ends turn
                        yield return new WaitUntil(() =>
                            CurrentCombatState != CombatState.PlayerInput);

                        break;

                    case CombatState.PlayerEnd:

                        endTurnButton.interactable = false;

                        RemoveEnemyBlocks();

                        yield return cardManager.DiscardHandCoroutine();

                        CurrentCombatState = CombatState.EnemyExecute;
                        break;

                    case CombatState.EnemyExecute:

                        yield return StartCoroutine(DoEnemyActions());

                        CurrentCombatState = CombatState.PlayerDraw;
                        break;
                }
            }
        }

        public void EndTurn()
        {
            CurrentCombatState = CombatState.PlayerEnd;
        }

        private IEnumerator DoEnemyActions()
        {
            foreach (var enemy in enemies)
            {
                if (enemy == null)
                    continue;

                yield return enemy.DoActionCoroutine();

                // Small pause between enemies
                yield return new WaitForSeconds(0.3f);
            }
        }

        private void RemoveEnemyBlocks()
        {
            foreach (var enemy in enemies)
            {
                enemy.RemoveBlock();
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