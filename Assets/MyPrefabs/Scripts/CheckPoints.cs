using UnityEngine;

public class CheckPoints : MonoBehaviour // чекпоинты
{
    public GameObject Pos;
    public Sprite[] Sprites = new Sprite[2]; //замена спрайтов при чеке
    public ParticleSystem Shine;
    private GameMaster _gm;
    private SpriteRenderer _sprite;
    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _sprite.sprite = Sprites[0];
        _gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>(); 
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _gm.LastCheckPointPos = Pos.transform.position;
            _sprite.sprite = Sprites[1];
            Shine.Play();
        }
    }
}
 