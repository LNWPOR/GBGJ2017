using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
  public GameObject wave;
  private float waveSpeed = 2.5f;
  public Color myColor;
  private int soundwaveCount = 24;

	// Use this for initialization
	void Start () {
    GetComponent<Renderer>().material.color = myColor;
	}

	// Update is called once per frame
	void Update () {

	}


  public void GenerateSound() {
    float degStep = 360f / soundwaveCount;
    for (float deg = 0; deg < 360f; deg += degStep) {
      GenerateSoundParticle(PolarToCoorVector3(deg, 1));
    }
  }

  Vector3 PolarToCoorVector3(float degrees, float r) {
    float radians = degrees * Mathf.Deg2Rad;
    return new Vector3(r * Mathf.Cos(radians), 0, r * Mathf.Sin(radians));
  }

  private void GenerateSoundParticle(Vector3 soundVector) {
    GameObject list;
    Rigidbody a;
    Renderer b;
    list = Instantiate(wave, transform.position, transform.rotation);
    a = list.GetComponent<Rigidbody>();
    a.velocity = soundVector * waveSpeed;
    b = list.GetComponent<Renderer>();
    b.material.color = myColor;
  }
}
