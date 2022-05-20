using UnityEngine;
public class RopesManager : MonoBehaviour
{
    //[SerializeField] Rigidbody beginElement;
    public FixedJoint endElement;
    void Start()
    {
        int count = transform.childCount;
        //transform.GetChild(0).GetComponent<FixedJoint>().connectedBody = beginElement;
        endElement.connectedBody = gameObject.transform.GetChild(count-1).GetComponent<Rigidbody>();
    }
}
