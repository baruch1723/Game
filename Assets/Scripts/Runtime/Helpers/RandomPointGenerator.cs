using UnityEngine;

namespace Runtime.Helpers
{
    public class RandomPointGenerator : MonoBehaviour
    {
        public static Vector3 GetRandomPoint(float spreadRadius, float checkRadius, int collisionMask)
        {
            var environmentBounds = CalculateEnvironmentBounds();
            if (environmentBounds.size == Vector3.zero) return Vector3.zero;

            Vector3 randomPoint;
            bool validPoint;

            do
            {
                randomPoint = new Vector3(
                    Random.Range(environmentBounds.min.x / spreadRadius, environmentBounds.max.x / spreadRadius),
                    0.5f,
                    Random.Range(environmentBounds.min.z / spreadRadius, environmentBounds.max.z / spreadRadius)
                );

                validPoint = !Physics.CheckSphere(randomPoint, checkRadius, collisionMask);
            } while (!validPoint);


            return randomPoint;
        }

        public static Bounds CalculateEnvironmentBounds()
        {
            var environment = GameObject.FindWithTag("Environment");
            if (environment == null) return new Bounds();

            var bounds = new Bounds(environment.transform.position, Vector3.zero);
            var renderers = environment.GetComponentsInChildren<Renderer>();

            foreach (var renderer in renderers)
            {
                bounds.Encapsulate(renderer.bounds);
            }

            return bounds;
        }
        
        public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
        {
            Vector3 randomDirection = Random.insideUnitSphere * distance;
            randomDirection += origin;
            UnityEngine.AI.NavMeshHit navHit;
            UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);
            return navHit.position;
        }
    }
}