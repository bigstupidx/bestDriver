using UnityEngine;
using System.Collections;

public class integration : MonoBehaviour {
  public GameObject sceneCarSelection;
  public GameObject sceneLevel1;


  // Use this for initialization
  void Start () {
    sceneCarSelection.SetActive(true);
    sceneLevel1.SetActive(false);

  }
	
	// Update is called once per frame
	void Update () {
	
	}
}
