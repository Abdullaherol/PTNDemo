using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Factory : MonoBehaviour
{
    private List<WorldEntity> _worldEntities = new List<WorldEntity>();

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

    public void ReturnWorldEntity(WorldEntity worldEntity)
    {
        worldEntity.gameObject.SetActive(false);
        _worldEntities.Add(worldEntity);
    }

    private WorldEntity CreateNewWorldEntity(Entity entity)
    {
        var newObject = Instantiate(entity.prefab);
        return newObject.GetComponent<WorldEntity>();
    }
}