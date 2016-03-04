using UnityEngine;
using System.Collections;

public class bonusSpawn : MonoBehaviour
{

  public GameObject[] objects;
  public float maxPos = 2.5f;
  public float delayTimer = 100f;
  public uiManager ui;

  float timer;

  // Use this for initialization
  void Start()
  {
    timer = delayTimer;

  }


  // Update is called once per frame
  void Update()
  {
    if (!ui.gameOver)
    {
      timer -= Time.deltaTime;

      if (timer <= 0)
      {
        Vector3 carPos = new Vector3(Random.Range(-maxPos, maxPos), transform.position.y, transform.position.z);
        Instantiate(objects[Random.Range(0, objects.Length)], carPos, transform.rotation);

        timer = delayTimer;
      }
    }
  }
}
