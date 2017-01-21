﻿using UnityEngine;
using System.Collections;

public class Controller : Character {
  public Rigidbody player;
  public GameObject AI;
  public float speed;
  public int NumSound = 6;
  int isMove;
  public AudioSource sound, heartsound;
  public AudioClip[] footstep;
  public GameObject splashstep;
  private bool isRunning = false;


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
    if (z != 0 || x != 0) {
      if (isMove == 0) {
        base.GenerateSound(false, 4f);
        player.velocity = new Vector3(x, 0, z).normalized * speed;
        splashstep.GetComponent<EllipsoidParticleEmitter>().maxSize = 0.3f;
      }
      if (isRunning) {
        splashstep.GetComponent<EllipsoidParticleEmitter>().maxSize = 0.3f;
        player.velocity = new Vector3(x, 0, z).normalized * speed * 2f;
      }
      isMove += 1;
    }
    else {
      isRunning = false;
      isMove = 0;
      splashstep.GetComponent<EllipsoidParticleEmitter>().maxSize = 0f;
      player.velocity = new Vector3(0, 0, 0);
    }
    if (isMove >= 5) {
      isRunning = true;
    }
    if (isMove == 10) {
      int randomFootstep = Random.Range(0, NumSound);
      sound.PlayOneShot(footstep[randomFootstep], 1);
      base.GenerateSound(false, 50f);
      isMove = 1;
    }
    Plane playerPlane = new Plane(Vector3.up, transform.position);
  	Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
  	float hitdist = 0.0f;
  	if (playerPlane.Raycast(ray, out hitdist)) {
      Vector3 targetPosition = ray.GetPoint(hitdist);
      transform.LookAt(targetPosition);
      transform.Rotate(90, 0, 0);
    	// transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
		}

      soundBeat();
    }

    void soundBeat()
    {
      float DisPlaAI = Vector3.Distance(player.transform.position, AI.transform.position);
      if (DisPlaAI > 20)
      {
        heartsound.pitch = 1;
      }
      else
      {
        heartsound.pitch = 3 - 2 * (DisPlaAI / 20);
      }
    }

  void OnTriggerStay(Collider other) {
    if (other.gameObject.tag.Equals("Item"))
    {
      float damage = 5;
      ItemController itemControllerScript = other.gameObject.GetComponent<ItemController>();
      if (Input.GetKeyDown("space"))
      {
        base.GenerateSound(true, 70f);
        itemControllerScript.Pull(5);
      }
    }
  }
}
