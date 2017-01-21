using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveLife : MonoBehaviour {
  float life;
  private float lifespan = 300f;
  private string sourceTag;
  public bool isWaveSecondhand;
  private Vector3 sourcePosition;

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

  public void SetSourcePosition(Vector3 pos) {
    sourcePosition = pos;
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
    if (!other.gameObject.tag.Equals("Untagged")) Debug.Log(other.gameObject.tag);
    if (other.gameObject.tag.Equals("Item")) {
      ItemController itemControllerScript = other.gameObject.GetComponent<ItemController>();
      if (!isWaveSecondhand && itemControllerScript.IsResonanceable()) {
        itemControllerScript.GenerateSound(true, 20f);
      }
      Destroy(gameObject);
    } else if (other.gameObject.tag.Equals("Player")) {
      Controller playerControllerScript = other.gameObject.GetComponent<Controller>();
      if (!isWaveSecondhand && playerControllerScript.IsResonanceable()) {
        playerControllerScript.GenerateSound(true, 20f);
      }
      Destroy(gameObject);
    } else if (other.gameObject.tag.Equals("AI")) {
      AI aiControllerScript = other.gameObject.GetComponent<AI>();
      if (!aiControllerScript.IsOnTheFloor()) return;
      if (!isWaveSecondhand && aiControllerScript.IsResonanceable()) {
        aiControllerScript.GenerateSound(true, 200f);
      }
      if (sourceTag == "Player") {
        aiControllerScript.UpdatePlayerLastKnownPosition(sourcePosition);
      }
      Destroy(gameObject);
    }
  }
}
