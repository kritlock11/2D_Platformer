using UnityEngine;

public class Sphere : MonoBehaviour  //сфера рандомно перемещается и стреляет в сторону игрока  
{
    public int PoolSize;
    public GameObject Prefab;
    public GameObject SpearPrefab;
    public float Rate;
    public Transform PosL;
    public Transform PosR;

    private Vector3 curPos;
    private GameObject[] _sphere;
    private float timeSinceLastSpawned;
    private int _currentSphere = 0;
    private Vector2 _dir;
    public Transform PointFrom;
    public Transform PointTo;
    [HideInInspector] public Vector3 From;
    [HideInInspector] public Vector3 To;
    [HideInInspector] public Vector3 DirFromTo;
    [HideInInspector] public float DistanseFromTo;
    private float _angle;

    private void Start()
    {
        _sphere = new GameObject[PoolSize];
        for (int i = 0; i < PoolSize; i++)
        {
            _sphere[i] = Instantiate(Prefab, curPos, Quaternion.identity);
            _sphere[i].SetActive(false);
        }
    }
    private void Update()
    {
        Vector3 v = PosL.position;
        Vector3 v2 = PosR.position;
        timeSinceLastSpawned += Time.deltaTime;
        if (timeSinceLastSpawned >= Rate)
        {
            Launch();
            timeSinceLastSpawned = 0;
            float r = Random.Range(0, 2);
            curPos = r == 0 ? v : v2;

            _sphere[_currentSphere].transform.position = curPos;
            _sphere[_currentSphere].SetActive(true);
            _currentSphere++;
            if (_currentSphere >= PoolSize)
            {
                _currentSphere = 0;
            }
        }
    }
    void Launch()
    {
        _sphere[_currentSphere].SetActive(false);
        From = PointFrom.position;
        To = PointTo.position;
        DirFromTo = From - curPos;
        DirFromTo.Normalize();
        Debug.DrawLine(curPos, From, Color.red);
        _angle = Mathf.Atan2(DirFromTo.y, DirFromTo.x) * Mathf.Rad2Deg;
        GameObject projectileObject = Instantiate(SpearPrefab, curPos, Quaternion.Euler(0, 0, _angle));
        BossFireBall projectile = projectileObject.GetComponent<BossFireBall>();
        projectile.Launch(DirFromTo, 500);
    }
}
