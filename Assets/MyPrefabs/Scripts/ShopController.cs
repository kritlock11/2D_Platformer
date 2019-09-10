using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour   //контроллер шопа
{
    public TextMeshProUGUI textBox;
    private PlayerController player;
    public bool inShopZone;
    public float itemPrice;
    public ParticleSystem PickUpVFX;
    public GameObject Item;
    void Start()
    {
        textBox.text = $"{itemPrice}";
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    private void Update()
    {
        ByeItem();
        TextColor();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) inShopZone = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) inShopZone = false;
    }

    void TextColor()
    {
        if (player.CoinCounter < itemPrice)
        {
            textBox.color = Color.red;
        }
        else
        {
            textBox.color = Color.yellow;
        }
    }

    bool GotMoney()
    {
        if (player.CoinCounter > 0) return true;
        return false;
    }
    void ByeItem()
    {
        if (Input.GetKeyDown(KeyCode.T) && GotMoney() && inShopZone)
        {
            if (player.CoinCounter >= itemPrice)
            {
                if (Item.CompareTag("hpBottle"))
                {
                    player.CoinCounter -= itemPrice;
                    player.HpPotionCounter++;
                    PickUpVFX.Play();
                }

                else if (Item.CompareTag("poisonBottle"))
                {
                    player.CoinCounter -= itemPrice;
                    player.PoisonPotionCounter++;
                    PickUpVFX.Play();
                }

                else if(Item.CompareTag("smokeBottle"))
                {
                    player.CoinCounter -= itemPrice;
                    player.SmokeBombCounter++;
                    PickUpVFX.Play();
                }
            }
        }
    }
}
