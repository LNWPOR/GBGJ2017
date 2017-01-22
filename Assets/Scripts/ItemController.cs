using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : Character {
  public int itemNumber;
  public float itemStartHP = 50;
  public float itemCurrentHP;
  private float itemStartColorR;
  private float itemStartColorG;
  private float itemStartColorB;
  public GameObject mainCamera;
  private float timeGotHit = 0;
  private float fadeOutTime = 8f;
  private bool isPulling = false;
  public float pullingCooldown = 1f;

  public override void Start () {
    base.Start();
    itemCurrentHP = itemStartHP;
        
    itemStartColorR = GetComponent<SpriteRenderer>().color.r;
    itemStartColorG = GetComponent<SpriteRenderer>().color.g;
    itemStartColorB = GetComponent<SpriteRenderer>().color.b;

    myColor = new Color(itemStartColorR, itemStartColorG, itemStartColorB, 1);
    GetComponent<SpriteRenderer>().enabled = false;
    tag = "Item";
  }

  public override void Update() {
    base.Update();
    if (timeGotHit > 0) timeGotHit--;
    else {
      GetComponent<SpriteRenderer>().enabled = false;
    }
    float fadingRate = timeGotHit / fadeOutTime;
    GetComponent<SpriteRenderer>().material.color = new Color(itemStartColorR * fadingRate, itemStartColorG * fadingRate, itemStartColorB * fadingRate, fadingRate);
  }

  public void Pull(float damage)
  {
        if (!isPulling)
        {
            if (itemCurrentHP - damage > 0)
            {
                itemCurrentHP = itemCurrentHP - damage;
                CalculateCurrentColor(itemCurrentHP);
            }
            else
            {
                itemCurrentHP = 0;
                CalculateCurrentColor(itemCurrentHP);
                //mainCamera.GetComponent<CameraController>().ZoomOutStep2();
                List<GameObject> itemInRangeList = GameObject.Find("ItemColliderCheck").GetComponent<ItemColliderCheck>().itemInRangeList;
                int index = itemInRangeList.FindIndex(x => x.gameObject.name.Equals(gameObject.name));
                itemInRangeList.RemoveAt(index);
                Destroy(gameObject);
            }
            isPulling = true;
            RunAwayAndTurnAround itemParentScript = transform.parent.GetComponent<RunAwayAndTurnAround>();
            itemParentScript.itemSpeed = itemParentScript.itemSpeedUp;
            StartCoroutine(WaitPulling(pullingCooldown));
        }
    
  }

  public void GetHitByWave() {
    timeGotHit = fadeOutTime;
    GetComponent<SpriteRenderer>().enabled = true;
  }

  private void CalculateCurrentColor(float itemCurrentHP) {
    float percentCurrentHP = itemCurrentHP / itemStartHP;
    // GetComponent<Renderer>().material.color = new Color(itemStartColorR, itemStartColorG, itemStartColorB, percentCurrentHP);
  }

    private IEnumerator WaitPulling(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            isPulling = false;
            RunAwayAndTurnAround itemParentScript = transform.parent.GetComponent<RunAwayAndTurnAround>();
            itemParentScript.itemSpeed = itemParentScript.itemStartSpeed;
        }
    }
}
