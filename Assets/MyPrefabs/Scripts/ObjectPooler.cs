﻿using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour  // пулер на несколько видов врагов/ не использую в игре (нет столько врагов:))
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPooler instans;
    private void Awake()
    {
        instans = this;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;


    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectpool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectpool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectpool);
        }
    }

    public void SpawnFromPool(string tag, Vector2 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            return  ;
        }

        while (poolDictionary[tag].Count>0)
        {
            GameObject objectToSpawn = poolDictionary[tag].Dequeue();
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
        }
        //poolDictionary[tag].Enqueue(objectToSpawn);
    }
}
