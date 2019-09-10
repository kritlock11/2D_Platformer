using UnityEngine;

public class BarellPusher : MonoBehaviour
{
    private Vector3 v_epicenter;
    private Vector3 v_enemies;
    private Vector3 v_direction;

    public Transform epicenter;
    public float sphereRadius;
    public float force;
    public LayerMask whatIsEnemy;

    private void FixedUpdate()
    {
        v_epicenter = epicenter.position;
    }
    public void Boom()
    {
        Collider2D[] enemiesInCurcle = Physics2D.OverlapCircleAll(epicenter.position, sphereRadius, whatIsEnemy);
        for (int i = 0; i < enemiesInCurcle.Length; i++)
        {
            if (enemiesInCurcle[i].CompareTag("Enemy"))
            {
                v_enemies = enemiesInCurcle[i].GetComponent<Transform>().position;
                v_direction = v_enemies - v_epicenter;
                v_direction.Normalize();
                enemiesInCurcle[i].GetComponent<Rigidbody2D>().AddForce(v_direction * force, ForceMode2D.Impulse);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(epicenter.position, sphereRadius);
    }
}
