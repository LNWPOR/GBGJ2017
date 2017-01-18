using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
public class SocketIOTest : MonoBehaviour {
	void Start () {
        SocketOn();
    }

    private void SocketOn()
    {
        NetworkManager.Instance.Socket.On("NET_AVARIABLE", (SocketIOEvent evt) => {
            Debug.Log("Net Avariable");
        });
    }
}
