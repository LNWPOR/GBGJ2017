using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DNA {
  public static int size = 10;
  public Vector2[] genes = new Vector2[size];
  public float fitness;

  public DNA (Vector2[] newGenes) {
    genes = newGenes;
    fitness = 1;
  }

  public DNA () {
    for (int i = 0; i < size; i++) {
      float scale = 0.3f;
      float x = Random.Range(-scale, scale);
      float y = Random.Range(-scale, scale);
      genes[i] = new Vector2(x, y);
    }
    fitness = 1;
  }

  public void calculateFitness(GameObject ai, GameObject player) {
    float distance = Vector3.Distance(ai.transform.position, player.transform.position);
    float newFitness = 1 / distance * 30;
    newFitness = newFitness * newFitness * newFitness;
    Debug.Log("DNA fitness: " + newFitness);
    fitness = newFitness;
  }

  public DNA crossover(DNA partner) {
    int mid = Random.Range(0, size - 1);
    Vector2[] newGenes = new Vector2[size];
    for (int i = 0; i < size; i++) {
      if (i < mid) {
        newGenes[i] = genes[i];
      } else {
        newGenes[i] = partner.genes[i];
      }
    }
    return new DNA(newGenes);
  }
}

public class AI : Objective {

  public GameObject player;

  private static int size = 20;
  private List<DNA> dna = new List<DNA>();
  private DNA currentDNA;
  private Vector3 defaultPosition;
  private int timeSinceLastJump = 0;
  private int timeJumped = 0;

	// Use this for initialization
	void Start () {
    Debug.Log("ai position: " + transform.position);
    Debug.Log("player position: " + player.transform.position);
    currentDNA = new DNA();
    defaultPosition = transform.position;
	}

	// Update is called once per frame
	void Update () {
    if (timeSinceLastJump == 1) {
      if (timeJumped < currentDNA.genes.Length) {
        Vector2 gene = currentDNA.genes[timeJumped];
        transform.position = transform.position + new Vector3(gene.x, 0, gene.y);
        timeJumped++;
      } else {
        currentDNA = GenerateNewDNA();
        transform.position = defaultPosition;
        timeJumped = 0;
      }
      timeSinceLastJump = 0;
    } else {
      timeSinceLastJump++;
    }
	}

  DNA GenerateNewDNA() {
    currentDNA.calculateFitness(gameObject, player);

    // Remove less fit dna
    if (dna.Count == size) {
      int lessFitIndex = 0;
      float minFitness = 1000000f;
      for (int i = 0; i < dna.Count; i++) {
        if (minFitness > dna[i].fitness) {
          lessFitIndex = i;
          minFitness = dna[i].fitness;
        }
      }
      if (currentDNA.fitness > lessFitIndex) {
        dna[lessFitIndex] = currentDNA;
      }
    } else {
      dna.Add(currentDNA);
    }

    // Mating
    List<DNA> matingPool = new List<DNA>();
    for (int i = 0; i < dna.Count; i++) {
      for (int j = 0; j < dna[i].fitness; j++) {
        matingPool.Add(dna[i]);
      }
    }

    float mutationRate = 0.1f;
    float prob = Random.Range(0.0f, 1.0f);
    if (dna.Count >= 2 && prob > mutationRate) {
      int indexA = Random.Range(0, matingPool.Count - 1);
      DNA parentA = matingPool[indexA];
      int indexB = Random.Range(0, matingPool.Count - 1);
      while (indexA == indexB) {
        indexB = Random.Range(0, matingPool.Count - 1);
      }
      DNA parentB = matingPool[indexB];
      // Debug.Log(parentA.fitness);
      // Debug.Log(parentB.fitness);
      return parentA.crossover(parentB);
    } else {
      return new DNA();
    }
  }
}
