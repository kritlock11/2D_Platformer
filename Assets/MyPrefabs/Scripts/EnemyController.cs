using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
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
    public Transform GroundDetector;
    public Transform WallDetector;
    public float WallDetectionDistance;
    public LayerMask WhatIsWall;

    [Header("PlayerDetection")]
    [HideInInspector] public Transform PlayerTransform;
    [HideInInspector] public PlayerController PlayerController;
    public float PlayerDetectionDistanceFront;
    public float PlayerDetectionDistanceBack;
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
    public ParticleSystem AttackVFX;
    public ParticleSystem AirAttackVFX;

    [Header("Ranged")]
    public GameObject ProjectilePrefab;
    public Transform ProjectilePos;
    private Vectors _distanseToPlayer;
    public float AttackArk;
    public float ProjectileLaunchDistance;
    private float _projectileLaunchDistanceMultiplier;
    public float ProjectileLDM;
    public float LaunchDamage;
    public float TimeBtwLaunch;
    private float TimeBtwLaunchCurrent;

    [Header("StealthDamageMultipliers")]
    private int _stealthDamageMultiplier;
    public int StealthDM;
    private int _stealthBackstabDamageMultiplier;
    public int StealthBDM;
    private int _stealthPoisonDamageMultiplier;
    public int StealthPDM;

    [Header("Emoji")]
    public Transform EmojiPos;
    public GameObject EmojiImage;

    private Rigidbody2D _rb;
    private SpriteRenderer _enemyBodyColor;

    [HideInInspector]
    public bool MoovingLeft;
    public GameObject Loot;
    public GameObject DeathAnimation;
    public Animation AttackAnimation;

    void Start()
    {
        MoovingLeft = true;
        CurrentHealth = MaxHealth;
        Speed = NormalSpeed;
        HelthBeforeTakingDamage = CurrentHealth;
        _timeBtwDamageCurrent = TimeBtwDamage;
        TimeBtwLaunchCurrent = TimeBtwLaunch;
        AttackAnimation.GetComponent<Animation>();
        _distanseToPlayer = GetComponent<Vectors>();
        _rb = GetComponent<Rigidbody2D>();
        _enemyBodyColor = GetComponent<SpriteRenderer>();
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();


        Physics2D.queriesStartInColliders = false;
    }
    void AttackOnDistance()
    {
        if (_timeBtwDamageCurrent > 0)
        {
            _timeBtwDamageCurrent -= Time.deltaTime;
        }
        if (_timeBtwDamageCurrent <= 0)
        {
            if (_distanseToPlayer.DistanseFromTo <= AttackArk * AttackArk)
            {
                AttackAnimation.Play();
                _timeBtwDamageCurrent = TimeBtwDamage;
            }
        }
    }
    public void DealDamage()
    {
        if (_distanseToPlayer.DistanseFromTo <= AttackArk * AttackArk)
        {
            PlayerController.CurrentHealth -= MeleeDamage;
        }
    }

    private void Update()
    {
        HealthBar.value = CurrentHealth;
        if (TimeBtwLaunchCurrent > 0) TimeBtwLaunchCurrent -= Time.deltaTime;
        Attacked();
        Die();
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

    void FixedUpdate()
    {
        PlayerDetection();
        PlayerStealthState();
    }
    void EmojiSpawn()
    {
        Instantiate(EmojiImage, transform.position, transform.rotation);
    }
    void Launch()
    {
        if (TimeBtwLaunchCurrent <= 0)
        {
            if (MoovingLeft)
            {
                GameObject projectileObject = Instantiate(ProjectilePrefab, ProjectilePos.position, Quaternion.Euler(0, 180, 0));
                Projectile projectile = projectileObject.GetComponent<Projectile>();
                projectile.Launch(Vector2.left, 200);
            }
            else if (!MoovingLeft)
            {
                GameObject projectileObject = Instantiate(ProjectilePrefab, ProjectilePos.position, Quaternion.Euler(0, 0, 0));
                Projectile projectile = projectileObject.GetComponent<Projectile>();
                projectile.Launch(Vector2.right, 200);
            }
            TimeBtwLaunchCurrent = TimeBtwLaunch;
        }
    }
    void ChangeMoovingDirection()
    {
        if (MoovingLeft == true)
        {
            transform.eulerAngles = new Vector2(0, -180);
            MoovingLeft = false;
        }
        else
        {
            transform.eulerAngles = new Vector2(0, 0);
            MoovingLeft = true;
        }
    }
    void GroundDetection()
    {
        Speed = NormalSpeed;
        transform.Translate(Vector2.left * Speed * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(GroundDetector.position, Vector2.down, WallDetectionDistance, WhatIsWall);
        Debug.DrawLine(GroundDetector.position, GroundDetector.position - transform.up * WallDetectionDistance, Color.yellow);
        RaycastHit2D wallInfo = Physics2D.Raycast(WallDetector.position, Vector2.left, WallDetectionDistance, WhatIsWall);
        Debug.DrawLine(WallDetector.position, WallDetector.position - transform.right * WallDetectionDistance, Color.yellow);
        //Debug.Log($"wallInfo-{wallInfo.collider},groundInfo- {groundInfo.collider}");
        if (wallInfo.collider == true || groundInfo.collider == false)
        {
            ChangeMoovingDirection();
        }
    }


    void PlayerDetection()
    {
        RaycastHit2D playerInfoFront = Physics2D.Raycast(transform.position, -transform.right, PlayerDetectionDistanceFront, WhatIsPlayer);
        RaycastHit2D playerInfoBack = Physics2D.Raycast(transform.position, transform.right, PlayerDetectionDistanceBack, WhatIsPlayer);
        //Debug.DrawLine(transform.position, transform.position - transform.right * playerDetectionDistanceFront, Color.green);
        //Debug.DrawLine(transform.position, playerInfoFront.point, Color.red);

        if (playerInfoFront.collider != null)
        {
            if (playerInfoFront.collider.CompareTag("Player"))
            {
                if (_distanseToPlayer.DistanseFromTo < FrontAgroDistance * _frontAgroDistanceMultiplier)
                {
                    if (_distanseToPlayer.DistanseFromTo <= AttackArk * AttackArk)
                    {
                        Speed = 0;
                        AttackOnDistance();
                    }
                    else
                    {
                        Speed = BurstSpeed;
                        transform.position = Vector2.MoveTowards(transform.position, playerInfoFront.collider.transform.position, Speed * Time.deltaTime);
                    }
                }
                else if (_distanseToPlayer.DistanseFromTo <= ProjectileLaunchDistance * _projectileLaunchDistanceMultiplier)
                {
                    Speed = 0;
                    Launch();
                }
                else
                {
                    GroundDetection();
                }
            }
        }
        else if (playerInfoBack.collider != null)
        {
            //Debug.DrawLine(transform.position, playerInfoBack.point, Color.red);
            if (playerInfoBack.collider.CompareTag("Player"))
            {
                if ((_distanseToPlayer.DistanseFromTo < BackAgroDistance * _backAgroDistanceMultiplier) || _attacked)
                {
                    ChangeMoovingDirection();
                }
                else
                {
                    GroundDetection();
                }
            }
        }
        else GroundDetection();
    }
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
        _enemyBodyColor.color = Color.yellow;
    }
    //Backstab Damage
    public void TakeBackstabDamage(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - amount * _stealthBackstabDamageMultiplier, 0, MaxHealth);
        _enemyBodyColor.color = Color.red;
    }
    //Damage over time
    public void TakePoisonDamage(int amount, int damageTime)
    {
        StartCoroutine(TakePoisonDamageCorotine(amount, damageTime));
    }
    IEnumerator TakePoisonDamageCorotine(float damageamount, float duration)
    {
        PoisonEnemyStatus = true;
        _enemyBodyColor.color = Color.green;

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
        _enemyBodyColor.color = Color.white;
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
        }
    }
}

