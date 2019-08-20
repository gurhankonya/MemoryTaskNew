/* das wird nicht gebraucht: Es ist nur für VR relevant
 
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class LineOfSight : MonoBehaviour
{
    // from: https://www.studica.com/blog/how-to-create-a-raycast-in-unity-3d
    // used for detecting Raycast collision
    private RaycastHit vision;
    
    // used for assigning a length to the raycast
    public float rayLength;
    
    // used so we know whether or not we are holding an object
    private bool isGrabbed;
    
    // used to assign the object we are looking at to a variable we can use
    private Rigidbody grabbedObject;

    private void Start()
    {
        rayLength = 4.0f;
        isGrabbed = false;
    }

    private void Update()
    {
        // this will constantly draw the ray in our scene view so we can see where the ray is going
        Debug.DrawRay(Camera.main.transform.position,Camera.main.transform.forward * rayLength, Color.red, 0.5f);
        
        // this statement is called when the raycast is hitting a collider in the scene
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out vision, rayLength))
        {
            // determine if the object our raycast is hitting has the cube tag
            if (vision.collider.tag == "Cube")
            {
                // output the name of the object our raycast is hittint to our console
                Debug.Log(vision.collider.name);
                
                // if our user is looking at an interactive object allow them to press E and pick it up
                // but only if we are not holding something
                if (Input.GetKeyDown(KeyCode.E) && !isGrabbed)
                {
                    // assign grabbedObject to the object we are looking at
                    grabbedObject = vision.rigidbody;
                    // set the rigidbody to kinematik so it ignores physics
                    grabbedObject.isKinematic = true;
                    // set grabbedObject's parent to our camera
                    grabbedObject.transform.SetParent(gameObject.transform);
                    // we are now grabbing an object so set this to true
                    isGrabbed = true;
                }
                
                // if we have an object already and user presses E, drop object
                else if (isGrabbed && Input.GetKeyDown(KeyCode.E))
                {
                    // set grabbedObject's parent back to nothing
                    grabbedObject.transform.parent = null;
                    // allow grabbedObject to interact with physics
                    grabbedObject.isKinematic = false;
                    // we are no longer grabbing anything
                    isGrabbed = false;
                }
            }
        }
    }
}
*/
