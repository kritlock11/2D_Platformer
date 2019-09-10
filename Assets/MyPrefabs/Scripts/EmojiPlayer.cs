using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiPlayer : MonoBehaviour
{
    public float lifeTime;
    float startLifeTime;
    public Transform position;
    //public Animator anim;
    //public Animation animator;
    private void Start()
    {
        //anim = GetComponent<Animator>();
        startLifeTime = lifeTime;
        position = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {

        //anim.Play("emojiLove");

        transform.position = position.gameObject.GetComponent<PlayerController>().EmojiPos.transform.position;
        startLifeTime -= Time.deltaTime;
        if (startLifeTime < 0)
        {
            Destroy(gameObject);
        }
    }
}
