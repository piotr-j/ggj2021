using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraMovementSpeed = 3;
    
    public float dragSpeed = 3f;
    public float velocity = 0;

    private Transform cameraTarget;


    public void Update()
    {
        if (!cameraTarget) cameraTarget = FindObjectOfType<TeamScript>()?.transform;

        DragUpdate();
    }
    
    private Vector3 moveDragOrigin;
    private Vector3 cameraDragOrigin;
    
    private Vector3 rotationDragOrigin;
    private Vector3 rotationStart;

    private float side = 1;

    private bool startingRotation = true;

    void DragUpdate()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            startingRotation = false;
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            rotationDragOrigin = Input.mousePosition;
            rotationStart = transform.rotation.eulerAngles;

        } 
        else if (Input.GetMouseButton(1))
        {
            velocity = (Input.mousePosition - rotationDragOrigin).x;
            transform.rotation = Quaternion.Euler(rotationStart + new Vector3(0 ,velocity * dragSpeed, 0));
            side = Mathf.Sign((Input.mousePosition - rotationDragOrigin).x);
        }

        /*if (Input.GetMouseButtonDown(0))
        {
            moveDragOrigin = Input.mousePosition;
            cameraDragOrigin = transform.position;
        } 
        else if (Input.GetMouseButton(0))
        {
            Vector3 pos = Input.mousePosition - moveDragOrigin;
            Vector3 cameraDiff = cameraDragOrigin - transform.position;
            Vector3 move = new Vector3(pos.x * dragSpeed * 0.02f, 0, pos.y * dragSpeed  * 0.02f);

            transform.Translate(move, Space.Self);  
            
            // transform.position = new Vector3(cameraDragOrigin.x + pos.x * dragSpeed / 2f, cameraDragOrigin.y, cameraDragOrigin.z + pos.y * dragSpeed / 2f);
        }*/

        if (cameraTarget != null)
        {
            transform.position = Vector3.Lerp(transform.position, cameraTarget.position, cameraMovementSpeed * Time.deltaTime);
        }
        
 
        // Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        // Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);
 
        // transform.Translate(move, Space.World);  
        // transform.Translate(new Vector3((Input.mousePosition - dragOrigin).x, 0, 0) * 0.3f * Time.deltaTime);
        // transform.rotation = Quaternion.Euler(rotationStart + new Vector3(0 ,(Input.mousePosition - dragOrigin).x * dragSpeed, 0));
        if (startingRotation)
        {
            transform.Rotate(0, 15 * Time.deltaTime, 0);
        }
    }
}
