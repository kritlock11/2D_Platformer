using UnityEngine;

public class TrapSpawner : MonoBehaviour //спавним трапы на позиции
{
    public int poolSize;
    public GameObject prefab;
    public GameObject[] Pools;
    public Transform[] spawnerPos;

    private void Start()
    {
        Pools = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            Pools[i] = Instantiate(prefab, spawnerPos[i].position, Quaternion.identity);
            Pools[i].SetActive(false);
        }
        Invoke("Spawn",1);
    }
    void Spawn()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Pools[i].SetActive(true);
        }
    }
}
