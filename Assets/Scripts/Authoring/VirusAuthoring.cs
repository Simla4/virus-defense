using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class VirusAuthoring : MonoBehaviour
{
    public float infectionPower = 1f;
    public float2 startPosition;

    class Baker : Baker<VirusAuthoring>
    {
        public override void Bake(VirusAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new VirusComponent
            {
                infectionPower = authoring.infectionPower,
                position = new float2(authoring.transform.position.x, authoring.transform.position.y)
            });
            
            AddComponent(entity, new Unity.Transforms.LocalTransform
            {
                Position = new float3(authoring.startPosition.x, authoring.startPosition.y, 0),
                Rotation = quaternion.identity,
                Scale = 1f
            });
        }
    }
}