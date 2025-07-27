using Unity.Entities;
using UnityEngine;

public class VirusSpawnSettingsAuthoring : MonoBehaviour
{
    public GameObject virusPrefab;
    public float spawnInterval = 10;

    class Baker : Baker<VirusSpawnSettingsAuthoring>
    {
        public override void Bake(VirusSpawnSettingsAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new VirusSpawnSettings
            {
                virusPrefab = GetEntity(authoring.virusPrefab, TransformUsageFlags.Renderable),
                spawnInterval = authoring.spawnInterval
            });
        }
    }
}