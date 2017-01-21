using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
  public GameObject player;
  public GameObject ai;
  private AI aiScript;

  private float flashInterval = 300f;
  private float time = 0;

  private string stateLabel;

	// Use this for initialization
	void Start () {
    stateLabel = "Run, Forest, RUN!";
    aiScript = ai.GetComponent<AI>();
	}

	// Update is called once per frame
	void Update () {
    if (time == 0) {
      aiScript.UpdatePlayerLastKnownPosition(player.transform.position);
      stateLabel = "I. SEE. YOU!";
    } else if (time == flashInterval / 5) {
      stateLabel = "Run, Forest, RUN!";
    } else if (time >= flashInterval) {
      time = 0;
    }
    time++;
    if (aiScript.CanKillPlayer()) {
      stateLabel = "DIE!";
    }
	}

  void OnGUI() {
    GUI.Label(new Rect(10, 10, 200, 50), stateLabel);
  }
}
