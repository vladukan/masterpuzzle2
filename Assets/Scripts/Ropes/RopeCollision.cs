using UnityEngine;
public class RopeCollision : MonoBehaviour
{
    [SerializeField] AudioClip sound;
    [SerializeField] int id;
    [SerializeField] GameObject rootRope;
    private AudioSource cameraAudio;
    private void Start()
    {
        cameraAudio = Camera.main.transform.GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Bullet")
        {
            cameraAudio.PlayOneShot(sound);
            ClearRope();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Bullet")
        {
            cameraAudio.PlayOneShot(sound);
            ClearRope();
        }
    }
    private void ClearRope()
    {
        for (int i = id; i < rootRope.transform.childCount; i++)
        {
            if (rootRope.transform.GetChild(i))
                Lean.Pool.LeanGameObjectPool.Destroy(rootRope.transform.GetChild(i).gameObject);
        }
        if(rootRope.GetComponent<RopesManager>().endElement.GetComponent<FixedJoint>())
        Destroy(rootRope.GetComponent<RopesManager>().endElement.GetComponent<FixedJoint>());
        
    }
}
