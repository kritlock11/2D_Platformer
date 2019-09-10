using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [Header("HpBar")]
    public Slider HealthBar;

    [Header("Health")]
    public float CurrentHealth;
    public int MaxHealth;
    [HideInInspector]
    public float HelthBeforeTakingDamage;
    private float _timeBeforeTakingDMG;
    public float DamagetimeReaction;
    public int BottleHeal;
    [HideInInspector]
    public bool PoisonEnemyStatus;
    private bool _attacked;

    [Header("Speed")]
    public float Speed;
    public float NormalSpeed;
    public float BurstSpeed;

    [Header("GroundDetection")]
    //public Transform GroundDetector;
    public Transform WallDetector;
    public float WallDetectionDistance;
    public LayerMask WhatIsWall;

    [Header("PlayerDetection")]
    [HideInInspector] public Transform PlayerTransform;
    [HideInInspector] public PlayerController PlayerController;
    //public float PlayerDetectionDistanceFront;
    //public float PlayerDetectionDistanceBack;
    public LayerMask WhatIsPlayer;

    [Header("Melee")]
    public float MeleeDamage;
    public float TimeBtwDamage;
    private float _timeBtwDamageCurrent;
    public float FrontAgroDistance;
    private float _frontAgroDistanceMultiplier;
    public float FrontADM;
    public float BackAgroDistance;
    private float _backAgroDistanceMultiplier;
    public float BackADM;

    [Header("Ranged")]
    public GameObject ProjectilePrefab;
    private Vectors _vectors;
    public float ProjectileLaunchDistance;
    private float _projectileLaunchDistanceMultiplier;
    public float ProjectileLDM;
    public int LaunchDamage;
    public float CastTimer;
    private float _castTimer;

    [Header("StealthDamageMultipliers")]
    private int _stealthDamageMultiplier;
    public int StealthDM;
    private int _stealthBackstabDamageMultiplier;
    public int StealthBDM;
    private int _stealthPoisonDamageMultiplier;
    public int StealthPDM;

    //[Header("Emoji")]
    //public Transform EmojiPos;
    //public GameObject EmojiImage;

    private Rigidbody2D _rb;
    private SpriteRenderer _enemyBodyColor;

    [HideInInspector]
    public bool MoovingLeft;
    public GameObject Loot;
    public GameObject DeathAnimation;


    private float _walkTimer;
    public float WalkTimer;
    private bool _casting;
    public ParticleSystem PrefabSphere;
    public ParticleSystem BossShield;
    public GameObject PrefabBall;
    private bool _sphereOn;
    private bool _stormOn;
    private bool _stormOn2;


    public Transform PointFrom;
    public Transform PointTo;
    [HideInInspector] public Vector3 From;
    [HideInInspector] public Vector3 To;
    [HideInInspector] public Vector3 DirFromTo;
    [HideInInspector] public Vector3 DirPush;
    [HideInInspector] public float DistanseFromTo;
    private float _angle;
    private bool _lowHp;
    private bool _hp500;
    private bool _hp400;
    private bool _hp300;
    private bool _hp200;
    private ParticleSystem _bossShield;
    public Transform[] StormPos;
    public bool BossDead;


    void Start()
    {
        _walkTimer = WalkTimer;
        MoovingLeft = true;
        CurrentHealth = MaxHealth;
        Speed = NormalSpeed;
        HelthBeforeTakingDamage = CurrentHealth;
        _timeBtwDamageCurrent = TimeBtwDamage;
        _castTimer = CastTimer;
        _vectors = GetComponent<Vectors>();
        _rb = GetComponent<Rigidbody2D>();
        _enemyBodyColor = GetComponent<SpriteRenderer>();
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();


        Physics2D.queriesStartInColliders = false;
    }

    void Attacked()
    {
        _timeBeforeTakingDMG += Time.deltaTime;
        if (_timeBeforeTakingDMG > DamagetimeReaction)
        {
            HelthBeforeTakingDamage = CurrentHealth;
            _timeBeforeTakingDMG = 0;
        }
        if (HelthBeforeTakingDamage > CurrentHealth)
        {
            _attacked = true;
        }
        if (HelthBeforeTakingDamage == CurrentHealth)
        {
            _attacked = false;
        }
        //Debug.Log($"{_attacked},{HelthBeforeTakingDamage}, {CurrentHealth}");
    }
    private void Update()
    {
        HealthBar.value = CurrentHealth;


        //ChangeMoovingDirection();
        GroundDetection();
        Attacked();
        CheckHpForShield();
        Die();

        if (_casting)
        {
            _castTimer -= Time.deltaTime;
        }

        if (_walkTimer >= 0) _walkTimer -= Time.deltaTime;
        if (_walkTimer <= 0)
        {
            _casting = true;
            Speed = 0;

            if (!_sphereOn)
            {
                ParticleSystem _sphere = Instantiate(PrefabSphere, (Vector2)transform.position + Vector2.up * 1.7f, Quaternion.identity);
                _sphereOn = true;
            }
        }
        if (_castTimer <= 0)
        {
            if (CurrentHealth <= 500 && _stormOn && !_stormOn2)
            {
                for (int i = 0; i < StormPos.Length; i++)
                {
                    GameObject projectileObject = Instantiate(PrefabBall, StormPos[i].transform.position, Quaternion.Euler(0, 0, -90));
                    BossFireBall projectile = projectileObject.GetComponent<BossFireBall>();
                    projectile.Launch(Vector3.down, 100);
                }
                _stormOn2 = true;
            }

            DarkSphereLaunch();
            _sphereOn = false;
            _casting = false;
            _walkTimer = WalkTimer;
            _castTimer = CastTimer;
            Speed = NormalSpeed;
        }
        if (CurrentHealth <= 500)
        {
            if (!_stormOn)
            {
                _castTimer = CastTimer;
                _casting = true;
                Speed = 0;
                WalkTimer = 1;
                for (int i = 0; i < StormPos.Length; i++)
                {
                    Instantiate(PrefabSphere, StormPos[i].transform.position, Quaternion.identity);
                }
            }
            _stormOn = true;
        }
    }

    void FindDirection()
    {
        From = PointFrom.position;
        To = (Vector2)PointTo.position + Vector2.up * 1.7f;
        DirFromTo = From - To;
        DirFromTo.Normalize();
    }

    void CheckHpForShield()
    {
        if (CurrentHealth <= 400)
        {
            if (!_hp400)
            {
                _bossShield = Instantiate(BossShield, transform.position, Quaternion.identity);
                _hp400 = true;
            }

        }
        if (CurrentHealth <= 300)
        {
            if (!_hp300)
            {
                _bossShield = Instantiate(BossShield, transform.position, Quaternion.identity);
                _hp300 = true;
            }

        }
        if (CurrentHealth <= 200)
        {
            if (!_hp200)
            {
                _bossShield = Instantiate(BossShield, transform.position, Quaternion.identity);
                _hp200 = true;
            }

        }
        if (_bossShield != null)
        {

            _bossShield.gameObject.transform.position = transform.position;
        }
    }


    void DarkSphereLaunch()
    {
        FindDirection();
        Debug.DrawLine(To, From, Color.red);
        float _angle = Mathf.Atan2(DirFromTo.y, DirFromTo.x) * Mathf.Rad2Deg;
        GameObject projectileObject = Instantiate(PrefabBall, To, Quaternion.Euler(0, 0, _angle));
        BossFireBall projectile = projectileObject.GetComponent<BossFireBall>();
        projectile.Launch(DirFromTo, 600);
    }

    //public void HalfHpPush()
    //{
    //    Collider2D[] playerInCurcle = Physics2D.OverlapCircleAll(transform.position, 3, WhatIsPlayer);
    //    for (int i = 0; i < playerInCurcle.Length; i++)
    //    {
    //        if (playerInCurcle[i].CompareTag("Player"))
    //        {

    //            Debug.Log($"{playerInCurcle}");
    //            FindDirection();
    //            playerInCurcle[i].GetComponent<Rigidbody2D>().AddForce(Vector2.right * 22, ForceMode2D.Impulse);
    //        }
    //    }

    //}

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, 3);
    //}



    void FixedUpdate()
    {
        //PlayerDetection();
        PlayerStealthState();
    }

    //void EmojiSpawn()
    //{
    //    Instantiate(EmojiImage, transform.position, transform.rotation);
    //}


    void ChangeMoovingDirection()
    {
        if (MoovingLeft == true)
        {
            //transform.Translate(Vector2.left * Speed * Time.deltaTime);

            transform.eulerAngles = new Vector2(0, -180);
            MoovingLeft = false;
        }
        else
        {
            //transform.Translate(Vector2.right * Speed * Time.deltaTime);

            transform.eulerAngles = new Vector2(0, 0);
            MoovingLeft = true;
        }
    }
    void GroundDetection()
    {
        //Speed = NormalSpeed;
        transform.Translate(Vector2.left * Speed * Time.deltaTime);
        //RaycastHit2D groundInfo = Physics2D.Raycast(GroundDetector.position, Vector2.down, WallDetectionDistance, WhatIsWall);
        //Debug.DrawLine(GroundDetector.position, GroundDetector.position - transform.up * WallDetectionDistance, Color.yellow);
        RaycastHit2D wallInfo = Physics2D.Raycast(WallDetector.position, Vector2.left, WallDetectionDistance, WhatIsWall);
        Debug.DrawLine(WallDetector.position, WallDetector.position - transform.right * WallDetectionDistance, Color.yellow);
        //Debug.Log($"wallInfo-{wallInfo.collider},groundInfo- {groundInfo.collider}");
        if (wallInfo.collider == true) //|| groundInfo.collider == false
        {
            ChangeMoovingDirection();
        }
    }


    //void PlayerDetection()
    //{
    //    RaycastHit2D playerInfoFront = Physics2D.Raycast(transform.position, -transform.right, PlayerDetectionDistanceFront, WhatIsPlayer);
    //    RaycastHit2D playerInfoBack = Physics2D.Raycast(transform.position, transform.right, PlayerDetectionDistanceBack, WhatIsPlayer);
    //    Debug.DrawLine(transform.position, transform.position - transform.right * PlayerDetectionDistanceFront, Color.green);
    //    Debug.DrawLine(transform.position, playerInfoFront.point, Color.red);

    //    if (playerInfoFront.collider != null)
    //    {
    //        if (playerInfoFront.collider.CompareTag("Player"))
    //        {
    //            if (_distanseToPlayer.DistanseFromTo < FrontAgroDistance * _frontAgroDistanceMultiplier)
    //            {
    //                if (_distanseToPlayer.DistanseFromTo <= AttackArk * AttackArk)
    //                {
    //                    Speed = 0;
    //                    AttackOnDistance();
    //                }
    //                else
    //                {
    //                    Speed = BurstSpeed;
    //                    transform.position = Vector2.MoveTowards(transform.position, playerInfoFront.collider.transform.position, Speed * Time.deltaTime);
    //                }
    //            }
    //            else if (_distanseToPlayer.DistanseFromTo <= ProjectileLaunchDistance * _projectileLaunchDistanceMultiplier)
    //            {
    //                Speed = 0;
    //                Launch();
    //            }
    //            else
    //            {
    //                GroundDetection();
    //            }
    //        }
    //    }
    //    else if (playerInfoBack.collider != null)
    //    {
    //        //Debug.DrawLine(transform.position, playerInfoBack.point, Color.red);
    //        if (playerInfoBack.collider.CompareTag("Player"))
    //        {
    //            if ((_distanseToPlayer.DistanseFromTo < BackAgroDistance * _backAgroDistanceMultiplier) || _attacked)
    //            {
    //                ChangeMoovingDirection();
    //            }
    //            else
    //            {
    //                GroundDetection();
    //            }
    //        }
    //    }
    //    else GroundDetection();
    //}
    void PlayerStealthState()
    {
        if (PlayerTransform != null && PlayerController.StealthState)
        {
            _stealthDamageMultiplier = StealthDM;
            _stealthBackstabDamageMultiplier = StealthBDM;
            _stealthPoisonDamageMultiplier = StealthPDM;

            _frontAgroDistanceMultiplier = FrontADM;
            _backAgroDistanceMultiplier = BackADM;
            _projectileLaunchDistanceMultiplier = ProjectileLDM;
        }
        else
        {
            _stealthDamageMultiplier = 1;
            _stealthBackstabDamageMultiplier = 1;
            _stealthPoisonDamageMultiplier = 1;

            _frontAgroDistanceMultiplier = 1;
            _backAgroDistanceMultiplier = 1;
            _projectileLaunchDistanceMultiplier = 1;
        }
    }


    //Normal Damage
    public void TakeNormalDamage(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - amount * _stealthDamageMultiplier, 0, MaxHealth);
        //_enemyBodyColor.color = Color.yellow;
    }
    //Backstab Damage
    public void TakeBackstabDamage(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - amount * _stealthBackstabDamageMultiplier, 0, MaxHealth);
        //_enemyBodyColor.color = Color.red;
    }
    //Damage over time
    public void TakePoisonDamage(int amount, int damageTime)
    {
        StartCoroutine(TakePoisonDamageCorotine(amount, damageTime));
    }
    IEnumerator TakePoisonDamageCorotine(float damageamount, float duration)
    {
        PoisonEnemyStatus = true;
        //_enemyBodyColor.color = Color.green;

        float amountDamaded = 0;
        float damagePerLoop = (damageamount * _stealthPoisonDamageMultiplier) / duration;
        while ((amountDamaded < damageamount * _stealthPoisonDamageMultiplier))
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth - damagePerLoop, 0, MaxHealth);
            amountDamaded += damagePerLoop;
            yield return new WaitForSeconds(1f);
            Debug.Log(CurrentHealth + "/" + MaxHealth + "/" + amountDamaded + "/" + duration);
        }
        PoisonEnemyStatus = false;
        //_enemyBodyColor.color = Color.white;
    }

    public void Die()
    {
        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
            HealthBar.gameObject.SetActive(false);
            PlayerController.EmojiSpawn();
            Instantiate(Loot, transform.position + Vector3.up * 1.1f, transform.rotation);
            Instantiate(DeathAnimation, transform.position, transform.rotation);
            BossDead = true;
        }
    }
}
