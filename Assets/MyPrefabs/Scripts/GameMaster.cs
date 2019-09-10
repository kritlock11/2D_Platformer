using UnityEngine;

public class GameMaster : MonoBehaviour  // это для чекпоинтов, чтобы не сгорала LastCheckPointPos при лоаде чекпоинта
{
    private static GameMaster instanse;
    public Transform PlayerSpawnPos;
    [HideInInspector]
    public Vector2 LastCheckPointPos;
    private void Awake()
    {
        LastCheckPointPos = PlayerSpawnPos.transform.position;
        if (instanse == null)
        {
            instanse = this;
            DontDestroyOnLoad(instanse);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private float fps;
    private void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 120, 25), "FPS = " + fps);
    }
    private void Update()
    {
        fps = 1 / Time.deltaTime;


        Debug.Log(fps);
    }


}
