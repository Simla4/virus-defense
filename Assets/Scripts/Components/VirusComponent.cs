using Unity.Entities;
using Unity.Mathematics;

public struct VirusComponent : IComponentData
{
    public float infectionPower;
    public float2 position;
}