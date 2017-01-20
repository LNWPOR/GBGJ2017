using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveLife : MonoBehaviour {
  float life;
  public float lifespan = 60f;


	void Start () {
    life = 0;
	}

	// Update is called once per frame
	void Update () {
    life += 1;
    GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r, GetComponent<Renderer>().material.color.g, GetComponent<Renderer>().material.color.b, 1f-(life/100));
    if (life >= lifespan) {
      Destroy(gameObject);
    }
	}

  void OnTriggerEnter(Collider other) {
    if (other.gameObject.tag.Equals("Item")) {
      ItemController itemControllerScript = other.gameObject.GetComponent<ItemController>();
      itemControllerScript.GenerateSound();
      Destroy(gameObject);
    } else if (other.gameObject.tag.Equals("Player")) {
      Controller playerControllerScript = other.gameObject.GetComponent<Controller>();
      playerControllerScript.GenerateSound();
      Destroy(gameObject);
    } else if (other.gameObject.tag.Equals("AI")) {
      AI aiControllerScript = other.gameObject.GetComponent<AI>();
      aiControllerScript.GenerateSound();
      Destroy(gameObject);
    }
  }
}
