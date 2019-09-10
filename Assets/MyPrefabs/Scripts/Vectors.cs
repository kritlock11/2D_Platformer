using UnityEngine;
using UnityEngine.UI;

public class Vectors : MonoBehaviour // просто вектор откуда/ куда для других скриптов    / дистанс между точками 
{
    private EnemyController _enemy;
    private Boss _boss;
    public Slider EnemyHpBar;
    public Slider BossHpBar;

    [HideInInspector] public Vector3 From;
    [HideInInspector] public Vector3 To;
    [HideInInspector] public Vector3 DirectionFromTo;

    private float _vLeft;
    private float _vRight;
    [HideInInspector] public float DistanseFromTo;

    public Transform PointFrom;
    public Transform PointTo;

    private void Start()
    {
        _enemy = GetComponent<EnemyController>();
        _boss = GetComponent<Boss>();
    }

    void Update()
    {
        DrawVectorLines();
    }

    void DrawVectorLines()
    {
        if (PointFrom != null && PointTo != null)
        {
            From = PointFrom.position;
            To = PointTo.position;

            DirectionFromTo = To - From;
            DirectionFromTo.Normalize();

            _vLeft = Vector2.Dot(DirectionFromTo, new Vector2(1, 0));
            _vRight = Vector2.Dot(DirectionFromTo, new Vector2(-1, 0));
            DistanseFromTo = (From - To).sqrMagnitude;

            if (DistanseFromTo > 100)
            {
                EnemyHpBar.gameObject.SetActive(false);
            }
        }
    }

    public bool Backstapble()
    {
        if ((_vLeft <= 0 && !_enemy.MoovingLeft) || (_vRight <= 0 && _enemy.MoovingLeft))
        {
            return true;
        }
        return false;
    }
    public bool BossBackstapble()
    {
        if ((_vLeft <= 0 && !_boss.MoovingLeft) ||  (_vRight <= 0 && _boss.MoovingLeft))
        {
            return true;
        }
        return false;
    }
}
