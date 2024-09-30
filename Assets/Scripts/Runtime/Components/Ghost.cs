using Runtime.Helpers;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Components
{
    public class Ghost : Enemy
    {
        [SerializeField] private Transform _player;
        [SerializeField] private float _wanderRadius = 10f;
        [SerializeField] private float _detectionRadius = 15f;
        [SerializeField] private float _attackRadius = 1f;
        [SerializeField] private float _wanderTimer = 5f;

        private float _timer;

        private void Start()
        {
            Health = 100;
            Damage = 10;
            
            _player = LevelManager.Instance.GetPlayer().transform;
            _timer = _wanderTimer;
        }

        private void Update()
        {
            var distanceToPlayer = Vector3.Distance(_player.position, transform.position);
            if (distanceToPlayer < _attackRadius)
            {
                AttackPlayer();
            }
            else if (distanceToPlayer < _detectionRadius)
            {
                ChasePlayer();
            }
            else
            {
                Wander(_wanderRadius);
            }
        }

        public override void Hit(int amount)
        {
            Health += amount;
            if (Health <= 0)
            {
                //TODO:Destroy and remove from list
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                player.PlayerHealth.UpdateHealth(-Damage);
            }
        }

        private void Wander(float wanderRadius)
        {
            _timer += Time.deltaTime;

            if (_timer >= _wanderTimer)
            {
                var newPos = RandomPointGenerator.RandomNavSphere(transform.position, wanderRadius, -1);
                Agent.SetDestination(newPos);
                _timer = 0;
            }
        }

        private void ChasePlayer()
        {
            Walk(true);
            Agent.SetDestination(_player.position);
        }

        private void AttackPlayer()
        {
            Agent.isStopped = true;
            EnemyAnimator.SetTrigger(Animation_Attack);
        }

        private void Walk(bool state)
        {
            EnemyAnimator.SetBool(Animation_Walk,state);
        }
    }
}
