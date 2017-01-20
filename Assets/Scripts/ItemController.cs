using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {

    public int ItemNumber;
    public float itemStartHP = 50;
    public float itemCurrentHP;
    private float itemStartColorA;
    private float itemStartColorB;
    private float itemStartColorG;

	void Start () {
        itemCurrentHP = itemStartHP;
        itemStartColorA = GetComponent<Renderer>().material.color.a;
        itemStartColorB = GetComponent<Renderer>().material.color.b;
        itemStartColorG = GetComponent<Renderer>().material.color.g;
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
            Debug.Log("Destroy Item");
        }
    }

    private void CalculateCurrentColor(float itemCurrentHP)
    {
        float percentCurrentHP = itemCurrentHP / itemStartHP;
        GetComponent<Renderer>().material.color = new Color(itemStartColorA * percentCurrentHP,
                                                            itemStartColorB * percentCurrentHP,
                                                            itemStartColorG * percentCurrentHP);
    }

}
