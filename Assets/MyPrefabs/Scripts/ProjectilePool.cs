using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour // пул проджектайлов
{
    public Transform Pos;
    public int PoolSize;
    public GameObject Prefab;
    public float FireRate;
    public float FireSpeed;
    public int FireDamage;
    public int InvokeTimer;
    private GameObject _objectToSpawn;
    private GameObject _obj;
    public bool DirLeft; //направление выстрела
    public bool DirRight;
    public bool DirUp;
    public bool DirDown;
    [HideInInspector]
    public List<GameObject> objectpool;

    private void Start()
    {
        objectpool = new List<GameObject>(); // пул проджектайлов
        for (int i = 0; i < PoolSize; i++)
        {
            if (DirLeft)
            {
                _obj = Instantiate(Prefab, Pos.position, Quaternion.Euler(0, 180, 0));
            }
            else if (DirRight)
            {
                _obj = Instantiate(Prefab, Pos.position, Quaternion.Euler(0, 0, 0));
            }
            else if (DirUp)
            {
                _obj = Instantiate(Prefab, Pos.position, Quaternion.Euler(0, 0, 90));

            }
            else if (DirDown)
            {
                _obj = Instantiate(Prefab, Pos.position, Quaternion.Euler(0, 0, -90));
            }
            objectpool.Add(_obj);
        }
        InvokeRepeating("Spawn", InvokeTimer, FireRate);
    }
    public void AddToPool(GameObject obj) // возвращаем проджектайлы в очередь
    {
        obj.SetActive(false);
    }
    public void Spawn() // спамим проджектайлы
    {
        for (int i = 0; i < objectpool.Count; i++)
        {
            if (!objectpool[i].gameObject.activeInHierarchy)
            {
                objectpool[i].transform.position = Pos.position;
                objectpool[i].SetActive(true);

                if (DirLeft)
                {
                    objectpool[i].GetComponent<TrapFireBall>().Launch(Vector2.left, FireSpeed);
                }
                else if (DirRight)
                {
                    objectpool[i].GetComponent<TrapFireBall>().Launch(Vector2.right, FireSpeed);
                }
                else if(DirUp)
                {
                    objectpool[i].GetComponent<TrapFireBall>().Launch(Vector2.up, FireSpeed);
                }
                else if (DirDown)
                {
                    objectpool[i].GetComponent<TrapFireBall>().Launch(Vector2.down, FireSpeed);
                }
                break;
            }
        }
    }
}
