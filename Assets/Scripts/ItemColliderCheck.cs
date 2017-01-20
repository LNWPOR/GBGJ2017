using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemColliderCheck : MonoBehaviour {

    public GameObject camera;
    private CameraController cameraControllerScript; 

    void Awake()
    {
        cameraControllerScript = camera.GetComponent<CameraController>();
    }
	void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Item"))
        {
            cameraControllerScript.ZoomInStep2();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Item"))
        {
            cameraControllerScript.ZoomOutStep2();
        }
    }
}
