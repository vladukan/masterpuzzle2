using UnityEngine;
using TMPro;
public class FPScontroller : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float updateInterval = 0.5F;
    private double lastInterval;
    private int frames;
    private float fps;
    void Start()
    {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
    }
    void OnGUI()
    {
        //GUILayout.Label("" + fps.ToString("f2"));
        text.text = "FPS: "+fps.ToString("f0");
    }
    void Update()
    {
        ++frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + updateInterval)
        {
            fps = (float)(frames / (timeNow - lastInterval));
            frames = 0;
            lastInterval = timeNow;
        }
    }
}
