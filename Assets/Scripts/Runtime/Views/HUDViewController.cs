using Runtime.Managers;
using TMPro;
using UnityEngine;

namespace Runtime.Views
{
    public class HUDViewController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        public void Init()
        {
            OnScoreUpdated();
            LevelManager.Instance.OnObjectCollected += OnScoreUpdated;
        }

        public void OnDisable()
        {
            LevelManager.Instance.OnObjectCollected -= OnScoreUpdated;
        }

        private void OnScoreUpdated()
        {
            var score = LevelManager.Instance.GetScore();
            _scoreText.text = $"{score}";
        }
    }
}