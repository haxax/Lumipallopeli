using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    //poolType and poolId are used to return the object back to the correct stacks in pool
    private int poolType = -1;
    private int poolId = -1;

    public void OnInstantiate(int type, int id)
    {
        poolType = type;
        poolId = id;
        OnInstantiate();
    }

    /// <summary>
    /// OnInstantiate() is called when object is created by the pool. Similar to Start() but instant. Do the basic setup here e.g.getcomponent.
    /// </summary>
    public virtual void OnInstantiate() { }

    /// <summary>
    /// OnPreSpawn() is called when object is still in the pool but being taken out. Don't do position related stuff here.
    /// </summary>
    public virtual void OnPreSpawn() { }

    /// <summary>
    /// Call manually. It's recommended to SetPosition() before calling this. Enable collider etc. here.
    /// </summary>
    public virtual void Setup() { }

    public virtual void SetLocalPosition(Vector3 position)
    {
        transform.localPosition = position;
    }

    public virtual void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    //Override to reset values and disable components e.g. colliders. Call base.ReturnToPool() at the end.
    public virtual void ReturnToPool()
    {
        gameObject.SetActive(false);
        transform.position = new Vector3(9998, 9998, 9998);
        Pool.instance.ReturnObject(poolType, poolId, this);
    }
}
