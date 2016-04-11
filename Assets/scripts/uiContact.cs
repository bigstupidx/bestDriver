using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class uiContact : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public void returnToMain()
  {
    SceneManager.LoadScene(0);
  }
}
