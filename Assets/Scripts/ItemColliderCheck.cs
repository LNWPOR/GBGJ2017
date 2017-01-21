using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemColliderCheck : MonoBehaviour {

    public GameObject camera;
    private CameraController cameraControllerScript;
    private List<GameObject> itemInRangeList;
    private GameObject currentNearestItem;
    private float currentNearestOffset = -1;
    private Vector3 newCamPos;
    void Awake()
    {
        cameraControllerScript = camera.GetComponent<CameraController>();
    }
    void Start()
    {
        itemInRangeList = new List<GameObject>();
    }
    void Update()
    {
        UpdateCurrentNearestItem();
        UpdateCamraZoom();
    }
    
    void UpdateCamraZoom()
    {
        if (!itemInRangeList.Count.Equals(0))
        {
            float percentOffset = currentNearestOffset / 6f;
            //Debug.Log(3+ percentOffset * (9 - 3f));
            newCamPos = new Vector3(camera.transform.position.x, cameraControllerScript.minPosY + percentOffset * (cameraControllerScript.normalPosY - cameraControllerScript.minPosY), camera.transform.position.z);
        }
        else
        {
            currentNearestOffset = -1;
            currentNearestItem = null;
            newCamPos = new Vector3(camera.transform.position.x, cameraControllerScript.normalPosY, camera.transform.position.z);

        }
        camera.transform.position = Vector3.Lerp(camera.transform.position, newCamPos, 0.05f);
    }
    
    void UpdateCurrentNearestItem()
    {
        if (!itemInRangeList.Count.Equals(0))
        {
            
            foreach (GameObject item in itemInRangeList)
            {
                float offset = Vector3.Distance(transform.position, item.transform.position);
                if (currentNearestOffset == -1)
                {
                    currentNearestOffset = offset;
                    currentNearestItem = item;
                }
                else if (offset < currentNearestOffset)
                {
                    currentNearestOffset = offset;
                    currentNearestItem = item;
                }
            }
           
        }

        //Debug.Log(currentNearestItem);

    }
    

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Item"))
        {
            //cameraControllerScript.ZoomInStep2();
            itemInRangeList.Add(other.gameObject);
            //Debug.Log("gg");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Item"))
        {
            //cameraControllerScript.ZoomOutStep2();
            int otherIndex = itemInRangeList.FindIndex(x => x.gameObject.name.Equals(other.gameObject.name));
            itemInRangeList.RemoveAt(otherIndex);
            //Debug.Log("ff");
        }
    }
}
