using UnityEngine;
using UnityEngine.UI;

public class TrapController : MonoBehaviour // прост наземная трапа наносит дмг
{
    public int trapDamage;
    public GameObject VFX;
    private void Start()
    {
        VFX.GetComponent<GameObject>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other != null)
            {
                other.gameObject.GetComponent<PlayerController>().TakeNormalDamage(trapDamage);
            }
            Destroy(gameObject);
            Instantiate(VFX, transform.position, transform.rotation);

        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            if (other != null)
            {
                other.gameObject.GetComponent<EnemyController>().TakeNormalDamage(trapDamage);
            }
            Destroy(gameObject);
            Instantiate(VFX,transform.position,transform.rotation);
        }
    }
}
