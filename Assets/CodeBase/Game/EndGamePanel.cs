using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Game
{
    public class EndGamePanel : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject losePanel;

        private void OnEnable()
        {
            GameManager.OnGameWin += OnGameWin;
            GameManager.OnGameLose += OnGameLose;
        }

        private void OnDisable()
        {
            GameManager.OnGameWin -= OnGameWin;
            GameManager.OnGameLose -= OnGameLose;
        }

        private void OnGameLose()
        {
            panel.SetActive(true);
            losePanel.SetActive(true);
        
            winPanel.SetActive(false);
        }

        private void OnGameWin()
        {
            panel.SetActive(true);
            winPanel.SetActive(true);
        
            losePanel.SetActive(false);
        }

        public void Restart()
        {
            SceneManager.LoadScene(0);
        }
    }
}
