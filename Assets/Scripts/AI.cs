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
  public GameObject splashstep;
  public AudioSource sound;
  public AudioClip[] footstep;
  private Controller playerScript;

  // private List<DNA> dna = new List<DNA>();
  private DNA currentDNA;
  private Vector3 defaultPosition;
  private int timeSinceLastJump = 0;
  private int timeJumped = 0;

  private Vector3 playerLastKnownPosition;
  public static float speed = 6f;
  public static int jumpInterval = 40;
  public static int beforeJumpInterval = jumpInterval / 2;
  public static int degStep = 20;
  public static float rangeClose = 18f;

  public static List<DNA>[,] dnaLegacy;
  private int currentDegRegion = 0;
  private int currentRangeRegion = 0;
  public bool isOnTheFloor = true;
  private bool isAddedScene = false;
  private bool killedPlayer = false;

	// Use this for initialization
	public override void Start () {
    base.Start();
    if (AI.dnaLegacy == null) AI.dnaLegacy = new List<DNA>[degStep,2];
    tag = "AI";
    killedPlayer = false;
    playerLastKnownPosition = player.transform.position;
    InitDNALegacy();
    FindRegion();
    currentDNA = new DNA(transform.position, playerLastKnownPosition);
    defaultPosition = transform.position;
    playerScript = player.GetComponent<Controller>();
    // GetComponent<Renderer>().enabled = false;
	}

	// Update is called once per frame
	public override void Update () {
    base.Update();
    if (killedPlayer) return;
    if (isAddedScene) return;
    if (timeSinceLastJump == jumpInterval) {
      Jump();
      timeSinceLastJump = 0;
      defaultPosition = transform.position;
      isOnTheFloor = true;
    } else {
      timeSinceLastJump++;
      splashstep.GetComponent<EllipsoidParticleEmitter>().maxSize = 0f;
      if (timeSinceLastJump >= beforeJumpInterval) {
        isOnTheFloor = false;
        Vector2 gene = currentDNA.genes[timeJumped];
        int time = timeSinceLastJump - beforeJumpInterval;
        float fracJourney = time * 1f / (jumpInterval - beforeJumpInterval);
        Vector3 targetPosition = defaultPosition + new Vector3(gene.x, 0, gene.y);
        transform.LookAt(targetPosition);
        transform.Rotate(0, 0, 0);
        transform.position = Vector3.Lerp(defaultPosition, targetPosition, fracJourney);
      }
    }
	}

  void Awake() {
    player = GameObject.Find("Player");
    LogLegacyCount();
  }

  public List<DNA>[,] GetDNALegacy() {
    return AI.dnaLegacy;
  }

  void InitDNALegacy() {
    for (int i = 0; i < degStep; i++) {
      for (int j = 0; j < 2; j++) {
        AI.dnaLegacy[i,j] = new List<DNA>();
      }
    }
  }

  void LogLegacyCount() {
    if (AI.dnaLegacy == null) return;
    int sumCount = 0;
    for (int i = 0; i < degStep; i++) {
      for (int j = 0; j < 2; j++) {
        if (AI.dnaLegacy[i,j] != null) sumCount += AI.dnaLegacy[i,j].Count;
      }
    }
    Debug.Log("Legacy count: " + sumCount);
  }

  public void UpdatePlayerLastKnownPosition(Vector3 pos) {
    playerLastKnownPosition = pos;
    currentDNA = GenerateNewDNA();
    timeSinceLastJump = beforeJumpInterval;
    timeJumped = 0;
  }

  void Jump() {
    splashstep.GetComponent<EllipsoidParticleEmitter>().maxSize = 5f;
    if (timeJumped < currentDNA.genes.Length - 1) {
      int randomFootstep = Random.Range(0, 2);
      sound.PlayOneShot(footstep[randomFootstep], 1);
      timeJumped++;
      base.GenerateSound(false, 50f);
    } else {
      timeJumped = 0;
      // UpdatePlayerLastKnownPosition(player.transform.position);
      currentDNA = GenerateNewDNA();
      base.GenerateSound(false, 50f);
    }
  }

  public void KillPlayer() {
    if (!isOnTheFloor) return;
    if (playerScript.IsDiving()) return;
    killedPlayer = true;
    if (!isAddedScene) {
      SceneManager.LoadScene("Result");
      isAddedScene = true;
    }
  }

  public bool GetKilledPlayer() {
    return killedPlayer;
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
    List<DNA> dna = AI.dnaLegacy[currentDegRegion,currentRangeRegion];
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
    AI.dnaLegacy[currentDegRegion,currentRangeRegion] = dna;

    FindRegion();
    dna = AI.dnaLegacy[currentDegRegion,currentRangeRegion];
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

  void OnTriggerEnter(Collider other) {
    Debug.Log(other.gameObject.tag);
    if (other.gameObject.tag.Equals("Player")) {
      KillPlayer();
    }
  }
}
