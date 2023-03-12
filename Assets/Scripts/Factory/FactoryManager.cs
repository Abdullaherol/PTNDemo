using UnityEngine;

public class FactoryManager : Singleton<FactoryManager>//Factory manager,it control our general factory.
{
    [SerializeField] private Factory _factory;

    public WorldEntity GetWorldEntity(Entity entity)
    {
        return _factory.GetWorldEntity(entity);
    }

    public void ReturnWorldEntity(WorldEntity worldEntity)
    {
        _factory.ReturnWorldEntity(worldEntity);
    }
}