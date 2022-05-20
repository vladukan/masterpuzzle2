using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class StoreManager : MonoBehaviour
{
    public int activePageHats = 1;
    public int activePageGuns = 1;
    public int maxPageHats = 5;
    public int maxPageGuns = 2;
    public int prevPageHats = 1;
    public int prevPageGuns = 1;
    public GameObject pageStoreHats;
    public GameObject pageStoreGuns;
    public GameObject rootContainer;
    public string typeStore = "hats";
    public GameObject btnActiveHat;
    public GameObject btnActiveGun;
    public GameObject btnBuy;
    public GameObject btnEquip;
    public GameObject btnBlock;
    public GameObject btnOff;
    public float directionHats = 1200f;
    public float directionGuns = 1200f;
    public GameObject[] activeElements;
    public GameObject[] hats;
    public GameObject[] guns;
    private Color onColor = Color.white;
    private Color offColor = new Color(0.197f, 0f, 0.5f, 1f);
    public bool isAnimate = false;
    public PlayerCaps player;
    public GameObject ContainerHats;
    public GameObject ContainerGuns;
    void Start()
    {
        hats = GameObject.FindGameObjectsWithTag("StoreCaps");
        guns = GameObject.FindGameObjectsWithTag("StoreGuns");
        if (typeStore == "hats") SetActivePage(pageStoreHats, activePageHats);
        else SetActivePage(pageStoreGuns, activePageGuns);
        //FindStore();
        ContainerElements(false);
    }
    public void SetActivePage(GameObject pages, int activePage)
    {
        if (pages.transform.childCount == 0) return;
        for (int i = 0; i < pages.transform.childCount; i++)
            pages.transform.GetChild(i).GetComponent<Image>().color = offColor;
        pages.transform.GetChild(activePage - 1).GetComponent<Image>().color = onColor;
    }
    public void FindStore(bool animate = false, int prev = 1)
    {
        GameObject[] arr;
        int activePage;
        float direction;
        if (typeStore == "hats")
        {
            arr = hats;
            pageStoreHats.SetActive(true);
            pageStoreGuns.SetActive(false);
            activePage = activePageHats;
            ClearElements("hats");
            direction = directionHats;
        }
        else
        {
            arr = guns;
            pageStoreHats.SetActive(false);
            pageStoreGuns.SetActive(true);
            activePage = activePageGuns;
            ClearElements("guns");
            direction = directionGuns;
        }
        foreach (GameObject el in arr)
        {
            if (el.GetComponent<ScrollElement>() && el.GetComponent<ScrollElement>().page == activePage)
                AnimateElement(el, el.GetComponent<ScrollElement>().number, animate, direction);
            if (animate && el.GetComponent<ScrollElement>() && el.GetComponent<ScrollElement>().page == activePage - prev)
                AnimatePrevElement(el, el.GetComponent<ScrollElement>().number, animate, direction);
        }
        StartCoroutine(StopAnimate());
    }
    private void AnimateElement(GameObject el, int value, bool animate = true, float direction = 1200f)
    {
        float time = 0.5f;
        //if (el.transform.parent != rootContainer.transform) el.transform.SetParent(rootContainer.transform);
        if (!animate)
        {
            el.GetComponent<RectTransform>().localPosition = activeElements[value - 1].GetComponent<RectTransform>().localPosition;
        }
        else
        {
            el.GetComponent<RectTransform>().localPosition = activeElements[value - 1].GetComponent<RectTransform>().localPosition;
            el.GetComponent<RectTransform>().localPosition = new Vector3(
                el.GetComponent<RectTransform>().localPosition.x - direction,
                el.GetComponent<RectTransform>().localPosition.y,
                el.GetComponent<RectTransform>().localPosition.z);
            LeanTween.move(el.GetComponent<RectTransform>(),
            activeElements[value - 1].GetComponent<RectTransform>().localPosition, time).setEaseLinear();
        }
    }
    private void AnimatePrevElement(GameObject el, int value, bool animate = true, float direction = 1200f)
    {
        Vector3 pos = new Vector3(
        el.GetComponent<RectTransform>().localPosition.x - direction * -1,
        el.GetComponent<RectTransform>().localPosition.y,
        el.GetComponent<RectTransform>().localPosition.z);
        float time = 0.5f;
        LeanTween.move(el.GetComponent<RectTransform>(), pos, time).setEaseLinear();
    }
    IEnumerator StopAnimate()
    {
        yield return new WaitForSeconds(0.5f);
        isAnimate = false;
    }
    public void OpenStore(string type)
    {
        // foreach (Transform child in player.player.Cap.transform) 
        // Lean.Pool.LeanGameObjectPool.Destroy(child.gameObject);
        typeStore = type;
        FindStore();
    }
    private void ClearElements(string name)
    {
        if (name == "hats")
        {
            ContainerGuns.GetComponent<CanvasGroup>().alpha = 0;
            ContainerHats.GetComponent<CanvasGroup>().alpha = 1;
            ContainerGuns.GetComponent<CanvasGroup>().blocksRaycasts = false;
            ContainerHats.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            ContainerGuns.GetComponent<CanvasGroup>().alpha = 1;
            ContainerHats.GetComponent<CanvasGroup>().alpha = 0;
            ContainerGuns.GetComponent<CanvasGroup>().blocksRaycasts = true;
            ContainerHats.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }
    public void ContainerElements(bool visible = false)
    {
        if (visible)
        {
            ContainerGuns.GetComponent<CanvasGroup>().alpha = 1;
            ContainerHats.GetComponent<CanvasGroup>().alpha = 1;
            ContainerGuns.GetComponent<CanvasGroup>().blocksRaycasts = true;
            ContainerHats.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            ContainerGuns.GetComponent<CanvasGroup>().alpha = 0;
            ContainerHats.GetComponent<CanvasGroup>().alpha = 0;
            ContainerGuns.GetComponent<CanvasGroup>().blocksRaycasts = false;
            ContainerHats.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

    }
}
