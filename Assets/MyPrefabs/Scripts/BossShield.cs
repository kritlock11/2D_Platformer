using UnityEngine;

public class BossShield : MonoBehaviour
{
    private float dmg = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
                other.GetComponent<PlayerController>().CurrentHealth -= dmg;

        }
    }
}
