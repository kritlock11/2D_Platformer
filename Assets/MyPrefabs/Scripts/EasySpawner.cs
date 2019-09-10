using UnityEngine;

public class EasySpawner : MonoBehaviour  // простенький спавн врагов
{
    public int poolSize;
    public GameObject prefab;
    private GameObject[] enemy;

    public Transform spawnerPos;
    private int currentEnemy = 0;

    private void Start()
    {
        enemy = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            enemy[i] = Instantiate(prefab, spawnerPos.position, Quaternion.identity);
            enemy[i].SetActive(false);
        }

        InvokeRepeating("Spawn", 3, 3);

    }
    void Spawn()
    {
        enemy[currentEnemy].SetActive(true);
        currentEnemy++;
        if (currentEnemy >= poolSize)
        {
            currentEnemy = 0;
        }

    }
}


