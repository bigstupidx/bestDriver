using UnityEngine;
using System.Collections;

public class carController : MonoBehaviour {

    public float carSpeed;
    public float maxPos = 2.5f;
    public float maxPosPont = 2.10f;
    Vector3 position;
    public uiManager ui;

    public audioManager am;

    bool currentPlataformAndroid = false;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        #if UNITY_ANDROID
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            currentPlataformAndroid = true;
        #else
            currentPlataformAndroid = false;
        #endif
        am.carSound.Play();
    }

	// Use this for initialization
	void Start () {
        
        position = transform.position;

        if (currentPlataformAndroid == true)
        {
            Debug.Log("Android");
        }
        else
        {
            Debug.Log("Windows");
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (currentPlataformAndroid == true)
        {
            AccelerometerMove();
        }
        else {
            position.x += Input.GetAxis("Horizontal") * carSpeed * Time.deltaTime;
        }
        position = transform.position;
        position.x = Mathf.Clamp(position.x, -maxPos, maxPos);
        bool value = true;
        if (position.x > maxPosPont || position.x < -maxPosPont)
        {
            value = false;
        }
        
        ui.setPontuating(value);
        transform.position = position;
    }
    
    void AccelerometerMove()
    {
        float x = Input.acceleration.x;
        if (x < -0.1f || x > 0.1)
        {
            Move(x * 25);
        }
        else
        {
            setVelocityZero();
        }
    }

    void OnCollisionEnter2D(Collision2D col) {

        if(col.gameObject.tag == "Enemy Car")
        {
            Destroy(gameObject);
            am.carSound.Stop();
            ui.gameOverActivated();
        }

    }

    public void Move(float value)
    {
        rb.velocity = new Vector2(value, 0);
    }


    public void setVelocityZero()
    {
        rb.velocity = Vector2.zero;
    }
}
