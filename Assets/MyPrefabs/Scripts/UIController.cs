using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour  // отображение на экране количество собранных ключей/ банок/ денег
{
    private PlayerController playerCounters;

    public TextMeshProUGUI keyTextDisplay;
    public TextMeshProUGUI hpTextDisplay;
    public TextMeshProUGUI poisonTextDisplay;
    public TextMeshProUGUI coinTextDisplay;
    public TextMeshProUGUI smokeTextDisplay;

    private Image _keyImg;
    private Image _hpPotionImg;
    private Image _poisonPotionImg;
    private Image _coinImg;
    private Image _smokeImg;

    public Sprite fromKeyImg;
    public Sprite toKeyImg;
    public Sprite fromHpPotionImg;
    public Sprite toHpPotionImg;
    public Sprite fromPoisonPotionImg;
    public Sprite toPoisonPotionImg;
    public Sprite fromCoinImg;
    public Sprite toCoinImg;
    public Sprite fromSmokeImg;
    public Sprite toSmokeImg;

    public ParticleSystem Coin;
    public ParticleSystem Smoke;
    public ParticleSystem Poison;
    public ParticleSystem Hp;
    public ParticleSystem Key;


    private void Start()
    {
        playerCounters = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        _keyImg = GameObject.FindGameObjectWithTag("UIKeyImage").GetComponent<Image>();
        _hpPotionImg = GameObject.FindGameObjectWithTag("hpBottleImg").GetComponent<Image>();
        _poisonPotionImg = GameObject.FindGameObjectWithTag("poisonBottleImg").GetComponent<Image>();
        _coinImg = GameObject.FindGameObjectWithTag("coinImg").GetComponent<Image>();
        _smokeImg = GameObject.FindGameObjectWithTag("smokeBottleImg").GetComponent<Image>();
    }

    private void Update()
    {
        keyTextDisplay.text = $"{playerCounters.KeyCounter}";
        if (playerCounters.KeyCounter == 0)
        {
            _keyImg.sprite = fromKeyImg;
        }
        else
        {
            _keyImg.sprite = toKeyImg;
            //Instantiate(Hp, new Vector2(0.124f, -0.101f), Quaternion.identity);
        }

        hpTextDisplay.text = $"{playerCounters.HpPotionCounter}";
        if (playerCounters.HpPotionCounter == 0)
        {
            _hpPotionImg.sprite = fromHpPotionImg;
        }
        else
        {
            _hpPotionImg.sprite = toHpPotionImg;
            //Instantiate(Hp, new Vector2(0.124f, -0.101f), Quaternion.identity);
        }

        poisonTextDisplay.text = $"{playerCounters.PoisonPotionCounter}";
        if (playerCounters.PoisonPotionCounter == 0)
        {
            _poisonPotionImg.sprite = fromPoisonPotionImg;
        }
        else
        {
            _poisonPotionImg.sprite = toPoisonPotionImg;
            //Instantiate(Poison, new Vector2(0.671f, -0.101f), Quaternion.identity);
        }

        coinTextDisplay.text = $"{playerCounters.CoinCounter}";
        if (playerCounters.CoinCounter == 0)
        {
            _coinImg.sprite = fromCoinImg;
        }
        else
        {
            _coinImg.sprite = toCoinImg;
            //Instantiate(Coin, new Vector2(1.84f, -0.101f), Quaternion.identity);
        }

        smokeTextDisplay.text = $"{playerCounters.SmokeBombCounter}";
        if (playerCounters.SmokeBombCounter == 0)
        {
            _smokeImg.sprite = fromSmokeImg;
        }
        else
        {
            _smokeImg.sprite = toSmokeImg;
            //Instantiate(Smoke, new Vector2(1.227f, -0.101f), Quaternion.identity);
        }
    }
}
