using UnityEngine;

public class DedattackAnim : MonoBehaviour  //анимация атаки с ивентом нанесения дмг
{
    EnemyController enemy;
    ParticleSystemRenderer flipRotation;
    float rot;
    private void Start()
    {
        enemy = gameObject.GetComponentInParent<EnemyController>();
        flipRotation = transform.Find("!").transform.Find("AttackA").gameObject.GetComponent<ParticleSystemRenderer>();
    }
    private void FixedUpdate()
    {
        rot = GetComponentInParent<Transform>().rotation.y * 90;
        flipRotation.flip = new Vector3 (0, rot, 0);
    }
    public void Attacked()
    {
        enemy.DealDamage();
    }
}
