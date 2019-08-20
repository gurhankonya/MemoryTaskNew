/* Das wird nicht gebraucht, es ist nur für VR relevant
 using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Raycasting : MonoBehaviour
{
    public float rayLength;
    public LayerMask layermask;

    private Ray ray;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, rayLength, layermask))
            {
                //Debug.DrawRay(transform.position, transform.forward, Color.red);
                Debug.Log(hit.collider.name);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;
        Gizmos.DrawRay(transform.position, direction);
        // Oben erstellten ray in variable speichern und hier variable.direction*100
        Debug.DrawRay(ray.direction * 100);
    }
}
*/