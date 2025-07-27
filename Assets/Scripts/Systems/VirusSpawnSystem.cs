using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct VirusSpawnSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GridComponent>();  // Grid olmadan çalışmasın
        state.RequireForUpdate<VirusSpawnSettings>(); // Ayarlar olmalı
    }

    public void OnUpdate(ref SystemState state)
    {
        var entityManager = state.EntityManager;
        var gridQuery = entityManager.CreateEntityQuery(typeof(GridComponent));

        if (gridQuery.IsEmpty)
            return; // Grid yoksa çık

        var settings = SystemAPI.GetSingleton<VirusSpawnSettings>();

        var gridEntities = gridQuery.ToEntityArray(Allocator.Temp);
        var gridComponents = gridQuery.ToComponentDataArray<GridComponent>(Allocator.Temp);

        NativeList<int> emptyIndices = new NativeList<int>(Allocator.Temp);
        for (int i = 0; i < gridComponents.Length; i++)
            if (gridComponents[i].isEmpty)
                emptyIndices.Add(i);

        if (emptyIndices.Length == 0)
            return;

        var rand = new Unity.Mathematics.Random((uint)UnityEngine.Random.Range(1, 100000));
        int spawnCount = math.min(settings.spawnCount, emptyIndices.Length);

        for (int i = 0; i < spawnCount; i++)
        {
            int randIndex = rand.NextInt(emptyIndices.Length);
            int gridIndex = emptyIndices[randIndex];

            Entity gridEntity = gridEntities[gridIndex];
            var gridData = gridComponents[gridIndex];

            Entity virusEntity = entityManager.Instantiate(settings.virusPrefab);

            entityManager.SetComponentData(virusEntity, new LocalTransform
            {
                Position = new float3(gridData.gridPosition.x, gridData.gridPosition.y, 1f),
                Rotation = quaternion.identity,
                Scale = 1f
            });

            gridData.isEmpty = false;
            entityManager.SetComponentData(gridEntity, gridData);

            emptyIndices.RemoveAtSwapBack(randIndex);
            
            Debug.Log($"Spawned virus at {gridData.gridPosition.x},{gridData.gridPosition.y}");
        }

        emptyIndices.Dispose();

        state.Enabled = false; // Bir kere çalışsın
    }
}
