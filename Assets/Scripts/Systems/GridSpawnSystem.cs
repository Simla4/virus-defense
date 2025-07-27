using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct GridSpawnSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GridSpawnTag>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var entityManager = state.EntityManager;

        var query = SystemAPI.QueryBuilder()
            .WithAll<GridSpawnTag, GridPrefabReference, GridSettingsComponent>()
            .Build();

        if (query.IsEmpty)
            return;

        var spawnerEntity = query.GetSingletonEntity();

        var prefab = entityManager.GetComponentData<GridPrefabReference>(spawnerEntity).prefab;
        var settings = entityManager.GetComponentData<GridSettingsComponent>(spawnerEntity);

        int width = settings.width;
        int height = settings.height;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var cellEntity = entityManager.Instantiate(prefab);

                entityManager.SetComponentData(cellEntity, new LocalTransform
                {
                    Position = new float3(x, y, 0f), // 2D için XY düzlemi
                    Rotation = quaternion.identity,
                    Scale = 1f
                });

                entityManager.AddComponentData(cellEntity, new GridComponent
                {
                    gridPosition = new int2(x, y),
                    isEmpty = true
                });
            }
        }

        state.Enabled = false;
    }
}