using UnityEngine;
using System.Collections;

public class enemyDestroyer : MonoBehaviour
{

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  void OnCollisionEnter2D(Collision2D col)
  {
    if (col.gameObject.tag == "Enemy Car" || col.gameObject.tag == "pt5" || col.gameObject.tag == "pt10")
    {
      Destroy(col.gameObject);
    }
  }


  void OnTriggerEnter2D(Collider2D col)
  {

    if (col.gameObject.tag == "pt10")
    {
      Destroy(col.gameObject);
    }
    else if (col.gameObject.tag == "pt5")
    {
      Destroy(col.gameObject);
    }
    else if (col.gameObject.tag == "pt-5")
    {
      Destroy(col.gameObject);
    }
    else if (col.gameObject.tag == "pt-10")
    {
      Destroy(col.gameObject);
    }
  }
}
