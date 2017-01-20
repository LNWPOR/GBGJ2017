﻿using UnityEngine;
using System.Collections;

public class Controller : Character {
  public Rigidbody player;
  int isMove;

  void Start() {
    GetComponent<Renderer>().material.color = myColor;
    Debug.Log(myColor);
    isMove = 0;
  }


  void Update() {
    float z = 0, x = 0;
    if (Input.GetKey("w")) {
      z = 1;
    }
    if (Input.GetKey("s")) {
      z = -1;
    }
    if (Input.GetKey("a")) {
      x = -1;
    }
    if (Input.GetKey("d")) {
      x = 1;
    }
    if (z != 0 || x != 0) isMove += 1;
    else isMove = 0;
    if (isMove == 10) {
      base.GenerateSound();
    }
    if (isMove > 10) {
      isMove += 1;
      if (isMove == 20) isMove = 0;
      player.velocity = new Vector3(0, 0, 0);
    } else {
      player.velocity = new Vector3(x, 0, z).normalized * speed;
    }
  }

  void OnCollisionStay(Collision other) {
    if (other.gameObject.tag.Equals("Item"))
    {
      float damage = 5;
      ItemController itemControllerScript = other.gameObject.GetComponent<ItemController>();
      if (Input.GetKeyDown("space"))
      {
        itemControllerScript.Pull(5);
      }
    }
  }
}
