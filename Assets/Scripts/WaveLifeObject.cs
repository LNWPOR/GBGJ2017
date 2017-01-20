using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveLifeObject : MonoBehaviour {

    float life;
    void Start()
    {
        life = 0;
    }

    void Update()
    {
        life += 1;
        GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r, GetComponent<Renderer>().material.color.g, GetComponent<Renderer>().material.color.b, 1f - (life / 100));
        if (life == 100)
        {
            Destroy(gameObject);
        }
    }
}
