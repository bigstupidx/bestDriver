using UnityEngine;
using System.Collections;

public class carSpawnerSameWay : MonoBehaviour {
  public GameObject[] cars;
  float minPos = 0.10f;
  float maxPos = 2.63f;
  float delayTimer = 1.5f;
  public uiManager ui;

  float timer;

  // Use this for initialization
  void Start()
  {
    timer = delayTimer;
    InvokeRepeating("changeVelocity", 1.0f, 15f);

  }

  void changeVelocity()
  {
    delayTimer -= 0.1f;
  }


  // Update is called once per frame
  void Update()
  {
    if (!ui.gameOver)
    {
      timer -= Time.deltaTime;

      if (timer <= 0)
      {
        Vector3 carPos = new Vector3(Random.Range(minPos, maxPos), transform.position.y, transform.position.z);
        Instantiate(cars[Random.Range(0, cars.Length)], carPos, transform.rotation);

        timer = delayTimer;
      }
    }
  }
}
