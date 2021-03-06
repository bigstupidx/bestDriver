﻿using UnityEngine;

public class carSpawner : MonoBehaviour
{

  public GameObject[] cars;
  float minPos = -2.53f;
  float maxPos = -0.1f;
  float delayTimer = 0.7f;
  public uiManager ui;

  int breakDelay = 8;

  float timer;
  int count;

  // Use this for initialization
  void Start()
  {
    timer = delayTimer;
    count = 0;
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
