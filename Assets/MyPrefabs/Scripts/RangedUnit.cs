//using System.Collections;
//using UnityEngine;
//using UnityEngine.UI;

//public class RangedUnit : MonoBehaviour    // не юзаю
//{
//    [Header("Health")]
//    public float currentHealth;
//    public float helthBeforeTakingDMG;
//    private float timeBeforeTakingDMG;
//    public float DMGtimeReaction;
//    bool attaked = false;
//    public int maxHealth;
//    public int bottleHeal;
//    public bool poisonEnemyStatus;

//    [Header("HpBar")]
//    public Slider healthBar;


//    [Header("Speed")]
//    public float speed;
//    public float normalSpeed;
//    public float burstSpeed;

//    [Header("GroundDetection")]
//    public Transform groundDetector;
//    public Transform wallDetector;
//    public float wallDetectionDistance;
//    public LayerMask whatIsWall;

//    [Header("PlayerDetection")]
//    public Transform playerChase;
//    private float distanseEnemyToPlayer;
//    public float playerDetectionDistanceFront;
//    public float playerDetectionDistanceBack;
//    public LayerMask whatIsPlayer;




//    [Header("Ranged")]
//    public GameObject projectilePrefab;
//    public Transform projectilePos;
//    private Vectors distansToPlayer;
//    public float projectileLaunchDistance;
//    private float projectileLaunchDistanceMultiplier;
//    public float projectileLDM;
//    public float launchDamage;
//    public float timeBtwLaunch;
//    private float timeBtwLaunchCurrent;

//    [Header("StealthDamageMultipliers")]
//    private int stealthDamageMultiplier;
//    public int stealthDM;
//    private int stealthBackstabDamageMultiplier;
//    public int stealthBDM;
//    private int stealthPoisonDamageMultiplier;
//    public int stealthPDM;

//    [Header("Emoji")]
//    public Transform emojiPos;
//    public GameObject emojiImage;

//    Rigidbody2D rigidbody2d;
//    public bool moovingLeft = true;
//    public GameObject loot;

//    void Start()
//    {
//        speed = normalSpeed;
//        currentHealth = maxHealth;

//        helthBeforeTakingDMG = currentHealth;


//        timeBtwLaunchCurrent = timeBtwLaunch;
//        rigidbody2d = GetComponent<Rigidbody2D>();
//        distansToPlayer = GetComponent<Vectors>();
//        playerChase = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

//        Physics2D.queriesStartInColliders = false;
//    }

//    private void Update()
//    {

//        timeBeforeTakingDMG += Time.deltaTime;
//        if (timeBeforeTakingDMG > DMGtimeReaction)
//        {
//            helthBeforeTakingDMG = currentHealth;
//            timeBeforeTakingDMG = 0;
//        }
//        if (helthBeforeTakingDMG > currentHealth)
//        {
//            attaked = true;
//        }
//        Debug.Log($"{attaked},{helthBeforeTakingDMG}, {currentHealth}");


//        if (timeBtwLaunchCurrent > 0) timeBtwLaunchCurrent -= Time.deltaTime;

//        healthBar.value = currentHealth;
//        //healthBar.maxValue = currentHealth;
//    }

//    void FixedUpdate()
//    {
//        PlayerDetection();
//        StealthState();
//    }
//    void EmojiSpawn()
//    {
//        Instantiate(emojiImage, transform.position, transform.rotation);
//    }
//    void Launch()
//    {
//        if (timeBtwLaunchCurrent <= 0)
//        {
//            if (moovingLeft)
//            {
//                GameObject projectileObject = Instantiate(projectilePrefab, projectilePos.position, Quaternion.Euler(0, 180, 0));
//                Projectile projectile = projectileObject.GetComponent<Projectile>();
//                projectile.Launch(Vector2.left, 200);
//            }
//            else if (!moovingLeft)
//            {
//                GameObject projectileObject = Instantiate(projectilePrefab, projectilePos.position, Quaternion.Euler(0, 0, 0));
//                Projectile projectile = projectileObject.GetComponent<Projectile>();
//                projectile.Launch(Vector2.right, 200);
//            }
//            if (playerChase.gameObject.GetComponent<PlayerController>().CurrentHealth <= 0)
//            {
//                playerChase.gameObject.GetComponent<PlayerController>().Die();
//            }
//            timeBtwLaunchCurrent = timeBtwLaunch;
//            EmojiSpawn();

//        }
//    }
//    void LookDirectionChange()
//    {
//        if (moovingLeft == true)
//        {
//            transform.eulerAngles = new Vector2(0, -180);
//            moovingLeft = false;
//        }
//        else
//        {
//            transform.eulerAngles = new Vector2(0, 0);
//            moovingLeft = true;
//        }
//    }

//    void GroundDetection()
//    {
//        speed = normalSpeed;
//        transform.Translate(Vector2.left * speed * Time.deltaTime);
//        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetector.position, Vector2.down, wallDetectionDistance, whatIsWall);
//        Debug.DrawLine(groundDetector.position, groundDetector.position - transform.up * wallDetectionDistance, Color.yellow);
//        RaycastHit2D wallInfo = Physics2D.Raycast(wallDetector.position, Vector2.left, wallDetectionDistance, whatIsWall);
//        Debug.DrawLine(wallDetector.position, wallDetector.position - transform.right * wallDetectionDistance, Color.yellow);
//        //Debug.Log($"wallInfo-{wallInfo.collider},groundInfo- {groundInfo.collider}");
//        if (wallInfo.collider == true || groundInfo.collider == false)
//        {
//            LookDirectionChange();
//        }

//    }


//    void PlayerDetection()
//    {
//        RaycastHit2D playerInfoFront = Physics2D.Raycast(transform.position, -transform.right, playerDetectionDistanceFront, whatIsPlayer);
//        RaycastHit2D playerInfoBack = Physics2D.Raycast(transform.position, transform.right, playerDetectionDistanceBack, whatIsPlayer);
//        //Debug.DrawLine(transform.position, transform.position - transform.right * playerDetectionDistanceFront, Color.green);
//        //Debug.DrawLine(transform.position, playerInfoFront.point, Color.red);

//        if (playerInfoFront.collider != null)
//        {
//            Debug.Log($"playerInfoBack.collider-{playerInfoBack.collider}");


//            if (playerInfoFront.collider.CompareTag("Player"))
//            {
//                //playerChase.GetComponent<PlayerController>().stealthState == false

//                if (distansToPlayer.DistanseFromTo < projectileLaunchDistance * projectileLaunchDistanceMultiplier)
//                {
//                    speed = 0;
//                    Launch();
//                }
//                else
//                {
//                    GroundDetection();
//                }
//            }
//        }
//        else if (playerInfoBack.collider != null)
//        {
//            Debug.DrawLine(transform.position, playerInfoBack.point, Color.red);

//            if (playerInfoBack.collider.CompareTag("Player"))
//            {
//                if (distansToPlayer.DistanseFromTo < projectileLaunchDistance * projectileLaunchDistanceMultiplier || attaked)

//                {
//                    attaked = false;

//                    speed = burstSpeed;
//                    LookDirectionChange();
//                    transform.position = Vector2.MoveTowards(transform.position, playerInfoBack.collider.transform.position, speed * Time.deltaTime);
//                    Launch();
//                }
//                else
//                {
//                    GroundDetection();
//                }
//            }
//        }

//        else GroundDetection();
//    }


//    void StealthState()
//    {
//        if (playerChase.GetComponent<PlayerController>().StealthState)
//        {
//            stealthDamageMultiplier = stealthDM;
//            stealthBackstabDamageMultiplier = stealthBDM;
//            stealthPoisonDamageMultiplier = stealthPDM;

//            projectileLaunchDistanceMultiplier = projectileLDM;
//        }
//        else
//        {
//            stealthDamageMultiplier = 1;
//            stealthBackstabDamageMultiplier = 1;
//            stealthPoisonDamageMultiplier = 1;

//            projectileLaunchDistanceMultiplier = 1;
//        }
//    }


//    //Normal Damage
//    public void TakeNormalDamage(int amount)
//    {
//        currentHealth = Mathf.Clamp(currentHealth - amount * stealthDamageMultiplier, 0, maxHealth);
//        SpriteRenderer enemyBodyColor = rigidbody2d.gameObject.GetComponent<SpriteRenderer>();
//        enemyBodyColor.color = Color.yellow;
//        Debug.Log(currentHealth + "/" + maxHealth);
//        if (currentHealth <= 0) Die();
//    }
//    //Backstab Damage
//    public void TakeBackstabDamage(int amount)
//    {
//        currentHealth = Mathf.Clamp(currentHealth - amount * stealthBackstabDamageMultiplier, 0, maxHealth);
//        var re = rigidbody2d.gameObject.GetComponent<SpriteRenderer>();
//        re.color = Color.red;

//        Debug.Log(currentHealth + "/" + maxHealth);

//        if (currentHealth <= 0) Die();

//    }
//    //Damage over time
//    public void TakePoisonDamage(int amount, int damageTime)
//    {
//        StartCoroutine(TakePoisonDamageCorotine(amount, damageTime));
//    }
//    IEnumerator TakePoisonDamageCorotine(float damageamount, float duration)
//    {
//        poisonEnemyStatus = true;
//        var enemySprite = rigidbody2d.gameObject.GetComponent<SpriteRenderer>();
//        enemySprite.color = Color.green;

//        float amountDamaded = 0;
//        float damagePerLoop = (damageamount * stealthPoisonDamageMultiplier) / duration;
//        while ((amountDamaded < damageamount * stealthPoisonDamageMultiplier)) //|| amountDamaded <= currentHealth
//        {
//            currentHealth = Mathf.Clamp(currentHealth - damagePerLoop, 0, maxHealth);
//            amountDamaded += damagePerLoop;
//            yield return new WaitForSeconds(1f);
//            Debug.Log(currentHealth + "/" + maxHealth + "/" + amountDamaded + "/" + duration);
//            if (currentHealth <= 0)
//            {
//                Die();
//            }
//        }
//        poisonEnemyStatus = false;
//        enemySprite.color = Color.white;
//        //Debug.Log($"poisonEnemyStatus = {poisonEnemyStatus}");
//    }

//    public void Die()
//    {
//        playerChase.transform.GetComponent<PlayerController>().EmojiSpawn();
//        Destroy(gameObject);
//        healthBar.gameObject.SetActive(false);
//        Instantiate(loot, transform.position, transform.rotation);

//    }

//    //private void OnDrawGizmosSelected()
//    //{
//    //    Gizmos.color = Color.blue;
//    //    Gizmos.DrawWireSphere(groundDetector.position, wallDetectionDistance);
//    //}
//}
