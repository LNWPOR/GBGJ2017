using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveLife : MonoBehaviour {
  float life;
  private float lifespan = 300f;
  private string sourceTag;
  public bool isWaveSecondhand;

	void Start () {
    life = 0;
        Debug.Log(GetComponent<TrailRenderer>().materials[0].color.r);
	}

	// Update is called once per frame
	void Update () {
    life += 1;
    GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r, GetComponent<Renderer>().material.color.g, GetComponent<Renderer>().material.color.b, 1f - (life / lifespan));
    if (life >= lifespan) {
      Destroy(gameObject);
    }
	}

  public void SetLifespan(float newLifespan) {
    lifespan = newLifespan;
  }

  public void SetSourceTag(string tag) {
    sourceTag = tag;
  }

  public void SetIsSecondhand(bool isSecondhand) {
    isWaveSecondhand = isSecondhand;
  }

  void OnTriggerEnter(Collider other) {
    if (other.gameObject.tag.Equals(sourceTag)) return;
    if (other.gameObject.tag.Equals("Item")) {
      if (!isWaveSecondhand) {
        ItemController itemControllerScript = other.gameObject.GetComponent<ItemController>();
        itemControllerScript.GenerateSound(true, 200f);
      }
      Destroy(gameObject);
    } else if (other.gameObject.tag.Equals("Player")) {
      if (!isWaveSecondhand) {
        Controller playerControllerScript = other.gameObject.GetComponent<Controller>();
        playerControllerScript.GenerateSound(true, 200f);
      }
      Destroy(gameObject);
    } else if (other.gameObject.tag.Equals("AI")) {
      if (!isWaveSecondhand) {
        AI aiControllerScript = other.gameObject.GetComponent<AI>();
        aiControllerScript.GenerateSound(true, 200f);
      }
      Destroy(gameObject);
    }
  }
}
