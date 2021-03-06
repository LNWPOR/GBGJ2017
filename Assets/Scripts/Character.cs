﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
  public GameObject wave;
  private float waveSpeed = 6f;
  public Color myColor;
  private int soundwaveCount = 60;
  public string tag;
  private float lastHitByWave = 0;
  public float hitByWaveInterval = 20f;

	// Use this for initialization
	public virtual void Start () {
        
    }

	// Update is called once per frame
	public virtual void Update () {
    lastHitByWave++;
	}

  public virtual void Awake () {
	}

  public bool IsResonanceable() {
    return lastHitByWave >= hitByWaveInterval;
  }

  public void GenerateSound(bool isSecondhand, float lifespan) {
    float degStep = 360f / soundwaveCount;
    if (isSecondhand) lastHitByWave = 0;
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
    b = list.GetComponent<TrailRenderer>();
    b.materials[0].color = myColor;
    WaveLife controller = list.GetComponent<WaveLife>();
    controller.SetSourcePosition(transform.position);
    controller.SetLifespan(lifespan);
    controller.SetSourceTag(tag);
    controller.SetIsSecondhand(isSecondhand);
  }

}
