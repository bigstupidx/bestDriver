using UnityEngine;
using System.Collections;

public class trackMove : MonoBehaviour {

    public float speed;
    Vector2 offset;
    public float delayTimer = 0.7f;

    float timer;
    int count;
    public uiManager ui;



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
                speed += 0.1f;
                timer = delayTimer;
                count++;
                if (count % 30 == 0)
                {
                    delayTimer -= 0.1f;
                }
            }
            offset = new Vector2(0, Time.time * speed);

            GetComponent<Renderer>().material.mainTextureOffset = offset;
        }
	}
}
