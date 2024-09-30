using Runtime.Managers;
using UnityEngine;

namespace Runtime.Components
{
    public class Treasure : MonoBehaviour
    {
        [SerializeField] private int _scoreAmount = 10;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player _)) return;

            var levelManager = FindObjectOfType<LevelManager>();
            if (levelManager != null)
            {
                levelManager.ObjectCollected(_scoreAmount);
            }

            Destroy(gameObject, 0.1f);
        }
    }
}