using UnityEngine;

public class Projectile : MonoBehaviour //проджектайлы персонажей
{
    private Rigidbody2D _rb;
    public int ProjectileDamage;
    public ParticleSystem _ProjectileVFX;


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    public void Launch(Vector2 direction, float force)
    {
        _rb.AddForce(direction * force);
        Instantiate(_ProjectileVFX, transform.position, Quaternion.identity);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other != null)
            {
                other.gameObject.GetComponent<PlayerController>().TakeNormalDamage(ProjectileDamage);
            }
            Instantiate(_ProjectileVFX, transform.position, Quaternion.identity); // стопЭкшн - дестрой
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            if (other != null)
            {
                other.gameObject.GetComponent<EnemyController>().HealthBar.gameObject.SetActive(true);
                other.gameObject.GetComponent<EnemyController>().TakeNormalDamage(ProjectileDamage);
                Instantiate(_ProjectileVFX, transform.position, Quaternion.identity);// стопЭкшн - дестрой
                Destroy(gameObject, 0.1f);
            }
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Boss"))
        {
            if (other != null)
            {
                other.gameObject.GetComponent<Boss>().HealthBar.gameObject.SetActive(true);
                other.gameObject.GetComponent<Boss>().TakeNormalDamage(ProjectileDamage);
                Instantiate(_ProjectileVFX, transform.position, Quaternion.identity);// стопЭкшн - дестрой
                Destroy(gameObject, 0.1f);
            }
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Map") || other.gameObject.CompareTag("projectileKnife"))
        {
            if (other != null)
            {
                Destroy(gameObject, 0.2f);
                Instantiate(_ProjectileVFX, transform.position, Quaternion.identity);
            }
        }
    }
}



