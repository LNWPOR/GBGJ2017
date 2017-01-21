using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DNA {
  public static int size = 6;
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
    float degLimit = 120;
    for (int i = 0; i < size; i++) {
      float deg = Random.Range(angle - degLimit / 2, angle + degLimit / 2);
      float speed = Random.Range(AI.speed / 10, AI.speed);
      Vector2 newVector = PolarToCoorVector2(deg, speed);
      genes[i] = newVector;
    }
    fitness = 1;
  }

  public void CalculateFitness(Vector3 ai, Vector3 player) {
    float distance = Vector3.Distance(ai, player);
    float newFitness = 1 / distance * 20;
    newFitness = newFitness * newFitness;
    fitness = newFitness;
    if (isKilledPlayer) {
      fitness *= 10;
    }
  }

  Vector2 PolarToCoorVector2(float degrees, float r)
  {
    float radians = degrees * Mathf.Deg2Rad;
    return new Vector2(r * Mathf.Cos(radians), r * Mathf.Sin(radians));
  }

  public static float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
  {
    Vector2 diff = vec2 - vec1;
    float sign = (vec2.y < vec1.y)? -1.0f : 1.0f;
    return Vector2.Angle(Vector2.right, diff) * sign;
  }
}

public class AI : Character {
  public static int size = 10;
  public GameObject player;

  // private List<DNA> dna = new List<DNA>();
  private DNA currentDNA;
  private Vector3 defaultPosition;
  private int timeSinceLastJump = 0;
  private int timeJumped = 0;

  private Vector3 playerLastKnownPosition;
  public static float speed = 6f;
  public static int jumpInterval = 50;
  public static int beforeJumpInterval = jumpInterval / 2;
  public static int degStep = 20;
  public static float rangeClose = 18f;

  private List<DNA>[,] dnaLegacy = new List<DNA>[degStep,2];
  private int currentDegRegion = 0;
  private int currentRangeRegion = 0;
  public bool isOnTheFloor = true;
  private bool isAddedScene = false;

	// Use this for initialization
	public override void Start () {
    base.Start();
    playerLastKnownPosition = player.transform.position;
    InitDNALegacy();
    FindRegion();
    currentDNA = new DNA(transform.position, playerLastKnownPosition);
    defaultPosition = transform.position;
    // GetComponent<Renderer>().enabled = false;
    tag = "AI";
	}

	// Update is called once per frame
	public override void Update () {
    base.Update();
    if (isAddedScene) return;
    if (CanKillPlayer()) return;
    if (timeSinceLastJump == jumpInterval) {
      Jump();
      timeSinceLastJump = 0;
      defaultPosition = transform.position;
      isOnTheFloor = true;
    } else {
      timeSinceLastJump++;
      if (timeSinceLastJump >= beforeJumpInterval) {
        isOnTheFloor = false;
        Vector2 gene = currentDNA.genes[timeJumped];
        int time = timeSinceLastJump - beforeJumpInterval;
        float fracJourney = time * 1f / (jumpInterval - beforeJumpInterval);
        Vector3 targetPosition = defaultPosition + new Vector3(gene.x, 0, gene.y);
        transform.LookAt(targetPosition);
        transform.Rotate(90, 0, 0);
        transform.position = Vector3.Lerp(defaultPosition, targetPosition, fracJourney);
      }
    }
	}

  void Awake() {
    DontDestroyOnLoad(gameObject);
  }

  public List<DNA>[,] GetDNALegacy() {
    return dnaLegacy;
  }

  void InitDNALegacy() {
    for (int i = 0; i < degStep; i++) {
      for (int j = 0; j < 2; j++) {
        dnaLegacy[i,j] = new List<DNA>();
      }
    }
  }

  public void UpdatePlayerLastKnownPosition(Vector3 pos) {
    playerLastKnownPosition = pos;
    currentDNA = GenerateNewDNA();
    timeSinceLastJump = beforeJumpInterval;
    timeJumped = 0;
  }

  void Jump() {
    if (timeJumped < currentDNA.genes.Length - 1) {
      timeJumped++;
      base.GenerateSound(false, 50f);
    } else {
      timeJumped = 0;
      base.GenerateSound(false, 50f);
      UpdatePlayerLastKnownPosition(player.transform.position);
    }
  }

  public bool CanKillPlayer() {
    if (!isOnTheFloor) return false;
    float distance = Vector3.Distance(transform.position, player.transform.position);
    if (distance < 1.2f) {
      Debug.Log("DIST: " + distance);
      if (!isAddedScene) {
        SceneManager.LoadScene("Result");
        isAddedScene = true;
      }
      return true;
    } else {
      return false;
    }
  }

  public DNA Crossover(DNA a, DNA b) {
    int mid = Random.Range(0, DNA.size - 1);
    Vector2[] newGenes = new Vector2[DNA.size];
    for (int i = 0; i < DNA.size; i++) {
      if (i < mid && i < a.genes.Length) {
        newGenes[i] = a.genes[i];
      } else if (i < b.genes.Length) {
        newGenes[i] = b.genes[i];
      }
    }
    return new DNA(newGenes);
  }

  public bool IsOnTheFloor() {
    return isOnTheFloor;
  }

  void FindRegion() {
    float distance = Vector3.Distance(transform.position, playerLastKnownPosition);
    int rangeRegion;
    if (distance <= rangeClose) {
      rangeRegion = 0;
    } else {
      rangeRegion = 1;
    }
    Vector2 aiPos = new Vector2(transform.position.x, transform.position.z);
    Vector2 playerPos = new Vector2(playerLastKnownPosition.x, playerLastKnownPosition.z);
    float angle = DNA.AngleBetweenVector2(aiPos, playerPos);
    int degRegion = Mathf.FloorToInt((angle + 360) % 360 / degStep);
    currentRangeRegion = rangeRegion;
    currentDegRegion = degRegion;
  }

  DNA GenerateNewDNA() {
    currentDNA.CalculateFitness(transform.position, playerLastKnownPosition);
    List<DNA> dna = dnaLegacy[currentDegRegion,currentRangeRegion];
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
    dnaLegacy[currentDegRegion,currentRangeRegion] = dna;

    FindRegion();
    dna = dnaLegacy[currentDegRegion,currentRangeRegion];
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
