using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public int size;
        public GameObject prefab;
        public Transform container;
    }

    //public Transform poolContainer;
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public Material[] entityMaterials;

    #region Singleton
    public static ObjectPooler instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public delegate void ObjectDespawned();
    public static event ObjectDespawned onObjectDespawned;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();

            for(int i = 0; i < pool.size; i++)
            {
                GameObject newObject = Instantiate(pool.prefab);

                if(pool.tag != "EntityDestroyed")
                {
                    // Random colors
                    int randomIndex = Random.Range(0, entityMaterials.Length);
                    newObject.GetComponent<SpriteRenderer>().material = entityMaterials[randomIndex];
                }

                newObject.transform.SetParent(pool.container);

                newObject.SetActive(false);

                objectQueue.Enqueue(newObject);
            }

            poolDictionary[pool.tag] = objectQueue;
        }
    }

    public GameObject SpawnFromPool(string poolTag, Vector3 spawnPosition, Quaternion rotation)
    {
        GameObject newEntity = poolDictionary[poolTag].Dequeue();

        newEntity.transform.position = spawnPosition;
        newEntity.transform.rotation = rotation;

        newEntity.SetActive(true);

        return newEntity;
    }

    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        poolDictionary[tag].Enqueue(objectToReturn);

        objectToReturn.SetActive(false);

        onObjectDespawned();
    }
}
