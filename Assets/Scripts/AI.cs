using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DNA {
  public static int size = 10;
  public Vector2[] genes = new Vector2[size];
  public float fitness;
  public bool isKilledPlayer = false;

  public DNA (Vector2[] newGenes) {
    genes = newGenes;
    fitness = 1;
  }

  public DNA (Vector3 ai, Vector3 player) {
    Vector2 aiPos = new Vector2(ai.x, ai.z);
    Vector2 playerPos = new Vector2(player.x, player.z);
    float angle = AngleBetweenVector2(aiPos, playerPos);
    float degLimit = 180;
    for (int i = 0; i < size; i++) {
      float deg = Random.Range(angle - degLimit / 2, angle + degLimit / 2);
      genes[i] = PolarToCoorVector2(deg, AI.scale);
    }
    fitness = 1;
  }

  public void CalculateFitness(Vector3 ai, Vector3 player) {
    float distance = Vector3.Distance(ai, player);
    float newFitness = 1 / distance * 100;
    newFitness = newFitness * newFitness;
    Debug.Log("DNA fitness: " + newFitness);
    fitness = newFitness;
    if (isKilledPlayer) {
      Debug.Log("KILLED!!");
      fitness *= 10;
    }
  }

  Vector2 PolarToCoorVector2(float degrees, float r)
  {
    float radians = degrees * Mathf.Deg2Rad;
    return new Vector2(r * Mathf.Sin(radians), r * Mathf.Cos(radians));
  }

  private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
  {
    Vector2 diference = vec2 - vec1;
    float sign = (vec2.y < vec1.y)? -1.0f : 1.0f;
    return Vector2.Angle(Vector2.right, diference) * sign;
  }
}

public class AI : Character {
  public static int size = 10;

  private List<DNA> dna = new List<DNA>();
  private DNA currentDNA;
  private Vector3 defaultPosition;
  private int timeSinceLastJump = 0;
  private int timeJumped = 0;

  public Vector3 playerLastKnownPosition;
  public static float scale = 4f;
  public static int degStep = 72;
  public static float rangeClose = 12f;

	// Use this for initialization
	void Start () {
    Debug.Log("ai position: " + transform.position);
    Debug.Log("player position: " + playerLastKnownPosition);
    currentDNA = new DNA(transform.position, playerLastKnownPosition);
    defaultPosition = transform.position;
    tag = "AI";
	}

	// Update is called once per frame
	void Update () {
    if (timeSinceLastJump == 40) {
      // for (int i = 0; i < currentDNA.genes.Length; i++) {
      Jump();
      // }
      timeSinceLastJump = 0;
    } else {
      timeSinceLastJump++;
    }
	}

  void updatePlayerLastKnowPosition(Vector3 pos) {
    playerLastKnownPosition = pos;
  }

  void Jump() {
    if (CanKillPlayer()) {
      currentDNA.isKilledPlayer = true;
    } else {
      if (timeJumped < currentDNA.genes.Length) {
        Vector2 gene = currentDNA.genes[timeJumped];
        transform.position = transform.position + new Vector3(gene.x, 0, gene.y);
        timeJumped++;
        base.GenerateSound(false);
      } else {
        currentDNA = GenerateNewDNA();
        transform.position = defaultPosition;
        timeJumped = 0;
      }
    }
  }

  bool CanKillPlayer() {
    float distance = Vector3.Distance(transform.position, playerLastKnownPosition);
    return distance < AI.scale / 2;
  }

  public DNA Crossover(DNA a, DNA b) {
    int mid = Random.Range(0, size - 1);
    Vector2[] newGenes = new Vector2[size];
    for (int i = 0; i < size; i++) {
      if (i < mid) {
        newGenes[i] = a.genes[i];
      } else {
        newGenes[i] = b.genes[i];
      }
    }
    return new DNA(newGenes);
  }

  DNA GenerateNewDNA() {
    currentDNA.CalculateFitness(transform.position, playerLastKnownPosition);
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
      return Crossover(parentA, parentB);
    } else {
      return new DNA(transform.position, playerLastKnownPosition);
    }
  }
}
