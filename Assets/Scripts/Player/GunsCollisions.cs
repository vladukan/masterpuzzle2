using UnityEngine;
public class GunsCollisions : MonoBehaviour
{
    public PlayerController player;
    private Levels levels;
    private void Start()
    {
        levels = FindObjectOfType<Levels>();
        if (GetComponent<BoxCollider>()) GetComponent<BoxCollider>().enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Mirror")
        {
            //print(player.transform.tag + " ENTER " + other.tag);
            player.isCollision = true;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Mirror")
        {
            //print(player.transform.tag + " EXIT " + other.tag);
            player.isCollision = false;
        }
    }
}
