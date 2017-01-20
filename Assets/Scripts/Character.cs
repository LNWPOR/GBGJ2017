using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
  public GameObject wave;
  private float waveSpeed = 4f;
  public Color myColor;
  private int soundwaveCount = 30;
  public string tag;

	// Use this for initialization
	public virtual void Start () {
    GetComponent<Renderer>().material.color = myColor;
	}

	// Update is called once per frame
	public virtual void Update () {

	}


  public void GenerateSound(bool isSecondhand, float lifespan) {
    float degStep = 360f / soundwaveCount;
    Debug.Log(lifespan);
    for (float deg = 0; deg < 360f; deg += degStep) {
      GenerateSoundParticle(PolarToCoorVector3(deg, 1), isSecondhand, lifespan);
    }
  }

  Vector3 PolarToCoorVector3(float degrees, float r) {
    float radians = degrees * Mathf.Deg2Rad;
    return new Vector3(r * Mathf.Cos(radians), 0, r * Mathf.Sin(radians));
  }

  private void GenerateSoundParticle(Vector3 soundVector, bool isSecondhand, float lifespan) {
    GameObject list;
    Rigidbody a;
    Renderer b;
    list = Instantiate(wave, transform.position, transform.rotation);
    a = list.GetComponent<Rigidbody>();
    a.velocity = soundVector * waveSpeed;
    b = list.GetComponent<Renderer>();
    b.material.color = myColor;
    WaveLife controller = list.GetComponent<WaveLife>();
    controller.SetLifespan(lifespan);
    controller.SetSourceTag(tag);
    controller.SetIsSecondhand(isSecondhand);
  }
}
