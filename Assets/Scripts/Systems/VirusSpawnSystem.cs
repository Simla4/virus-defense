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
        state.RequireForUpdate<GridComponent>(); 
        state.RequireForUpdate<VirusSpawnSettings>(); 
        
        Entity timerEntity = state.EntityManager.CreateEntity();
        state.EntityManager.AddComponentData(timerEntity, new VirusSpawnTimer { timeSinceLastSpawn = 0f });
    }

    public void OnUpdate(ref SystemState state)
{
    var entityManager = state.EntityManager;
    var settings = SystemAPI.GetSingleton<VirusSpawnSettings>();
    float deltaTime = SystemAPI.Time.DeltaTime;

    foreach (var (timer, entity) in SystemAPI.Query<RefRW<VirusSpawnTimer>>().WithEntityAccess())
    {
        timer.ValueRW.timeSinceLastSpawn += deltaTime;

        if (timer.ValueRW.timeSinceLastSpawn >= settings.spawnInterval)
        {
            timer.ValueRW.timeSinceLastSpawn = 0f;

            // 🔁 Spawn mantığı
            var gridQuery = entityManager.CreateEntityQuery(typeof(GridComponent));
            var gridEntities = gridQuery.ToEntityArray(Allocator.Temp);
            var gridComponents = gridQuery.ToComponentDataArray<GridComponent>(Allocator.Temp);

            NativeList<int> emptyIndices = new NativeList<int>(Allocator.Temp);
            for (int i = 0; i < gridComponents.Length; i++)
                if (gridComponents[i].isEmpty)
                    emptyIndices.Add(i);

            if (emptyIndices.Length == 0)
            {
                Debug.Log("Tüm gridler dolu!");
                return;
            }

            int totalGridCount = gridComponents.Length;
            float fillRate = (totalGridCount - emptyIndices.Length) / (float)totalGridCount;

            if (fillRate >= 0.85f)
            {
                Debug.Log("Oyun bitti: Grid %85 doldu");
                // TODO: GameOver sistemi tetiklenebilir (event, scene reload vs.)
                return;
            }

            var rand = new Unity.Mathematics.Random((uint)UnityEngine.Random.Range(1, 100000));
            int randIndex = rand.NextInt(emptyIndices.Length);
            int gridIndex = emptyIndices[randIndex];

            var gridData = gridComponents[gridIndex];
            Entity gridEntity = gridEntities[gridIndex];

            Entity virusEntity = entityManager.Instantiate(settings.virusPrefab);

            entityManager.SetComponentData(virusEntity, new LocalTransform
            {
                Position = new float3(gridData.gridPosition.x, gridData.gridPosition.y, 0f),
                Rotation = quaternion.identity,
                Scale = 1f
            });

            gridData.isEmpty = false;
            entityManager.SetComponentData(gridEntity, gridData);

            Debug.Log($"Spawned virus at {gridData.gridPosition.x},{gridData.gridPosition.y}");

            emptyIndices.Dispose();
        }
    }
}

}
