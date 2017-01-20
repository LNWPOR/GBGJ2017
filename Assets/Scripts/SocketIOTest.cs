using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
public class SocketIOTest : MonoBehaviour {
	void Start () {
        StartCoroutine(Wait(2f));
        //SocketOn();
    }

    private void SocketOn()
    {
        /*
        NetworkManager.Instance.Socket.On("NET_AVARIABLE", (SocketIOEvent evt) => {
            Debug.Log("Net Avariable");
        });
        */
        
    }

    private IEnumerator Wait(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            NetworkManager.Instance.Socket.Emit("HiServer");
        }
    }
}
