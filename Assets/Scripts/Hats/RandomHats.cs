using UnityEngine;
using System.Collections.Generic;
public class RandomHats : MonoBehaviour
{
    public GameObject hat;
    private SurpriceManager surprice;
    void Start()
    {
        surprice = FindObjectOfType<SurpriceManager>();
        LoadLevelHat();
    }
    private void LoadLevelHat()
    {
        if (surprice.rewardHats.Count == 0) return;
        int id = Random.Range(0, surprice.rewardHats.Count);
        surprice.hatLevelID = surprice.rewardHats[id];
        GameObject prefab = Resources.Load<GameObject>($"Caps/Cap_{surprice.rewardHats[id]}") as GameObject;
        if (prefab != null)
        {
            hat = Lean.Pool.LeanPool.Spawn(prefab, transform.position, Quaternion.identity);
            hat.transform.SetParent(transform);
            if (hat.transform.GetComponent<CapsController>()) hat.transform.GetComponent<CapsController>().rotateCap = true;
            surprice.hatLevel = hat;
        }
    }
}
