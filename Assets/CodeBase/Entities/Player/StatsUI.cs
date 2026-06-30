using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase.Entities.Player
{
    public class StatsUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private PlayerEntity player;
        [Header("Health")]
        [SerializeField] private Image playerHealthBar;
        [SerializeField] private TextMeshProUGUI playerHealthText;
        [Header("Mana")]
        [SerializeField] private TextMeshProUGUI playerManaText;
        [Header("Block")]
        [SerializeField] private GameObject blockUI;
        [SerializeField] private TextMeshProUGUI playerBlockText;

        private void OnEnable()
        {
            player.OnStatsChanged += UpdateUI;
        }
        private void OnDisable()
        {
            if(player != null)
                player.OnStatsChanged -= UpdateUI;
        }
    
        private void UpdateUI()
        {
            playerHealthBar.fillAmount = player.HealthPercentage;
            playerHealthText.text = $"{player.Health}/{player.MaxHealth}";
        
            playerManaText.text = $"{player.Mana}/{player.MaxMana}";

            var hasBlock = player.Block > 0;
        
            blockUI.SetActive(hasBlock);
            playerBlockText.text = player.Block.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            player.TakeDamage(10);
            player.ModifyBlock(5);
        }
    }
}
