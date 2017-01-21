using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : Character {
  public int itemNumber;
  public float itemStartHP = 50;
  public float itemCurrentHP;
  private float itemStartColorR;
  private float itemStartColorG;
  private float itemStartColorB;
  public GameObject mainCamera;
  private float timeGotHit = 0;
  private float fadeOutTime = 8f;

  public override void Start () {
    base.Start();
    itemCurrentHP = itemStartHP;
    itemStartColorR = GetComponent<Renderer>().material.color.r;
    itemStartColorG = GetComponent<Renderer>().material.color.g;
    itemStartColorB = GetComponent<Renderer>().material.color.b;

    myColor = new Color(itemStartColorR, itemStartColorG, itemStartColorB, 1);
    GetComponent<Renderer>().enabled = false;
    tag = "Item";
  }

  public override void Update() {
    base.Update();
    if (timeGotHit > 0) timeGotHit--;
    else {
      GetComponent<Renderer>().enabled = false;
    }
    float fadingRate = timeGotHit / fadeOutTime;
    GetComponent<Renderer>().material.color = new Color(itemStartColorR * fadingRate, itemStartColorG * fadingRate, itemStartColorB * fadingRate, fadingRate);
  }

  public void Pull(float damage)
  {
    if (itemCurrentHP - damage > 0) {
      itemCurrentHP = itemCurrentHP - damage;
      CalculateCurrentColor(itemCurrentHP);
    }
    else {
      itemCurrentHP = 0;
      CalculateCurrentColor(itemCurrentHP);
      mainCamera.GetComponent<CameraController>().ZoomOutStep2();
      Destroy(gameObject);
    }
  }

  public void GetHitByWave() {
    timeGotHit = fadeOutTime;
    GetComponent<Renderer>().enabled = true;
  }

  private void CalculateCurrentColor(float itemCurrentHP) {
    float percentCurrentHP = itemCurrentHP / itemStartHP;
    // GetComponent<Renderer>().material.color = new Color(itemStartColorR, itemStartColorG, itemStartColorB, percentCurrentHP);
  }
}
