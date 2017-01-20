using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveLife : MonoBehaviour {
  float life;

	void Start () {
    life = 0;
	}

	// Update is called once per frame
	void Update () {
    life += 1;
    GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r, GetComponent<Renderer>().material.color.g, GetComponent<Renderer>().material.color.b, 1f-(life/100));
    if (life == 100)
    {
      Destroy(gameObject);
    }
	}

  void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag.Equals("Item"))
    {
      ItemController itemControllerScript = other.gameObject.GetComponent<ItemController>();
      itemControllerScript.GenerateSound();
    }
  }
}
