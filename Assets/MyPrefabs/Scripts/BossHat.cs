using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BossHat : MonoBehaviour
{
    private bool _hatStorm;
    private bool _cyrsedEyes;
    public ParticleSystem Storm;
    public GameObject CyrsedEyes;
    private PlayerController playerController;
    private float faceTimer;
    private float eyeTimer;

    public ParticleSystem Shine;
    public ParticleSystem PickUpVFX;
    private ParticleSystem _storm;
    private SpriteRenderer sprite;

    public GameObject Canvas;
    public TextMeshProUGUI text;
    private GameObject HpBar;
    private GameObject UI;


    private void Start()
    {
        faceTimer = 6;
        eyeTimer = 1;
        HpBar = GameObject.FindGameObjectWithTag("playerHpSlider");
        UI = GameObject.FindGameObjectWithTag("UIcanvasUnderHpBar");
        sprite = GetComponent<SpriteRenderer>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (faceTimer >= 0 && _hatStorm)
        {
            faceTimer -= Time.deltaTime;

        }
        if (_hatStorm)
        {
            playerController._rb.velocity = new Vector2(playerController._rb.velocity.x, 1).normalized * 0.2f;
        }
        if (faceTimer <= 0)
        {
            _hatStorm = false;
            playerController.SpritePlayerFace.sprite = playerController.Sprites[4];
            playerController.SpritePlayerEyes.enabled = false;
            eyeTimer -= Time.deltaTime;
        }
        if (eyeTimer <= 0)
        {
            if (!_cyrsedEyes)
            {
                Instantiate(CyrsedEyes, (Vector2)playerController.transform.position, Quaternion.identity); //+Vector2.up * 1.5f
                playerController._rb.velocity = new Vector2(0, 0);
                Canvas.SetActive(true);
                text.rectTransform.position = (Vector2)playerController.transform.position + Vector2.down * 0.5f;
                HpBar.SetActive(false);
                UI.SetActive(false);
                _cyrsedEyes = true;
                Time.timeScale = 0F;    
            }
        }
        if (_storm != null)
        {

            _storm.transform.position = playerController.transform.position;
        }
        Debug.Log($"{_hatStorm}, ----{faceTimer}");

        //Destroy(gameObject, 12);

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Shine.Stop();
            PickUpVFX.Play();
            sprite.enabled = false;

            other.gameObject.GetComponent<PlayerController>().SpritePlayerHat.sprite = other.gameObject.GetComponent<PlayerController>().Sprites[3];
            other.gameObject.GetComponent<PlayerController>().SpritePlayerHat.enabled = true;
            other.gameObject.GetComponent<PlayerController>().Speed = 0;
            other.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            other.gameObject.GetComponent<PlayerController>()._shake.CamScreenBossShake();
            playerController.OnGround = false;
            HatStorm();
            //transform.position = new Vector2(0, 0);
        }
    }

    void HatStorm()
    {
        if (!_hatStorm)
        {
            _storm = Instantiate(Storm, playerController.transform.position, Quaternion.identity);

            _hatStorm = true;
        }
    }

}
