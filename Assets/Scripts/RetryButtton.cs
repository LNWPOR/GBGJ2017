using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButtton : MonoBehaviour {

	public void Retry()
    {
        SceneManager.LoadScene("Scene 3 test");
    }
}
