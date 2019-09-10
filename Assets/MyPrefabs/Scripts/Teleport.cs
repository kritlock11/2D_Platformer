using TMPro;
using UnityEngine;

public class Teleport : MonoBehaviour  //дверь сначала открываем ключем потом переносимся в другую локу
{
    public Transform teleportTo;
    public TextMeshProUGUI dialogBox;
    private PlayerController telo;
    public ParticleSystem locked;
    public bool inDoorZone;
    public bool unlocked;
    public string[] textToDisplay;
    public SpriteRenderer doorRenderer;
    public Sprite[] doorOpenCloseSprites = new Sprite[2];

    void Start()
        
    {
        doorRenderer = GetComponent<SpriteRenderer>();
        doorRenderer.sprite = doorOpenCloseSprites[0];
        dialogBox.text = "";
        telo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //locked.Play();
    }
    private void Update()
    {
        OpenDoor();
        TeleportTo();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inDoorZone = true;
            if (other != null && !unlocked) dialogBox.text = textToDisplay[1];
            else dialogBox.text = textToDisplay[0];
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other != null)
            {
                dialogBox.text = "";
                inDoorZone = false;
            }
        }
    }
    bool GotKey()
    {
        if (telo.KeyCounter > 0) return true;
        return false;
    }
    void OpenDoor()
    {
        if (Input.GetKeyUp(KeyCode.T) && GotKey() && inDoorZone && !unlocked)
        {
            unlocked = true;
            telo.KeyCounter--;
            doorRenderer.sprite = doorOpenCloseSprites[1];
            dialogBox.text = textToDisplay[0];
            locked.Stop();
        }

        else if (Input.GetKeyDown(KeyCode.T) && !GotKey() && inDoorZone && !unlocked)
            dialogBox.text = textToDisplay[2];
    }
    void TeleportTo()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (unlocked && inDoorZone)
            {
                telo.gameObject.transform.position = teleportTo.position;
            }
        }
    }
}
