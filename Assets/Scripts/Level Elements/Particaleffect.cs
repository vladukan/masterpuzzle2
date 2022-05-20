using UnityEngine;
using System.Collections;
public class Particaleffect : MonoBehaviour
{
    public static Particaleffect instance;
    void Awake()
    {
        if (instance != null)
        {
            Lean.Pool.LeanGameObjectPool.Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public IEnumerator playpop()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<ParticleSystem>().Play();
    }
}
