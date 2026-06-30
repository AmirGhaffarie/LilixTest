using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase.Entities.Enemy
{
    public class EnemyStatsUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private EnemyEntity enemy;
        [Header("Health")]
        [SerializeField] private Image enemyHealthBar;
        [SerializeField] private TextMeshProUGUI enemyHealthText;
        [Header("Block")]
        [SerializeField] private GameObject blockUI;
        [SerializeField] private TextMeshProUGUI enemyBlockText;
        [Header("Action Indicator")]
        [SerializeField] private Image actionIcon;
        [SerializeField] private TextMeshProUGUI actionText;

        private void OnEnable()
        {
            enemy.onActionChange += UpdateAction;
            enemy.OnStatsChanged += UpdateUI;
        }

        
        private void OnDisable()
        {
            if (enemy == null) return;
            
            enemy.onActionChange -= UpdateAction;
            enemy.OnStatsChanged -= UpdateUI;
        }
        private void UpdateAction()
        {
            var action = enemy.CurrentAction;
            actionIcon.sprite = action.icon;
            actionText.text = action.amount.ToString();
        }
        
        private void UpdateUI()
        {
            enemyHealthBar.fillAmount = enemy.HealthPercentage;
            enemyHealthText.text = $"{enemy.Health}/{enemy.MaxHealth}";
        

            var hasBlock = enemy.Block > 0;
        
            blockUI.SetActive(hasBlock);
            enemyBlockText.text = enemy.Block.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            enemy.DoAction();
        }
    }
}
