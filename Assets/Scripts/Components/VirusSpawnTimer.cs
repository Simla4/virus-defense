using Unity.Entities;

public struct VirusSpawnTimer : IComponentData
{
    public float timeSinceLastSpawn;
}