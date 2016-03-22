using UnityEngine;
using System.Collections;

public class carController : MonoBehaviour
{

  public float carSpeed;
  public float maxPos = 2.5f;
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
  void Start()
  {

    position = transform.position;


  }

  // Update is called once per frame
  void Update()
  {

    if (currentPlataformAndroid == true)
    {
      AccelerometerMove();
    }
    else {
      position.x += Input.GetAxis("Horizontal") * carSpeed * Time.deltaTime;
    }
    position = transform.position;
    position.x = Mathf.Clamp(position.x, -maxPos, maxPos);
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

  void OnCollisionEnter2D(Collision2D col)
  {

    if (col.gameObject.tag == "Enemy Car")
    {
      Destroy(gameObject);
      am.carSound.Stop();
      ui.gameOverActivated();
    }

  }

  void OnTriggerExit2D(Collider2D col)
  {
    if (col.gameObject.tag == "Margin")
    {
      Sprite sc = Resources.Load<Sprite>("Sprites/marginNoColor");
      SpriteRenderer sr = col.gameObject.GetComponent<SpriteRenderer>();
      sr.sprite = sc;
      ui.setPontuating(true);
    }
  }

  void OnTriggerEnter2D(Collider2D col)
  {

    if (col.gameObject.tag == "pt10")
    {
      Destroy(col.gameObject);
      ui.score = ui.score + 10;
    }
    else if (col.gameObject.tag == "pt5")
    {
      Destroy(col.gameObject);
      ui.score = ui.score + 5;
    }
    else if (col.gameObject.tag == "pt-5")
    {
      Destroy(col.gameObject);
      ui.score = ui.score - 5;
    }
    else if (col.gameObject.tag == "pt-10")
    {
      Destroy(col.gameObject);
      ui.score = ui.score -10;
    }
    else if (col.gameObject.tag == "Margin")
    {
      Sprite sc = Resources.Load<Sprite>("Sprites/margin");
      SpriteRenderer sr = col.gameObject.GetComponent<SpriteRenderer>();
      sr.sprite = sc;
      ui.setPontuating(false);
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
