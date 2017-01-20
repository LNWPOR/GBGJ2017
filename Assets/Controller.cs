using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
    public Rigidbody player;
    public GameObject wave;
    public float speed, waveSpeed;
    int isMove;
    public Color myColor;

    void Start()
    {
        GetComponent<Renderer>().material.color = myColor;
        Debug.Log(myColor);
        isMove = 0;
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

    void Update()
    {
        float z=0,x=0;
        if (Input.GetKey("w"))
        {
            z = 1;
        }
        if (Input.GetKey("s"))
        {
            z = -1;
        }
        if (Input.GetKey("a"))
        {
            x = -1;
        }
        if (Input.GetKey("d"))
        {
            x = 1;
        }
        if (z != 0 || x != 0) isMove += 1;
        else isMove = 0;
        if (isMove == 20)
        {
            generateSound();
        }
        if(isMove > 20)
        {
            isMove += 1;
            if (isMove == 40) isMove = 0;
            player.velocity = new Vector3(0, 0, 0);
        }
        else
        {
            player.velocity = new Vector3(x, 0, z).normalized * speed;
        }
    }
}