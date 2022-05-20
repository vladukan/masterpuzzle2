using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ScrollElement : MonoBehaviour
{
    public int page = 1;
    public int number = 1;
    public int IdCap = 1;
    private GameObject btnBuy;
    private GameObject btnOff;
    private GameObject btnEquip;
    private GameObject btnBlock;
    public int price;
    public bool reward;
    public bool bonus;
    public bool empty;
    public bool emptyGun;
    private StoreManager store;
    private CoinsManager coins;
    private SurpriceManager surprice;
    private string typeElement;
    private void Start()
    {
        coins = FindObjectOfType<CoinsManager>();
        store = FindObjectOfType<StoreManager>();
        surprice = FindObjectOfType<SurpriceManager>();
        if (store.typeStore == "hats") typeElement = "Cap_"; else typeElement = "Gun_";
        if (empty)
        {
            AddEquip();
            store.typeStore = "hats";
            if (IdCap == PlayerPrefs.GetInt("capPlayer")) ActiveCAp();
            return;
        }
        if (emptyGun)
        {
            AddEquip();
            store.typeStore = "guns";
            if (IdCap == PlayerPrefs.GetInt("gunPlayer")) ActiveCAp();
            return;
        }
        if (!PlayerPrefs.HasKey(typeElement + IdCap)) PlayerPrefs.SetInt(typeElement + IdCap, 0);
        SetBonusBtn();
        SetBtnStore();
    }
    private void OnChangeBalance(float coins)
    {
        if (btnEquip != null) return;
        if (bonus || reward) return;
        if (coins >= price)
        {
            if (btnBuy != null) return;
            ClearBtn();
            btnBuy = Lean.Pool.LeanPool.Spawn(store.btnBuy, transform);
            btnBuy.transform.SetParent(transform);
            btnBuy.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = price.ToString();
            btnBuy.GetComponent<Button>().onClick.AddListener(ClickElement);
        }
        else
        {
            if (btnOff != null) return;
            ClearBtn();
            btnOff = Lean.Pool.LeanPool.Spawn(store.btnOff, transform);
            btnOff.transform.SetParent(transform);
            btnOff.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = price.ToString();
        }
    }
    public void ClickElement()
    {
        if (btnOff != null) return;
        if (btnBuy != null)
        {
            coins.AddCoins(new Vector3(0, 0, 0), price, false, false);
            Lean.Pool.LeanGameObjectPool.Destroy(btnBuy);
            btnEquip = Lean.Pool.LeanPool.Spawn(store.btnEquip, transform);
            btnEquip.transform.SetParent(transform);
            btnEquip.GetComponent<Button>().onClick.AddListener(ClickElement);
            PlayerPrefs.SetInt(typeElement + IdCap, 1);
            return;
        };
        if (btnEquip != null) ActiveCAp();
    }
    public void ActiveCAp()
    {
        if (store.typeStore == "hats")
        {
            PlayerPrefs.SetInt("capPlayer", IdCap);
            PlayerCaps[] players;
            players = FindObjectsOfType<PlayerCaps>();
            foreach (PlayerCaps el in players)
            {
                el.cap = IdCap;
                foreach (Transform child in el.player.Cap.transform)
                    Lean.Pool.LeanGameObjectPool.Destroy(child.gameObject);
                el.ClearCap();
                el.SetCap();
            }
            store.btnActiveHat.transform.SetParent(transform);
            store.btnActiveHat.transform.SetAsFirstSibling();
            store.btnActiveHat.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            PlayerPrefs.SetInt("gunPlayer", IdCap);
            PlayerGuns[] players;
            players = FindObjectsOfType<PlayerGuns>();
            foreach (PlayerGuns el in players)
            {
                el.gun = IdCap;
                el.ClearGun();
                el.SetGun();
            }
            store.btnActiveGun.transform.SetParent(transform);
            store.btnActiveGun.transform.SetAsFirstSibling();
            store.btnActiveGun.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        }
    }
    public void ClearBtn()
    {
        foreach (Transform child in gameObject.transform) Lean.Pool.LeanGameObjectPool.Destroy(child.gameObject);
    }
    private void SetBtnStore()
    {
        if (PlayerPrefs.GetInt(typeElement + IdCap) == 1)
        {
            AddEquip();
            if (IdCap == PlayerPrefs.GetInt("capPlayer") && transform.tag == "StoreCaps")
            {
                store.btnActiveHat.transform.SetParent(transform);
                store.btnActiveHat.transform.SetAsFirstSibling();
                store.btnActiveHat.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            }
            if (IdCap == PlayerPrefs.GetInt("gunPlayer") && transform.tag == "StoreGuns")
            {
                store.btnActiveGun.transform.SetParent(transform);
                store.btnActiveGun.transform.SetAsFirstSibling();
                store.btnActiveGun.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            }
        }
        else
        {
            coins.PlayerBalance += OnChangeBalance;
            OnChangeBalance(coins.Coins);
        }
    }
    public void AddEquip()
    {
        btnEquip = Lean.Pool.LeanPool.Spawn(store.btnEquip, transform);
        btnEquip.transform.SetParent(transform);
        btnEquip.GetComponent<Button>().onClick.AddListener(ClickElement);
    }
    private void SetBonusBtn()
    {
        if (bonus && PlayerPrefs.GetInt(typeElement + IdCap) == 0)
        {
            AddBlock();
            surprice.bonusHats.Add(IdCap);
        }
        if (reward && PlayerPrefs.GetInt(typeElement + IdCap) == 0)
        {
            AddBlock();
            surprice.rewardHats.Add(IdCap);
        }
    }
    private void AddBlock()
    {
        btnBlock = Lean.Pool.LeanPool.Spawn(store.btnBlock, transform);
        btnBlock.transform.SetParent(transform);
    }
}
