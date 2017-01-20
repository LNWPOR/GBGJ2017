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
        anim.Play("CameraZoomOutStep2");
    }
	
	void Update () {
		
	}
}
