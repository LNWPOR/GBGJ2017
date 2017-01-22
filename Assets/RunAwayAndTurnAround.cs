﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAwayAndTurnAround : MonoBehaviour {
    float spinSpeed = 10f, itemSpeed = 2.4f;
    public Rigidbody item;
	// Use this for initialization
	void Start () {
        float x, z;
        x = Random.Range(-1f, 1f);
        z = Random.Range(-1f, 1f);
        item.velocity = new Vector3(x, 0, z).normalized * itemSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        
        transform.RotateAround(transform.position, new Vector3(0, 1, 0), -spinSpeed);

    }
}
