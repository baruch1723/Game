using Runtime.Controllers;
using Runtime.Tools;
using UnityEngine;

namespace Runtime.Components
{
    public class Player : MonoBehaviour
    {
        public Health PlayerHealth;
        public SignalController Signal;
        public SonarController Sonar;
        
        public void Init()
        {
            PlayerHealth = GetComponent<Health>();
            Signal = GetComponent<SignalController>();
            Sonar = GetComponent<SonarController>();
            
            InitControllers();
        }

        private void InitControllers()
        {
            PlayerHealth.Init();
            Signal.Init();
            Sonar.Init();
        }
    }
}