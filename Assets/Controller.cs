using UnityEngine;
using System.Collections;

public class Controller : Character {
  public Rigidbody player;
  public GameObject AI;
    public GameObject itemget;
  private AI aiScript;
  private Animator m_animator;
  public float speed;
  public int NumSound = 6;
  int isMove;
  public AudioSource sound, heartsound;
  public AudioClip[] footstep;
  public GameObject splashstep;
  private bool isRunning = false;
  private bool isCtrlDown = false;
  private bool isDiving = false;
  private float stepInterval = 12.5f;

  private float timeAfterDiving = 0f;
  private float timeAbleToDive = 120f;
  private float divingCooldownCount;
  private float divingCooldown = 200f;

  public static float waveLifespan = 30f;

    private bool isPulling = false;
    public float pullingCooldown = 1f;
    ItemController itemControllerScript;
    RunAwayAndTurnAround itemRunScript;


  public override void Start() {
    base.Start();
    isMove = 0;
    tag = "Player";
    AI = GameObject.Find("AI");
    aiScript = AI.GetComponent<AI>();
		m_animator = transform.GetChild(3).GetComponent<Animator> ();
    divingCooldownCount = divingCooldown;
  }

  public override void Update() {
    base.Update();
    float z = 0, x = 0;
    if (Input.GetKey("w")) {
      z = 1;
    }
    if (Input.GetKey("s")) {
      z = -1;
    }
    if (Input.GetKey("a")) {
      x = -1;
    }
    if (Input.GetKey("d")) {
      x = 1;
    }
    if ((Input.GetKeyUp(KeyCode.LeftControl) && timeAfterDiving > 0) || timeAfterDiving >= timeAbleToDive) {
      divingCooldownCount = 0;
      timeAfterDiving = 0;
    }
    if (Input.GetKey(KeyCode.LeftControl)) {
      isCtrlDown = true;
    } else {
      isCtrlDown = false;
    }
    if (divingCooldownCount < divingCooldown) divingCooldownCount++;
    if (z != 0 || x != 0) {
      if (isCtrlDown && timeAfterDiving < timeAbleToDive && divingCooldownCount >= divingCooldown) {
        isDiving = true;
        timeAfterDiving++;
        isMove = 0;
        player.velocity = new Vector3(x, 0, z).normalized * speed * 2f;
        splashstep.GetComponent<EllipsoidParticleEmitter>().maxSize = 0.3f;
      } else {
        isDiving = false;
        if (!isCtrlDown) timeAfterDiving = 0f;
        if (isMove == 0) base.GenerateSound(false, waveLifespan);
        splashstep.GetComponent<EllipsoidParticleEmitter>().maxSize = 0.3f;
        player.velocity = new Vector3(x, 0, z).normalized * speed * 2f;
        isMove += 1;
		m_animator.SetBool ("IsMove", true);
      }
    }
    else {
      isRunning = false;
      isMove = 0;
	  m_animator.SetBool ("IsMove", false);
      splashstep.GetComponent<EllipsoidParticleEmitter>().maxSize = 0f;
      player.velocity = new Vector3(0, 0, 0);
    }
    if (isMove >= stepInterval / 2) {
      isRunning = true;
    }
    if (isMove >= stepInterval) {
      int randomFootstep = Random.Range(0, NumSound);
      sound.PlayOneShot(footstep[randomFootstep], 1);
      base.GenerateSound(false, waveLifespan);
      isMove = 1;
    }
    Plane playerPlane = new Plane(Vector3.up, transform.position);
  	Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
  	float hitdist = 0.0f;
  	if (playerPlane.Raycast(ray, out hitdist)) {
      Vector3 targetPosition = ray.GetPoint(hitdist);
      transform.LookAt(targetPosition);
      transform.Rotate(90, 0, 0);
    	// transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
		}
    soundBeat();
  }

  void soundBeat()
  {
    float DisPlaAI = Vector3.Distance(player.transform.position, AI.transform.position);
    if (DisPlaAI > 20)
    {
      heartsound.pitch = 1;
    }
    else
    {
      heartsound.pitch = 3 - 2 * (DisPlaAI / 20);
    }
  }

  public bool IsDiving() {
    return isDiving;
  }

  void OnTriggerStay(Collider other) {
    if (other.gameObject.tag.Equals("Item"))
    {
      float damage = 5;
      itemControllerScript = other.gameObject.GetComponent<ItemController>();
      itemRunScript = other.gameObject.transform.parent.gameObject.GetComponent<RunAwayAndTurnAround>();
      if (Input.GetKeyDown("space") && !isPulling)
      {
        itemget.GetComponent<EllipsoidParticleEmitter>().maxSize = 1f;
        itemget.transform.position = this.transform.position;
                // base.GenerateSound(true, 50f);
                aiScript.UpdatePlayerLastKnownPosition(transform.position);
        itemControllerScript.Pull(5);
        isPulling = true;
        itemRunScript.itemSpeed = itemRunScript.itemSpeedUp;
        StartCoroutine(WaitPulling(pullingCooldown));
      }
        }
       
    }
    private IEnumerator WaitPulling(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            isPulling = false;
            itemRunScript.itemSpeed = itemRunScript.itemSpeedUp;
        }
    }
}
