using UnityEngine;

namespace Runtime.Interfaces
{
    public interface ISpawnFactory
    {
        GameObject Create(Vector3 position);
    }
}