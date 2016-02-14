using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class carSpawner : MonoBehaviour {

    public GameObject[] cars;
    public float maxPos = 2.5f;
    public float delayTimer = 0.7f;
    public uiManager ui;

    int breakDelay = 8;

    float timer;
    int count;

	// Use this for initialization
	void Start () {
        timer = delayTimer;
        count = 0;
        
	}


    // Update is called once per frame
    void Update () {
        if (!ui.gameOver)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                Vector3 carPos = new Vector3(Random.Range(-maxPos, maxPos), transform.position.y, transform.position.z);
                Instantiate(cars[Random.Range(0, 5)], carPos, transform.rotation);

                timer = delayTimer;
                count++;
                if (count % breakDelay == 0)
                {
                    delayTimer -= 0.1f;
                    breakDelay += 3;
                    count = 0;
                }
            }
        }
    }
}
