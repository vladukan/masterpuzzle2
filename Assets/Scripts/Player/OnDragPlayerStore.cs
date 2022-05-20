using UnityEngine.EventSystems;
using UnityEngine;
public class OnDragPlayerStore : MonoBehaviour, IDragHandler
{
    public GameObject player;
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
        GetDragDirection(dragVectorDirection);
    }
    private void GetDragDirection(Vector3 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        if (positiveX > positiveY)
        {
            if (dragVector.x > 0)
            {
                //print("Right-");
                LeanTween.rotateY(player, player.transform.eulerAngles.y - 5f, 0);
            }
            else
            {
                LeanTween.rotateY(player, player.transform.eulerAngles.y + 5f,0);
                //print("left+");
            }
        }
    }
}
