using UnityEngine;

public class BossFireBall : MonoBehaviour // проджектайлы фаер трап
{
    private Rigidbody2D _rb;
    public ParticleSystem _ProjectileVFX;
    private Boss _boss;


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
    }
    public void Launch(Vector2 direction, float force) // лаунчим
    {
        _rb.AddForce(direction * force);
        Instantiate(_ProjectileVFX, transform.position, transform.rotation); // партиклы дестроятся сами /стопЭкшн - дестрой
    }


    void OnCollisionEnter2D(Collision2D other) //при столкновении возвращаем в пул
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other != null)
            {
                other.gameObject.GetComponent<PlayerController>().TakeNormalDamage(_boss.LaunchDamage);
            }
            Instantiate(_ProjectileVFX, transform.position, transform.rotation);// партиклы дестроятся сами /стопЭкшн - дестрой
            Destroy(gameObject);
        }

        else if (other.gameObject.CompareTag("Map") || other.gameObject.CompareTag("bossProjectile") || other.gameObject.CompareTag("projectileKnife"))
        {
            if (other != null)
            {
                Instantiate(_ProjectileVFX, transform.position, transform.rotation);// партиклы дестроятся сами /стопЭкшн - дестрой
                Destroy(gameObject);

            }
        }
    }
}
