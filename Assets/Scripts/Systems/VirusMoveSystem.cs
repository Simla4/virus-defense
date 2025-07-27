using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct VirusMovementSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (virus, transform) in SystemAPI.Query<RefRW<VirusComponent>, RefRW<LocalTransform>>())
        {
            virus.ValueRW.position.x += virus.ValueRO.infectionPower * deltaTime;

            transform.ValueRW.Position.x = virus.ValueRW.position.x;
        }
    }
}