using Runtime.Views;
using UnityEngine;

namespace Runtime.Managers
{
    public class UIManager : MonoBehaviour
    {
        public HUDViewController HudView;
        public SonarViewController SonarView;
        public HealthBarView HealthView;
        public ProgressView ProgressViewController;
        public AreaClaimButton ClaimButton;

        public static UIManager Instance { get; private set; }

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
        
        public void Init()
        {
            var player = LevelManager.Instance.GetPlayer();
            HudView.Init();
            SonarView.Init(player.Sonar);
            HealthView.Init(player.PlayerHealth.HealthPoints);
            ProgressViewController.Init(0f);
        }
    }
}