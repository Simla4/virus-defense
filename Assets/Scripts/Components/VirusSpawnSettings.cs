using Unity.Entities;

public struct VirusSpawnSettings : IComponentData
{
    public Entity virusPrefab;
    public float spawnInterval;
}