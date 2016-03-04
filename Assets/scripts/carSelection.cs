using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class carSelection : MonoBehaviour {

  public GameObject main;
  public Button left;
  public Button right;
  int lastPosition = 0;
  public GameObject[] carPenels;
  // Use this for initialization
  void Start () {
    string sprite = PlayerPrefs.GetString("sprite");
    if (!sprite.Equals(""))
    {
      Sprite sc = Resources.Load<Sprite>("Sprites/" + PlayerPrefs.GetString("sprite"));
      SpriteRenderer sr = main.GetComponent<SpriteRenderer>();
      sr.sprite = sc;
    }
    left.interactable = false;
	}

  public void Go()
  {
    SpriteRenderer sr = main.GetComponent<SpriteRenderer>();

    PlayerPrefs.SetString("sprite", sr.sprite.name);
    SceneManager.LoadScene(1);
    
  }

  public void moveRight()
  {
   
    carPenels[lastPosition].SetActive(false);
    lastPosition++;
    carPenels[lastPosition].SetActive(true);
    if (lastPosition > 0)
    {
      left.interactable = true;
    }
    if (lastPosition == carPenels.Length-1)
    {
      right.interactable = false;
    }
  }

  public void moveLeft()
  {
  
    carPenels[lastPosition].SetActive(false);
    lastPosition--;
    carPenels[lastPosition].SetActive(true);
    if (lastPosition == 0)
    {
      left.interactable = false;
    }
    if (lastPosition != carPenels.Length-1)
    {
      right.interactable = true;
    }
  }

  // Update is called once per frame
  void Update () {
	}

  public void selectCar(Sprite sprite)
  {
    SpriteRenderer sr = main.GetComponent<SpriteRenderer>();
    sr.sprite = sprite;
  }
}
