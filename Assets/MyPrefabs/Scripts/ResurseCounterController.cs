using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurseCounterController : MonoBehaviour  // контроллер предметов которые можно подобрать(поты, ключи..) / просто анимация и дестрой
{
    public ParticleSystem Shine;
    public ParticleSystem PickUpVFX;
    private SpriteRenderer sprite;



    private void Start()
    {
        //hpPotionPickUpVFX.Stop();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))

        {
            if (gameObject.CompareTag("hpBottle"))
            {
                other.gameObject.GetComponent<PlayerController>().HpPotionCounter++;
                Shine.Stop();
                PickUpVFX.Play();
                sprite.enabled = false;
            }

            if (gameObject.CompareTag("poisonBottle"))
            {
                other.gameObject.GetComponent<PlayerController>().PoisonPotionCounter++;
                Shine.Stop();
                PickUpVFX.Play();
                sprite.enabled = false;
            }

            if (gameObject.CompareTag("SmallKey"))
            {
                other.gameObject.GetComponent<PlayerController>().KeyCounter++;
                Shine.Stop();
                PickUpVFX.Play();
                sprite.enabled = false;
            }

            if (gameObject.CompareTag("coins"))
            {
                other.gameObject.GetComponent<PlayerController>().CoinCounter++;
                Shine.Stop();
                PickUpVFX.Play();
                sprite.enabled = false;
            }
            //Destroy(gameObject, 0.5f);

            if (gameObject.CompareTag("smokeBomb"))
            {
                other.gameObject.GetComponent<PlayerController>().SmokeBombCounter++;
                Shine.Stop();
                PickUpVFX.Play();
                sprite.enabled = false;
            }

            Destroy(gameObject, 0.5f);

        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (gameObject.CompareTag("hpBottle"))
            {
                other.GetComponent<EnemyController>().CurrentHealth += other.GetComponent<EnemyController>().BottleHeal;
                Shine.Stop();
                PickUpVFX.Play();
                sprite.enabled = false;
                Destroy(gameObject, 0.5f);
            }
        }
    }
}
