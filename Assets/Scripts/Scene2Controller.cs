using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene2Controller : MonoBehaviour {

    //public GameObject congratText;
    void Start () {
		
	}
	void Update () {
        if (GameObject.FindGameObjectsWithTag("Item").Length.Equals(0))
        {
            //congratText.GetComponent<TextMesh>().text = "You shall pass for now.";
            StartCoroutine(Wait(3f));
        }
    }

    private IEnumerator Wait(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            SceneManager.LoadScene("Scene 3 test");
        }
    }
}
