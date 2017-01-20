using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

	void Start () {
        anim.Play("CameraZoomInStep1");
    }
	
	void Update () {
		
	}

    public void ZoomInStep1()
    {
        anim.Play("CameraZoomInStep1");
    }
    public void ZoomOutStep1()
    {
        anim.Play("CameraZoomOutStep1");
    }
    public void ZoomInStep2()
    {
        anim.Play("CameraZoomInStep2");
    }
    public void ZoomOutStep2()
    {
        anim.Play("CameraZoomOutStep2");
    }
}
