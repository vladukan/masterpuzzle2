using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    void Start()
    {
        LeanTween.moveX(gameObject, 0.8f, 2f).setLoopPingPong();
    }

}
