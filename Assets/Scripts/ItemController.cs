using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {

    public int ItemNumber;
    public float itemStartHP = 50;
    public float itemCurrentHP;
    private float itemStartColorR;
    private float itemStartColorG;
    private float itemStartColorB;

    public Color myColor;

    void Start () {
        itemCurrentHP = itemStartHP;
        itemStartColorR = GetComponent<Renderer>().material.color.r;
        itemStartColorG = GetComponent<Renderer>().material.color.g;
        itemStartColorB = GetComponent<Renderer>().material.color.b;

        GetComponent<Renderer>().material.color = myColor;
        Debug.Log(myColor);
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
        GetComponent<Renderer>().material.color = new Color(itemStartColorR * percentCurrentHP,
                                                            itemStartColorG * percentCurrentHP,
                                                            itemStartColorB * percentCurrentHP);
    }
    
    public void generateSound()
    {
        GameObject list;
        Rigidbody a;
        Renderer b;
        list = Instantiate(wave, player.position, player.rotation);
        a = list.GetComponent<Rigidbody>();
        a.velocity = new Vector3(1, 0, 0) * waveSpeed;
        b = list.GetComponent<Renderer>();
        b.material.color = myColor;
        list = Instantiate(wave, player.position, player.rotation);
        a = list.GetComponent<Rigidbody>();
        a.velocity = new Vector3(-1, 0, 0) * waveSpeed;
        b = list.GetComponent<Renderer>();
        b.material.color = myColor;
        list = Instantiate(wave, player.position, player.rotation);
        a = list.GetComponent<Rigidbody>();
        a.velocity = new Vector3(0, 0, 1) * waveSpeed;
        b = list.GetComponent<Renderer>();
        b.material.color = myColor;
        list = Instantiate(wave, player.position, player.rotation);
        a = list.GetComponent<Rigidbody>();
        a.velocity = new Vector3(0, 0, -1) * waveSpeed;
        b = list.GetComponent<Renderer>();
        b.material.color = myColor;
        list = Instantiate(wave, player.position, player.rotation);
        a = list.GetComponent<Rigidbody>();
        a.velocity = new Vector3(1, 0, 1).normalized * waveSpeed;
        b = list.GetComponent<Renderer>();
        b.material.color = myColor;
        list = Instantiate(wave, player.position, player.rotation);
        a = list.GetComponent<Rigidbody>();
        a.velocity = new Vector3(1, 0, -1).normalized * waveSpeed;
        b = list.GetComponent<Renderer>();
        b.material.color = myColor;
        list = Instantiate(wave, player.position, player.rotation);
        a = list.GetComponent<Rigidbody>();
        a.velocity = new Vector3(-1, 0, 1).normalized * waveSpeed;
        b = list.GetComponent<Renderer>();
        b.material.color = myColor;
        list = Instantiate(wave, player.position, player.rotation);
        a = list.GetComponent<Rigidbody>();
        a.velocity = new Vector3(-1, 0, -1).normalized * waveSpeed;
        b = list.GetComponent<Renderer>();
        b.material.color = myColor;
    }

}
