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

  void Start () {
    itemCurrentHP = itemStartHP;
    itemStartColorR = GetComponent<Renderer>().material.color.r;
    itemStartColorG = GetComponent<Renderer>().material.color.g;
    itemStartColorB = GetComponent<Renderer>().material.color.b;

    myColor = GetComponent<Renderer>().material.color;
    // base.GenerateSound();
    tag = "Item";
  }

  public void Pull(float damage)
  {
    if (itemCurrentHP - damage > 0)
    {
      itemCurrentHP = itemCurrentHP - damage;
      CalculateCurrentColor(itemCurrentHP);
    }
    else
    {
      itemCurrentHP = 0;
      CalculateCurrentColor(itemCurrentHP);
      Destroy(gameObject);
    }
  }

  private void CalculateCurrentColor(float itemCurrentHP)
  {
    float percentCurrentHP = itemCurrentHP / itemStartHP;
    GetComponent<Renderer>().material.color = new Color(itemStartColorR * percentCurrentHP,
                                                        itemStartColorG * percentCurrentHP,
                                                        itemStartColorB * percentCurrentHP);
  }
}
