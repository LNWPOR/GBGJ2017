using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour {
  private string label = "Result page";

	// Use this for initialization
	void Start () {
	}

  void OnGUI() {
    int height = 20, currentHeight = 10;
    GUI.Label(new Rect(10, currentHeight, 600, height), label);
    currentHeight += height; currentHeight += height;
    GUI.Label(new Rect(10, currentHeight, 600, height), "Avg. all fitness: " + AvgFitness());
    currentHeight += height; currentHeight += height;
    for (int i = 0; i < AI.degStep; i++) {
      for (int j = 0; j < 2; j++) {
        string avgLabel = AvgFitnessInList(AI.dnaLegacy[i,j]);
        GUI.Label(new Rect(10 + j * 300, currentHeight, 600, height), "Deg[" + i + "," + j + "]: " + avgLabel);
      }
      currentHeight += height;
    }
  }

	void Update () {
	}

  string AvgFitness() {
    int count = 0;
    float sum = 0;
    for (int i = 0; i < AI.degStep; i++) {
      for (int j = 0; j < 2; j++) {
        for (int k = 0; k < AI.dnaLegacy[i,j].Count; k++) {
          sum += AI.dnaLegacy[i,j][k].fitness;
          count++;
        }
      }
    }
    if (count > 0) return (sum / count) + "";
    else return "NaN";
  }

  string AvgFitnessInList(List<DNA> dnaList) {
    int count = 0;
    float sum = 0;
    for (int k = 0; k < dnaList.Count; k++) {
      sum += dnaList[k].fitness;
      count++;
    }
    if (count > 0) return (sum / count) + "";
    else return "NaN";
  }
}
