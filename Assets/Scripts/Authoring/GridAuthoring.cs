using Unity.Entities;
using UnityEngine;

public class GridAuthoring : MonoBehaviour
{
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private GameObject cellPrefab;

    private class Baker : Baker<GridAuthoring>
    {
        public override void Bake(GridAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent<GridSpawnTag>(entity);

            var prefabEntity = GetEntity(authoring.cellPrefab, TransformUsageFlags.Renderable);
            AddComponent(entity, new GridPrefabReference { prefab = prefabEntity });

            AddComponent(entity, new GridSettingsComponent
            {
                width = authoring.width,
                height = authoring.height
            });
        }
    }
}

public struct GridPrefabReference : IComponentData
{
    public Entity prefab;
}