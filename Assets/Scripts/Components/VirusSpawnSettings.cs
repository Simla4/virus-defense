using Unity.Entities;

public struct VirusSpawnSettings : IComponentData
{
    public Entity virusPrefab;
    public int spawnCount;
}