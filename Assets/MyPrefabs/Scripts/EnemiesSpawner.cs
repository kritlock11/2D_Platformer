using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    ObjectPooler objectPooler;
    public Transform[] spawnerPos;
    private void Start()
    {
        objectPooler = ObjectPooler.instans;
        //InvokeRepeating("Spawn", 1, 3);

    }

    void FixedUpdate()
    {
        Spawn();
    }


    void Spawn()
    {
        for (int i = 0; i < spawnerPos.Length; i++)
        {
            objectPooler.SpawnFromPool("deq", spawnerPos[0].position, Quaternion.identity);
        }
    }
}
