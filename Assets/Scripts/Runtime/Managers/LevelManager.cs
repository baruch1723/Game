using System;
using Runtime.Components;
using UnityEngine;

namespace Runtime.Managers
{
    public class LevelManager : MonoBehaviour
    {
        private Player _player;
        private int _score;

        public event Action OnObjectCollected;

        public static LevelManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _score = 0;
            
            _player = FindObjectOfType<Player>();
            _player.Init();
            
            UIManager.Instance.Init();
            //SpawnManager.Instance.Test();
        }

        public void ObjectCollected(int scoreAmount)
        {
            _score += scoreAmount;
            OnObjectCollected?.Invoke();
        }

        public int GetScore()
        {
            return _score;
        }

        public Player GetPlayer()
        {
            return _player;
        }
    }
}