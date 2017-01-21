﻿using UnityEngine;
using System.Collections;

public class Controller : Character {
  public Rigidbody player;
  public float speed;
    public int NumSound = 6;
  int isMove;
    public AudioSource sound;
    public AudioClip[] footstep;

  public override void Start() {
    base.Start();
    isMove = 0;
    tag = "Player";
  }


  public override void Update() {
    base.Update();
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
    if (isMove == 20) {
            int randomFootstep = Random.Range(0, NumSound);
            sound.PlayOneShot(footstep[randomFootstep], 1);
      base.GenerateSound(false, 300f);
    }
    if (isMove > 20) {
      isMove += 1;
      if (isMove == 30) isMove = 0;
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
        base.GenerateSound(true, 300f);
        itemControllerScript.Pull(5);
      }
    }
  }
}
