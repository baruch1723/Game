using System.Collections.Generic;
using System.Linq;
using Runtime.Components;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Controllers
{
    public class CollectSpots : MonoBehaviour
    {
        private List<GameObject> _spots;
        private Spot _currentSpot;
        
        private void Start()
        {
            _spots = new List<GameObject>();
            for (int i = 0; i < 10; i++)
            {
                var peeSpot = SpawnManager.Instance.SpawnPeeSpot();
                _spots.Add(peeSpot);
            }
        }

        public void RemoveSpot(GameObject o)
        {
            _spots.Remove(o);
            GetComponent<GameController>().ClaimTerrain(LevelManager.Instance.GetPlayer().transform.position,1);
        }
    }
}