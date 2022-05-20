using UnityEngine;
public class Follow : MonoBehaviour
{
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 input = cam.ScreenToWorldPoint(Input.mousePosition);
        // float AngleRad = Mathf.Atan2(input.y - transform.position.y, input.x - transform.position.x);
        // float AngleDeg = (180 / Mathf.PI) *AngleRad;
        // this.transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
    }
}
