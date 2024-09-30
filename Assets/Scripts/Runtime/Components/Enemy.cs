using UnityEngine;
using UnityEngine.AI;

namespace Runtime.Components
{
    public abstract class Enemy : MonoBehaviour
    {
        public NavMeshAgent Agent;
        public Animator EnemyAnimator;
        
        public int Health;
        public int Damage;
        
        public string Animation_Attack;
        public string Animation_Walk;

        public abstract void Hit(int amount);
    }
}