using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour {
    public GameObject lookAt;
    private bool smooth = true;
    public float smoothSpeed = 0.125f;
    private Vector3 offset = new Vector3(0,0,-6.5f);
   
    void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, lookAt.transform.position, smoothSpeed);
        
        Vector3 desiredPosition = new Vector3(lookAt.transform.position.x, transform.position.y, lookAt.transform.position.z);
        if (smooth)
        {
            transform.position = Vector3.Lerp(transform.position,desiredPosition,smoothSpeed);
        }else
        {
            transform.position = desiredPosition;
        }
    }
}