using UnityEngine;

public class PlayerAttackAnim : MonoBehaviour
{
    PlayerController player;
    ParticleSystemRenderer flipRotation;
    float rot;
    Transform perentRotation;
    private void Start()
    {
        player = gameObject.GetComponentInParent<PlayerController>();
        flipRotation = transform.Find("AttackA").gameObject.GetComponent<ParticleSystemRenderer>();
        perentRotation = GetComponentInParent<Transform>();
    }
    private void FixedUpdate()
    {
        rot = perentRotation.rotation.y * -90;
        flipRotation.flip = new Vector3(rot, 0, 0);
    }
    public void Attacked()
    {
        //player.DealDamage();
    }
}
