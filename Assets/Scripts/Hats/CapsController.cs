using UnityEngine;
using Lean.Transition;
public class CapsController : MonoBehaviour
{
    public bool rotateCap = false;
    private PlayerCaps playerCaps;
    private SurpriceManager surprice;
    private UiManager ui;

    void Start()
    {
        if (rotateCap) LeanTween.rotateAround(gameObject, Vector3.up, 360, 2f).setLoopClamp();
        playerCaps = FindObjectOfType<PlayerCaps>();
        surprice = FindObjectOfType<SurpriceManager>();
        ui = FindObjectOfType<UiManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Bullet")
        {
            playerCaps.ClearCap();
            MoveCap();
        }
    }

    private void MoveCap()
    {
        surprice.bonusHat = true;
        transform.SetParent(playerCaps.player.Cap.transform);
        LeanTween.cancel(gameObject);
        LeanTween.move(gameObject, playerCaps.player.Cap.transform.position, 0.3f);
        transform.localScaleTransition(new Vector3(1, 1, 1), 0.3f);
        transform.localRotationTransition(new Quaternion(0, 0, 0, 0), 0.3f);
    }

    public void SetHat()
    {
        transform.localPositionTransition(new Vector3(0, 0, 0), 0);
        transform.localScaleTransition(new Vector3(1, 1, 1), 0f);
        transform.localRotationTransition(new Quaternion(0, 0, 0, 0), 0f);
    }


}
