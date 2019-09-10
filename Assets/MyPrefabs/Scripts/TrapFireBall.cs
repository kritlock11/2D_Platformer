using UnityEngine;

public class TrapFireBall : MonoBehaviour // проджектайлы фаер трап
{
    private Rigidbody2D _rb;
    public ParticleSystem _ProjectileVFX;
    private ProjectilePool _pool;


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _pool = GameObject.FindGameObjectWithTag("FireTrap").GetComponent<ProjectilePool>();
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
                other.gameObject.GetComponent<PlayerController>().TakeNormalDamage(_pool.FireDamage);
            }
            Instantiate(_ProjectileVFX, transform.position, transform.rotation);// партиклы дестроятся сами /стопЭкшн - дестрой
            _pool.AddToPool(gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            if (other != null)
            {
                other.gameObject.GetComponent<EnemyController>().HealthBar.gameObject.SetActive(true);
                other.gameObject.GetComponent<EnemyController>().TakeNormalDamage(_pool.FireDamage);
                Instantiate(_ProjectileVFX, transform.position, transform.rotation);// партиклы дестроятся сами /стопЭкшн - дестрой
                _pool.AddToPool(gameObject);

            }
            //FireTrap.ReturnToPool(gameObject);
        }
        else if (other.gameObject.CompareTag("Map") || other.gameObject.CompareTag("fireballTrap") || other.gameObject.CompareTag("projectileKnife"))
        {
            if (other != null)
            {
                Instantiate(_ProjectileVFX, transform.position, transform.rotation);// партиклы дестроятся сами /стопЭкшн - дестрой
                _pool.AddToPool(gameObject);
            }
        }
    }
}
