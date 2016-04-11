using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Net;
using SimpleJSON;
using UnityEngine.SceneManagement;

public class uiRecords : MonoBehaviour {

  public GameObject[] flags;
  public Text[] names;
  public Text[] scores;
  public Text bt;

  // Use this for initialization
  void Start() {
    retrieveScore();

  }

  void Update()
  {
    bt.text = Screen.width + " - " + Screen.height;

  }
  public void returnToMain()
  {
    SceneManager.LoadScene(0);
  }

  void retrieveScore()
  {
    var request = (HttpWebRequest)WebRequest.Create(new Uri("http://bestdriver-moraes001.rhcloud.com/users/limit/10"));
    request.ContentType = "application/json";
    request.Method = "GET";

    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    string jsonResponse = string.Empty;
    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
    {
      jsonResponse = sr.ReadToEnd();
    }
    var jsonResult = JSON.Parse(jsonResponse);
    if (jsonResult.Count != 0)
    {
      int pos = 0; 
      foreach (JSONNode item in jsonResult.AsArray)
      {
        names[pos].text = item["name"];
        scores[pos].text = item["score"] + " pts";
        Sprite sc = Resources.Load<Sprite>("Sprites/" + item["country_code"]);
        SpriteRenderer sr = flags[pos].GetComponent<SpriteRenderer>();
        sr.sprite = sc;
        pos++;
      }
    }
  }
}
