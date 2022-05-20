using UnityEngine;
public class CameraPostRender : MonoBehaviour
{
    private Levels levels;
    private bool cancel = false;
    private UiManager ui;
    private void Start()
    {
        levels = FindObjectOfType<Levels>();
        ui = FindObjectOfType<UiManager>();
    }
    private void OnPostRender()
    {
        if (cancel) return;
        if (ui.activeBonusLevel) levels.LoadLevel(ui.levelBonus); else levels.LoadLevel(ui.level);
        ui.pauseGame.SetActive(false);
        cancel = true;
    }
}
