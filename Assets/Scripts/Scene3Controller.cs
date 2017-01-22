using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene3Controller : MonoBehaviour {

	void Update () {
        if (GameObject.FindGameObjectsWithTag("Item").Length.Equals(0))
        {
            SceneManager.LoadScene("Result");
            //StartCoroutine(Wait(3f));
        }
    }

    /*
    private IEnumerator Wait(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            SceneManager.LoadScene("Result");
        }
    }
    */
}
