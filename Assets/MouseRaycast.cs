using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRaycast : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If Escape is clicked, free the mouse.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("The selected object is: " + hit.collider.name);
            // Check if the other object has a onClick manager.
            if (Input.GetMouseButtonDown(0))
            {
                ClickManager cm = hit.collider.gameObject.GetComponent<ClickManager>();
                if (cm != null)
                {
                    cm.functionToCall.Invoke();
                }
                if (hit.transform.name == "Player")
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }
    }
}
