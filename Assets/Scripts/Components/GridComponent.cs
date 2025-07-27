using Unity.Entities;
using Unity.Mathematics;

public struct GridComponent : IComponentData
{
    public int2 gridPosition;
    public bool isEmpty;
}
