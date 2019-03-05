using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PrefabType
{
    [Tooltip("Category name for prefabs e.g. item, monster, effect, assorted")]
    [SerializeField] private string prefabTypeName;
    public string PrefabTypeName { get { return prefabTypeName; } private set { prefabTypeName = value; } }
    [Tooltip("Prefabs within the category")]
    [SerializeField] private List<GameObject> prefabs;
    public List<GameObject> Prefabs { get { return prefabs; } private set { prefabs = value; } }
}

public class Pool : MonoBehaviour
{
    public static Pool instance;

    [SerializeField] private List<PrefabType> prefabs = new List<PrefabType>();
    private List<List<Stack<Poolable>>> objectsInPool = new List<List<Stack<Poolable>>>();

    private void Awake()
    {
        if (instance == null)
        { instance = this; }
        else if (instance != this)
        { Destroy(gameObject); }

        //Generate pool stacks
        for (int i = 0; i < prefabs.Count; i++)
        {
            objectsInPool.Add(new List<Stack<Poolable>>());
            for (int j = 0; j < prefabs[i].Prefabs.Count; j++)
            {
                objectsInPool[i].Add(new Stack<Poolable>());
            }
        }
    }

    public Poolable GetFromPool(int type, int id)
    {
        try
        {
            //Check if pool stack is empty
            if (objectsInPool[type][id].Count <= 0)
            {
                //Generate new object to the stack
                GameObject newPoolObj = Instantiate(prefabs[type].Prefabs[id], new Vector3(9999, 9999, 9999), Quaternion.identity);
                objectsInPool[type][id].Push(newPoolObj.GetComponent<Poolable>());
                objectsInPool[type][id].Peek().OnInstantiate(type, id);
            }
            //Get object from pool
            objectsInPool[type][id].Peek().OnPreSpawn();
            return objectsInPool[type][id].Pop();
        }
        catch
        {
            if (type < 0 || id < 0)
            { Debug.Log(string.Format("Pool error, input less than zero, type: {0} id: {1}", type, id)); }
            else if (type >= objectsInPool.Count)
            { Debug.Log(string.Format("Pool error, type more than pool, type: {0}/{1}", type, objectsInPool.Count)); }
            else if (id >= objectsInPool[type].Count)
            { Debug.Log(string.Format("Pool error, id more than pool, id: {0}/{1}", id, objectsInPool[type].Count)); }
            else
            { Debug.Log("Pool error, else"); }
            return null;
        }
    }

    public void ReturnObject(int type, int id, Poolable obj)
    {
        objectsInPool[type][id].Push(obj);
    }

    public Poolable GetFromPool(string typeName, string prefabName)
    {
        for (int i = 0; i < prefabs.Count; i++)
        {
            if (prefabs[i].PrefabTypeName == typeName)
            {
                for (int j = 0; j < prefabs[i].Prefabs.Count; j++)
                {
                    if (prefabs[i].Prefabs[j].name == prefabName)
                    {
                        return GetFromPool(i, j);
                    }
                }
                Debug.Log(string.Format("Pool error, prefabName not found: {0}", prefabName));
                return null;
            }
        }
        Debug.Log(string.Format("Pool error, typeName not found: {0}", typeName));
        return null;
    }

    public Poolable GetFromPool(string typeName, int id)
    {
        for (int i = 0; i < prefabs.Count; i++)
        {
            if (prefabs[i].PrefabTypeName == typeName)
            {
                return GetFromPool(i, id);
            }
        }
        Debug.Log(string.Format("Pool error, typeName not found: {0}", typeName));
        return null;
    }

    public Poolable GetFromPool(int type, string prefabName)
    {
        for (int j = 0; j < prefabs[type].Prefabs.Count; j++)
        {
            if (prefabs[type].Prefabs[j].name == prefabName)
            {
                return GetFromPool(type, j);
            }
        }
        Debug.Log(string.Format("Pool error, prefabName not found: {0}", prefabName));
        return null;
    }
}