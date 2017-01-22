using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
  public GameObject player;
  public GameObject ai;
  private AI aiScript;
  private Controller playerScript;

  private float flashInterval = 300f;
  private float time = 0;

  private string stateLabel;
  private string isDivingLabel;

	// Use this for initialization
	void Start () {
    stateLabel = "Run, Forest, RUN!";
    ai = GameObject.Find("AI");
    aiScript = ai.GetComponent<AI>();
    playerScript = player.GetComponent<Controller>();
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
    if (playerScript.IsDiving()) {
      isDivingLabel = "DIVING";
    } else {
      isDivingLabel = "Above water";
    }
    time++;
    if (aiScript.GetKilledPlayer()) {
      stateLabel = "DIE!";
    }
	}

  void OnGUI() {
    GUI.Label(new Rect(10, 10, 200, 20), stateLabel);
    GUI.Label(new Rect(10, 30, 200, 20), isDivingLabel);
  }
}
