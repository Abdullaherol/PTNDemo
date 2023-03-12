using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Factory : MonoBehaviour //General factory and it has object pool.
{
    private List<WorldEntity> _worldEntities = new List<WorldEntity>();//pool

    //Get instance if has in pool, get from pool.if not instantiate a new world entity
    public WorldEntity GetWorldEntity(Entity entity)
    {
        int count = _worldEntities.Count(x => x.entity == entity);

        if (count > 0)
        {
            var worldEntity = _worldEntities.First(x => x.entity == entity);
            worldEntity.gameObject.SetActive(true);

            return worldEntity;
        }

        return CreateNewWorldEntity(entity);
    }

    //Return spawned world entity and add to pool
    public void ReturnWorldEntity(WorldEntity worldEntity)
    {
        worldEntity.gameObject.SetActive(false);
        worldEntity.transform.position = Vector3.zero;
        _worldEntities.Add(worldEntity);
    }

    //If pool does not have any entity which wanted type, then instantiate a new entity
    private WorldEntity CreateNewWorldEntity(Entity entity)
    {
        var newObject = Instantiate(entity.prefab);
        return newObject.GetComponent<WorldEntity>();
    }
}