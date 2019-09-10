using UnityEngine;

public class PlayerAttack : MonoBehaviour  // милии атака по клику
{
    [Header("Stats")]
    public int Damage;
    public int BackstabDamage;
    private float _pushForce;
    public float AttackRange;
    public int Poisondamage;
    public int PoisonDamagePeriod;
    public LayerMask WhatIsEnemy;

    public Transform AttackPos;
    private PlayerController _player;
    private EnemyController _enemy;
    private Animator _animator;
    [HideInInspector] public Animation AttackAnim;

    [Header("Debug")]
    bool TakeNormalDamage = false;
    bool TakePoisonDamage = false;
    bool TakebackstabDamage = false;
    private void Start()
    {
        _pushForce = 13;
        _animator = GetComponent<Animator>();
        AttackAnim = transform.Find("playerAttackAnim").gameObject.GetComponent<Animation>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _enemy = GameObject.FindGameObjectWithTag("Player").GetComponent<EnemyController>();
        Physics2D.queriesStartInColliders = false;
    }
    void Attacks()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            AttackAnim.Play();
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(AttackPos.position, AttackRange, WhatIsEnemy);

            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                if (enemiesToDamage[i].CompareTag("Enemy"))
                {
                    enemiesToDamage[i].GetComponent<EnemyController>().HealthBar.gameObject.SetActive(true);

                    //enemiesToDamage[i].GetComponent<Rigidbody2D>().AddForce(enemiesToDamage[i].GetComponent<Vectors>().directionPE * pushForce, ForceMode2D.Impulse);
                    if (_player.Direction == 3)
                    {
                        enemiesToDamage[i].GetComponent<Rigidbody2D>().AddForce(Vector2.left * _pushForce + Vector2.up * 5f, ForceMode2D.Impulse);
                    }

                    else if (_player.Direction == 4)
                    {
                        enemiesToDamage[i].GetComponent<Rigidbody2D>().AddForce(Vector2.right * _pushForce + Vector2.up * 5f, ForceMode2D.Impulse);
                    }

                    if (_player.PoisonState && !enemiesToDamage[i].GetComponent<EnemyController>().PoisonEnemyStatus)
                    {
                        enemiesToDamage[i].GetComponent<EnemyController>().TakePoisonDamage(Poisondamage, PoisonDamagePeriod);
                        TakePoisonDamage = true;
                        Debug.Log($"TakePoisonDamage = {TakePoisonDamage}");
                    }
                    else if (enemiesToDamage[i].GetComponent<Vectors>().Backstapble())
                    {
                        enemiesToDamage[i].GetComponent<EnemyController>().TakeBackstabDamage(BackstabDamage);
                        TakebackstabDamage = true;
                        Debug.Log($"TakebackstabDamage = {TakebackstabDamage}");
                    }
                    else
                    {
                        enemiesToDamage[i].GetComponent<EnemyController>().TakeNormalDamage(Damage);
                        TakeNormalDamage = true;
                        Debug.Log($"TakeNormalDamage = {TakeNormalDamage}");
                    }
                }
                if (enemiesToDamage[i].CompareTag("Boss"))
                {
                    enemiesToDamage[i].GetComponent<Boss>().HealthBar.gameObject.SetActive(true);

                    if (_player.PoisonState && !enemiesToDamage[i].GetComponent<Boss>().PoisonEnemyStatus)
                    {
                        enemiesToDamage[i].GetComponent<Boss>().TakePoisonDamage(Poisondamage, PoisonDamagePeriod);
                        TakePoisonDamage = true;
                        Debug.Log($"TakePoisonDamage = {TakePoisonDamage}");
                    }
                    else
                    {
                        enemiesToDamage[i].GetComponent<Boss>().TakeNormalDamage(Damage);
                        TakeNormalDamage = true;
                        Debug.Log($"TakeNormalDamage = {TakeNormalDamage}");
                    }
                }
            }
        }
    }
    private void Update()
    {
        Attacks();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPos.position, AttackRange);
    }
}
