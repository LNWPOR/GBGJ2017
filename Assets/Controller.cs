using UnityEngine;
using System.Collections;

public class Controller : Character {
  public Rigidbody player;
  public float speed;
  int isMove;

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
    if (isMove == 10) {
      base.GenerateSound(false, 30f);
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
        base.GenerateSound(true, 30f);
        itemControllerScript.Pull(5);
      }
    }
  }
}
