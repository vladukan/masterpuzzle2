using UnityEngine;
using UnityEngine.EventSystems;
public class OnDragController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private StoreManager store;
    private void Start()
    {
        store = FindObjectOfType<StoreManager>();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (store.isAnimate) return;
        Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
        GetDragDirection(dragVectorDirection);
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    private void GetDragDirection(Vector3 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        int prev;
        if (positiveX > positiveY)
        {
            if (store.typeStore == "hats")
            {
                store.prevPageHats = store.activePageHats;
                if (dragVector.x > 0)
                {
                    //print("Right-");
                    if (store.activePageHats <= 1) return;
                    store.activePageHats--;
                    store.directionHats = 1200f;
                    prev = -1;
                }
                else
                {
                    if (store.activePageHats >= store.maxPageHats) return;
                    store.activePageHats++;
                    store.directionHats = -1200f;
                    prev = 1;
                    //print("left+");
                }
                store.SetActivePage(store.pageStoreHats, store.activePageHats);
            }
            else
            {
                store.prevPageGuns = store.activePageGuns;
                if (dragVector.x > 0)
                {
                    //print("Right-");
                    if (store.activePageGuns <= 1) return;
                    store.activePageGuns--;
                    store.directionGuns = 1200f;
                    prev = -1;
                }
                else
                {
                    if (store.activePageGuns >= store.maxPageGuns) return;
                    store.activePageGuns++;
                    store.directionGuns = -1200f;
                    prev = 1;
                    //print("left+");
                }
                store.SetActivePage(store.pageStoreGuns, store.activePageGuns);
            }
            store.isAnimate = true;
            store.FindStore(true, prev);
        }
        else
        {
            if (dragVector.y > 0) print("up"); else print("down");
        }
    }
}
