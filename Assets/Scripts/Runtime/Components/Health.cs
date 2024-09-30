using System;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Components
{
    public class Health : MonoBehaviour
    {
        private float _hp;

        public float HealthPoints => _hp;

        public static event Action<float> OnHealthUpdate;

        public void Init()
        {
            _hp = 100f;
        }

        public void UpdateHealth(float amount)
        {
            _hp += amount;
            OnHealthUpdate.Invoke(_hp);
        }
    }
}