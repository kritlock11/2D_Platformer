using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public float Speed;
    //[HideInInspector]
    public float CurrentHealth;
    public int MaxHealth;
    public Slider HealthBar;

    [Header("Ranged")]
    public GameObject ProjectilePrefab;

    [Header("Jumping")]

    private float _x;
    private float _y;
    public bool OnGround;
    private bool _wallGrab;
    private bool _onWallRight;
    private bool _onWallLeft;
    public float JumpForce;
    public float DoubleJumpForce;
    public float ClimbJumpForce;
    public float CheckRadius;
    private float _jumpTimerCounter;
    public float JumpTime;
    private bool _isJumping;
    private int _extraJumps;
    public int ExtraJumpsValue;
    public LayerMask WhatIsGround;
    public LayerMask WhatIsEnemy;
    private float _fallMultiplier = 0.5f;
    private float _colRadius = 0.15f;
    private float _curSlideSpeed = 2f;
    private float _grabSpeed = 3f;

    private BarellPusher _explosion;

    [Header("DashController")]
    private float _dashTimerCounter;
    public float DashTime;
    public bool IsDashing;
    public float DashSpeed;
    public float StartDashtime;
    [HideInInspector]
    public int Direction;

    [Header("ParticleSystem")]
    public ParticleSystem FeetDust;
    public ParticleSystem PoofUp;
    public ParticleSystem PoofDown;
    public ParticleSystem Dash;
    public ParticleSystem HpPotActivation;
    public ParticleSystem PoisonPotActivation;
    public ParticleSystem StealthActivation;

    [Header("Emoji")]
    public Transform EmojiPos;
    public GameObject EmojiImage;

    [HideInInspector] public float KeyCounter = 0;
    [HideInInspector] public float HpPotionCounter = 0;
    [HideInInspector] public float PoisonPotionCounter = 0;
    [HideInInspector] public float CoinCounter = 0;
    [HideInInspector] public float SmokeBombCounter = 0;

    [Header("HealOverTime")]
    public int HealOverTimeAmount;
    public int HealOverTimePeriod;

    [Header("CamScreenShake")]
    [HideInInspector] public CamShake _shake;
    private bool _groundShakeTrigger;

    private Boom _boomAnim;

    [Header("StealthState")]
    public bool StealthState;
    public float SmokeTimer;
    private float _stealthTimer;

    [Header("PoisonState")]
    public bool PoisonState;
    public float PoisonTimer;
    private float _poisonBuffTimer;

    [Header("References")]
    [HideInInspector] public Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spritePlayerWeapon;
    public SpriteRenderer SpritePlayerHat;
    public SpriteRenderer SpritePlayerFace;
    public SpriteRenderer SpritePlayerEyes;
    public Sprite[] Sprites = new Sprite[5];

    public Vector2 bottomOffset, rightOffset, leftOffset;
    public Transform ProjectilePos;
    public Transform GhostDashPos;
    public Boss Boss;
    private bool _bdead;
    private ResurseCounterController _potions;
    private bool isfeetDust;

    [Header("AirAttack")]
    public TextMeshProUGUI AttackTextBox;
    public float AttackTextTimer;
    private float _attackTextTimerStart;
    public bool IsAirAttack;
    private string[] _symbols = { "W", "S", "A", "D" };
    public bool HasActiveSym;

    private float _jumpSpeed = 5;
    private bool _wallJump;

    [Header("CheckPoints")]
    private GameMaster _gm;
    //easy Flip
    //private bool facingRight = true;
    //void Flip()
    //{
    //    facingRight = !facingRight;
    //    Vector2 scaler = transform.localScale;
    //    scaler.x *= -1;
    //    transform.localScale = scaler;
    //}

    void Start()
    {
        Boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        //EmojiSpawn();
        _gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        transform.position = _gm.LastCheckPointPos;
        //WeaponSprites = new Sprite[2];
        _attackTextTimerStart = AttackTextTimer;
        AttackTextBox.text = "";
        _poisonBuffTimer = PoisonTimer;
        _stealthTimer = SmokeTimer;
        CurrentHealth = MaxHealth;
        _extraJumps = ExtraJumpsValue;
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _explosion = transform.Find("EpicenterPos").GetComponent<BarellPusher>();
        //_spritePlayerWeapon = GetComponent<SpriteRenderer>();
        _shake = GameObject.FindGameObjectWithTag("camRig").GetComponent<CamShake>();
        _boomAnim = GameObject.FindGameObjectWithTag("explosion").GetComponent<Boom>();
        _spritePlayerWeapon = transform.GetChild(4).GetChild(0).GetComponent<SpriteRenderer>();
        SpritePlayerHat = transform.GetChild(5).GetComponent<SpriteRenderer>();
        SpritePlayerFace = transform.GetChild(5).GetChild(0).GetComponent<SpriteRenderer>();
        SpritePlayerEyes = transform.GetChild(5).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        AirAttack();
        StealthBuffTimerOn();
        PoisonBuffTimerOn();
        InstantiateMoveJumpParticles();

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            _animator.SetBool("isRuning", true);
        }
        else
        {
            _animator.SetBool("isRuning", false);
        }
    }

    void Update()
    {
        _x = Input.GetAxis("Horizontal");
        _y = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(_x, _y);
        Moving(dir);
        Dushing();
        HpBarCurrentHealth();
        WallGrab();
        WallJumping();
        Jumping();
        Die();
        //WallSlide();

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Launch();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if ((CurrentHealth < MaxHealth) && HpPotionCounter > 0)
            {
                TakeHealOverTime(HealOverTimeAmount, HealOverTimePeriod);
                HpPotionCounter--;
                Instantiate(HpPotActivation, (Vector2)transform.position + Vector2.up * 0.3f, Quaternion.identity); // стопЭкшн - дестрой
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && SmokeBombCounter > 0)
        {
            Instantiate(StealthActivation, (Vector2)transform.position + Vector2.up * 0.7f, Quaternion.identity); // стопЭкшн - дестрой
            StealthState = true;
            SmokeBombCounter--;
            _spritePlayerWeapon.sprite = Sprites[2];
            Debug.Log($"stealthState = {StealthState}");
        }
        if (Input.GetKeyDown(KeyCode.E) && PoisonPotionCounter > 0)
        {
            Instantiate(PoisonPotActivation, (Vector2)transform.position + Vector2.up * 0.7f, Quaternion.identity); // стопЭкшн - дестрой
            PoisonState = true;
            PoisonPotionCounter--;
            _spritePlayerWeapon.sprite = Sprites[1];
            //spritePlayerWeapon.sprite = poisonWeaponStatus == true ? weaponSprites[1] : weaponSprites[0];
            Debug.Log($"poisonWeaponStatus == {PoisonState}");
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _animator.SetTrigger("isAttacking");
        }

        if (Boss.BossDead)
        {
            if (!_bdead)
            {
                //_shake.CamScreenBossShake();
            }
            _bdead = true;
        }
    }

    void StealthBuffTimerOn()
    {
        if (StealthState)
        {
            _stealthTimer -= Time.deltaTime;
            if (_stealthTimer < 0)
            {
                StealthState = false;
                _stealthTimer = SmokeTimer;
                _spritePlayerWeapon.sprite = Sprites[0];

            }
        }
    }
    void PoisonBuffTimerOn()
    {
        if (PoisonState)
        {
            _poisonBuffTimer -= Time.deltaTime;
            if (_poisonBuffTimer < 0)
            {
                PoisonState = false;
                _poisonBuffTimer = PoisonTimer;
                _spritePlayerWeapon.sprite = Sprites[0];

            }
        }
    }
    public void LoadLastCheckPoint()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void Moving(Vector2 dir)
    {
        _rb.velocity = new Vector2(dir.x * Speed, _rb.velocity.y);

        if (!Mathf.Approximately(dir.x, 0.0f))
        {
            _animator.SetBool("idleLeft", true);
            if (dir.x < 0)
            {
                transform.eulerAngles = new Vector2(0, 180);
                Direction = 3;
            }
            else
            {
                transform.eulerAngles = new Vector2(0, 0);
                Direction = 4;
            }
        }
    }
    void Dushing()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            IsDashing = true;
            _dashTimerCounter = DashTime;
            if (Direction == 3)
            {
                //_rb.velocity = Vector2.zero;
                _rb.velocity = new Vector2(-1, _rb.velocity.y).normalized * DashSpeed;
            }

            else if (Direction == 4)
            {
                //_rb.velocity = Vector2.zero;
                _rb.velocity = new Vector2(1, _rb.velocity.y).normalized * DashSpeed;
            }
        }

        if (Input.GetKey(KeyCode.LeftShift) && IsDashing)
        {

            if (_dashTimerCounter > 0)
            {
                Dash.Play();

                if (Direction == 3)
                {
                    _rb.velocity = Vector2.zero;
                    _rb.velocity = new Vector2(-1, _rb.velocity.y).normalized * DashSpeed;
                }

                else if (Direction == 4)
                {
                    _rb.velocity = Vector2.zero;
                    _rb.velocity = new Vector2(1, _rb.velocity.y).normalized * DashSpeed;
                }
                _dashTimerCounter -= Time.deltaTime;
            }
            else
            {
                IsDashing = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            IsDashing = false;
        }

    }
    void WallGrab()
    {
        _wallGrab = ((_onWallRight || _onWallLeft) && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W)) ||
                    ((_onWallRight || _onWallLeft) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W));
        if (_wallGrab)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _y * _grabSpeed);
        }
    }
    void WallSlide()
    {
        if ((_onWallRight || _onWallLeft) && !OnGround && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)))
        {
            _rb.velocity = new Vector2(_rb.velocity.x, -_curSlideSpeed);
        }
    }
    void WallJumping()
    {
        if ((_onWallLeft && Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.D) && !OnGround) ||
           (_onWallRight && Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.A) && !OnGround))
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0) + Vector2.up * ClimbJumpForce;
        }
        if ((Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.D)) && Input.GetKey(KeyCode.A))
        {
            _rb.velocity = Vector2.zero;
        }
    }
    void Jumping()
    {
        OnGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, CheckRadius, WhatIsGround);
        _onWallRight = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, CheckRadius, WhatIsGround);
        _onWallLeft = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, CheckRadius, WhatIsGround);

        if (_rb.velocity.y < 0) //zamedlennoe padenie
        {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
        }
        if (OnGround)
        {
            _extraJumps = ExtraJumpsValue;
        }
        //if (onGround && Input.GetKeyDown(KeyCode.Space) )

        if (Input.GetKeyDown(KeyCode.Space) && (OnGround || (_onWallRight && OnGround) || (OnGround && _onWallLeft)))
        {
            _groundShakeTrigger = false;
            _isJumping = true;
            _jumpTimerCounter = JumpTime;
            _rb.velocity = new Vector2(_rb.velocity.x, 0) + Vector2.up * JumpForce;

            PoofUp.Play();
        }
        if (Input.GetKey(KeyCode.Space) && _isJumping)
        {
            if (_jumpTimerCounter > 0)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, 0) + Vector2.up * JumpForce;

                _jumpTimerCounter -= Time.deltaTime;
            }
            else
            {
                _isJumping = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _isJumping = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Input.GetKey(KeyCode.S))
            {
                _groundShakeTrigger = true;
                _rb.velocity = Vector2.down * DoubleJumpForce * 2;
            }

            else if (_jumpTimerCounter > 0 && _extraJumps > 0)
            {
                _rb.velocity = Vector2.up * DoubleJumpForce;
                _extraJumps--;
            }
        }
    }
    public void GetSym()
    {
        if (IsAirAttack && !OnGround && !HasActiveSym)
        {
            int index = Random.Range(0, 4);
            string randomSymbol = _symbols[index];
            AttackTextBox.text = randomSymbol;
            HasActiveSym = true;
        }
    }
    void AirAttack()
    {
        Collider2D[] onEnemyHead = Physics2D.OverlapCircleAll((Vector2)transform.position + bottomOffset, CheckRadius, WhatIsEnemy);

        if (!OnGround)
        {
            IsAirAttack = true;
            for (int i = 0; i < onEnemyHead.Length; i++)
            {
                if (onEnemyHead[i].CompareTag("Enemy"))
                {
                    _attackTextTimerStart -= Time.deltaTime;
                    if (_attackTextTimerStart >= 0)
                    {
                        GetSym();
                        if (AttackTextBox.text == "W" && Input.GetKeyDown(KeyCode.W) ||
                            AttackTextBox.text == "S" && Input.GetKeyDown(KeyCode.S) ||
                            AttackTextBox.text == "A" && Input.GetKeyDown(KeyCode.A) ||
                            AttackTextBox.text == "D" && Input.GetKeyDown(KeyCode.D))
                        {
                            AttackTextBox.text = "";
                            onEnemyHead[i].GetComponent<EnemyController>().AirAttackVFX.Play();
                            onEnemyHead[i].GetComponent<EnemyController>().HealthBar.gameObject.SetActive(true);
                            onEnemyHead[i].GetComponent<EnemyController>().CurrentHealth -= onEnemyHead[i].GetComponent<EnemyController>().MaxHealth / 2;
                        }
                    }
                    else
                    {
                        IsAirAttack = false;
                        HasActiveSym = false;
                        AttackTextBox.text = "";
                    }
                }
            }
        }
        else
        {
            IsAirAttack = false;
            HasActiveSym = false;
            AttackTextBox.text = "";
            _attackTextTimerStart = AttackTextTimer;
        }
    }
    public void TakeNormalDamage(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, MaxHealth);
        Debug.Log(CurrentHealth + "/" + MaxHealth);
        if (CurrentHealth <= 0) Die();
    }
    void Launch()
    {
        //_shake.CamScreenBossShake();
        GameObject projectileObject = Instantiate(ProjectilePrefab, ProjectilePos.position, transform.rotation);

        Projectile projectile = projectileObject.GetComponent<Projectile>();

        if (Direction == 3)
        {
            projectile.Launch(Vector2.left, 400);
        }

        else if (Direction == 4)
        {
            projectile.Launch(Vector2.right, 400);
        }
    }
    public void EmojiSpawn()
    {
        GameObject emoji = Instantiate(EmojiImage, EmojiPos.position, transform.rotation);
    }
    public void Die()
    {
        if (CurrentHealth <= 0)
        {
            LoadLastCheckPoint();
            //Destroy(gameObject);
            //HealthBar.gameObject.SetActive(false);
        }
    }
    void HpBarCurrentHealth()
    {
        HealthBar.value = CurrentHealth;
    }
    public void TakeHealOverTime(int amount, int healTime)
    {
        StartCoroutine(TakeHealOverTimeCorotine(amount, healTime));
    }
    IEnumerator TakeHealOverTimeCorotine(float healamount, float dutation)
    {
        float amountHealed = 0;
        float HealPerLoop = healamount / dutation;
        while (amountHealed < healamount)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + HealPerLoop, 0, MaxHealth);
            amountHealed += HealPerLoop;
            Debug.Log(CurrentHealth + "/" + MaxHealth + "/" + amountHealed + "/" + dutation);
            yield return new WaitForSeconds(1f);
        }
    }
    void InstantiateMoveJumpParticles()
    {
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && OnGround) FeetDust.Play();
        else FeetDust.Stop();
        if (OnGround)
        {
            if (isfeetDust)
            {
                PoofDown.Play();
                if (_groundShakeTrigger)
                {
                    _explosion.Boom();
                    _shake.CamScreenShake();
                    _boomAnim.BoomAnim();
                }
                isfeetDust = false;
            }
        }
        else isfeetDust = true;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, _colRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, _colRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, _colRadius);
    }
}
