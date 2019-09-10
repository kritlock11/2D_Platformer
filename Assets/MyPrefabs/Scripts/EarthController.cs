using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthController : MonoBehaviour // пул планок по которым прыгает перс / по очереди рандомно меняют позицию
{
    public int poolSize = 5;
    public GameObject prefab;
    public float rate;
    public float miny;
    public float minx;
    public float maxy;
    public float maxx;
    public float offset;

    private GameObject[] plates;
    private Vector2 position = new Vector2(100, 14);
    private float timeSinceLastSpawned;

    private int currentPlate = 0;


    private void Start()
    {
        plates = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            plates[i] = Instantiate(prefab, position, Quaternion.identity); 
        }
    }

    private void Update()
    {
        timeSinceLastSpawned += Time.deltaTime;
        if (timeSinceLastSpawned >= rate)
        {
            timeSinceLastSpawned = 0;
            float spawnYpos = Random.Range(miny+ offset, maxy);
            float spawnXpos = Random.Range(minx + offset, maxx);
            plates[currentPlate].transform.position = new Vector2(spawnXpos, spawnYpos); // поочередно меняем поозицию
            currentPlate++;
            if (currentPlate >= poolSize)
            {
                currentPlate = 0;
            }
        }
    }
}
