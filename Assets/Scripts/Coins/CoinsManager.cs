using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
public class CoinsManager : MonoBehaviour
{
    [Header("UI references")]
    [SerializeField] TMP_Text coinUIText;
    [SerializeField] TMP_Text coinUIStore;
    [SerializeField] GameObject animatedCoinPrefab;
    [SerializeField] Transform target;
    public AudioClip soundCoin;
    [Space]
    Queue<GameObject> coinsQueue = new Queue<GameObject>();
    [Space]
    [Header("Animation settings")]
    [SerializeField][Range(0.1f, 0.9f)] float minAnimDuration;
    [SerializeField][Range(0.9f, 2f)] float maxAnimDuration;
    [SerializeField] LeanTweenType easeType;
    [SerializeField] float spread;
    Vector3 targetPosition;
    public GameObject addBtnLevel;
    public GameObject addBtnTripleLvl;
    public GameObject BtnCasino;
    public GameObject BlockBtnLevel;
    public GameObject BlockBtnBonus;
    public delegate void ChangeCoins(float coins);
    public event ChangeCoins PlayerBalance;
    public float Coins;
    public int levelCoins;
    public TextMeshProUGUI BtnCoins;
    public TextMeshProUGUI BtnTripleCoins;
    public TextMeshProUGUI BtnCoinsBonus;
    private AudioSource cameraAudio;
    private void Start()
    {
        LoadCoinsPlayer();
        cameraAudio = Camera.main.transform.GetComponent<AudioSource>();
    }
    public void AddCoins(Vector3 collectedCoinPosition, int amount, bool visible = true, bool direction = true)
    {
        if (!direction && Coins - amount < 0) return;
        for (int i = 0; i < amount; i++)
        {
            if (i < 20)
            {
                GameObject coin = Lean.Pool.LeanPool.Spawn(animatedCoinPrefab);
                if (!visible) coin.GetComponent<MeshRenderer>().enabled = false;
                coin.transform.position = collectedCoinPosition + new Vector3(Random.Range(-spread, spread), 0f, 0f);
                //coin.transform.SetParent(transform);
                float duration = Random.Range(minAnimDuration, maxAnimDuration);
                LeanTween.move(coin, targetPosition, duration)
                .setEase(easeType)
                .setOnComplete(() =>
                {
                    cameraAudio.PlayOneShot(soundCoin);
                    if (direction) Coins++; else Coins--;
                    Lean.Pool.LeanGameObjectPool.Destroy(coin);
                    LeanTween.move(target.GetComponent<RectTransform>(), new Vector3(35, -60, 0), 0.1f).setEase(LeanTweenType.easeOutBounce);
                    LeanTween.move(target.GetComponent<RectTransform>(), new Vector3(0, -40, 0), 0.1f).setEase(LeanTweenType.easeOutBounce);
                    CheckCoins();
                });
                
            }
            else StartCoroutine(DelayCoins(direction, i));
        }
        StartCoroutine(SaveCoins());
    }
    IEnumerator SaveCoins()
    {
        yield return new WaitForSeconds(1f);
        PlayerPrefs.SetFloat("coinsPlayer", Coins);
        PlayerBalance?.Invoke(Coins);
    }
    IEnumerator DelayCoins(bool direction, int time)
    {
        yield return new WaitForSeconds(0.001f * time);
        if (direction) Coins++; else Coins--;
        CheckCoins();
    }
    public void LoadCoinsPlayer()
    {
        targetPosition = new Vector3(2.12f, 8.18f, 0);
        Coins = PlayerPrefs.GetFloat("coinsPlayer");
        //Coins = 2000;
        PlayerBalance?.Invoke(Coins);
        CheckCoins();

    }
    private void CheckCoins()
    {
        if (Coins >= 10000 && Coins < 1000000)
        {
            float num = Coins / 1000;
            coinUIText.text = num.ToString("F0") + "K";
            coinUIStore.text = num.ToString("F0") + "K";
            return;
        }

        if (Coins >= 1000000)
        {
            float num = Coins / 1000000;
            coinUIText.text = num.ToString("F0") + "M";
            coinUIStore.text = num.ToString("F0") + "M";
            return;
        }
        coinUIText.text = Coins.ToString();
        coinUIStore.text = Coins.ToString();
    }
    public void AddLevel(int amount)
    {
        AddCoins(addBtnLevel.transform.position, amount, false);
    }
    public void AddLvlDouble(int amount)
    {
        AddCoins(addBtnLevel.transform.position, amount, false);
    }
    public void AddLevelCoins(bool type)
    {
        if (type) levelCoins *= 3;
        AddCoins(addBtnLevel.transform.position, levelCoins, false);
    }
    public void SetBtnCoins()
    {
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        levelCoins += player.countBullets * 10;
        BtnCoins.text = levelCoins.ToString();
        BtnCoinsBonus.text = levelCoins.ToString();
        BtnTripleCoins.text = (levelCoins * 3).ToString();
    }
}

