using UnityEngine;

public class Disparo : MonoBehaviour
{
    public Camera cam;
    public float distance = 100f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance))
        {
            if (hit.collider.CompareTag("target"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
