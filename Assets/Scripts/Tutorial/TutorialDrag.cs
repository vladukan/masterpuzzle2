using UnityEngine;
using UnityEngine.EventSystems;
public class TutorialDrag : MonoBehaviour, IDragHandler
{
    private UiManager uiManager;
    private void Start()
    {
        uiManager = FindObjectOfType<UiManager>();
    }
    public void OnDrag(PointerEventData eventData)
    {

        uiManager.gameMenu.SetActive(false);
        uiManager.pauseGame.SetActive(true);
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach(PlayerController el in players){
            el.isDown = true;
            el.isUpdate = true;
        }
    }
}
