using Unity.Entities;
using UnityEngine;

public class VirusSpawnSettingsAuthoring : MonoBehaviour
{
    public GameObject VirusPrefab;
    public int SpawnCount = 10;

    class Baker : Baker<VirusSpawnSettingsAuthoring>
    {
        public override void Bake(VirusSpawnSettingsAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new VirusSpawnSettings
            {
                virusPrefab = GetEntity(authoring.VirusPrefab, TransformUsageFlags.Renderable),
                spawnCount = authoring.SpawnCount
            });
        }
    }
}