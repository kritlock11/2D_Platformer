using UnityEngine;

public class GhostDash : MonoBehaviour  // даш сквозь врага
{
    private GameObject target;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (target.gameObject.GetComponent<PlayerController>().IsDashing && other.gameObject.CompareTag("Enemy"))
        {
            target.GetComponent<Rigidbody2D>().gravityScale = 0;
            target.GetComponent<CapsuleCollider2D>().isTrigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            target.GetComponent<Rigidbody2D>().gravityScale = 5;
            target.GetComponent<CapsuleCollider2D>().isTrigger = false;
        }
    }
}
