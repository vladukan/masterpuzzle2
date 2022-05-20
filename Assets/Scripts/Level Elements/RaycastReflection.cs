using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class RaycastReflection : MonoBehaviour
{
    private int reflections;
    private float maxLength;
    private LineRenderer lineRenderer;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 direction;
    public GunsCollisions Gun;
    public bool laserActive = false;
    private Levels levels;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Start()
    {
        levels = FindObjectOfType<Levels>();
        reflections = levels.LazerReflections;
        maxLength = levels.LazerMaxLength;
    }
    private void Update()
    {
        if (Gun.player.active) LaserGenerate(); else LaserClear();
    }
    private void LaserGenerate()
    {
        lineRenderer.enabled = true;
        ray = new Ray(transform.position, transform.forward);
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
        float remainingLength = maxLength;
        for (int i = 0; i < reflections; i++)
        {
            if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLength))
            {
                //print(hit.collider.tag);
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                remainingLength -= Vector3.Distance(ray.origin, hit.point);
                ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                if (hit.collider.tag != "Mirror") break;
            }
            else
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);
            }
        }
    }
    private void LaserClear()
    {
        lineRenderer.enabled = false;
    }
}