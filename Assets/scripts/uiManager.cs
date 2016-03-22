using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;
using System.Net;
using SimpleJSON;
using System.Collections.Generic;

public class uiManager : MonoBehaviour
{
  
  public Text scoreText;
  public int score;
  public bool gameOver;
  public Button[] buttons;
  public carSpawner carSpawer;
  public GameObject panel;
  public Text names;
  public Text scores;
  public InputField recordName;
  public Button pauseButton;
  public audioManager am;
  public GameObject panelRecord;
  Config config;
  public GameObject[] flags;
  private bool pontuating = true;
  public Button soundOn;
  public Button soundoff;
  public Button marginRight;
  public Button marginLeft;
  public GameObject main;
  private const string AD_UNITY_ID = "ca-app-pub-3127012185949555/2849453421";
  private AdMobPlugin admob;


  public void setPontuating(bool value)
  {
    pontuating = value;
  }

  public void goToRecords()
  {
    SceneManager.LoadScene(4);
  }

  public void goToContact()
  {
    SceneManager.LoadScene(5);
  }


  public void closePanel()
  {
    panel.SetActive(false);
    openMenu();
  }

  public void stopSoud()
  {
    PlayerPrefs.SetInt("sound", -1);
    setSoud();
  }

  public void startSoud()
  {
    PlayerPrefs.SetInt("sound", 1);
    setSoud();
  }

  void setSoud()
  {
    int sound = PlayerPrefs.GetInt("sound");
    if(sound >=0 )
    {
      soundOn.gameObject.SetActive(false);
      soundoff.gameObject.SetActive(true);
      am.carSound.Play();
    }
    else
    {
      soundOn.gameObject.SetActive(true);
      soundoff.gameObject.SetActive(false);
      am.carSound.Stop();
    }
  }


  // Use this for initialization
  void Start()
  {
   
    admob = GetComponent<AdMobPlugin>();
    admob.CreateBanner(AD_UNITY_ID, AdMobPlugin.AdSize.SMART_BANNER, false);
    admob.RequestAd();
    admob.ShowBanner();
    if (main != null)
    {
      Sprite sc = Resources.Load<Sprite>("Sprites/" + PlayerPrefs.GetString("sprite"));
      SpriteRenderer sr = main.GetComponent<SpriteRenderer>();
      sr.sprite = sc;
      score = 0;
      gameOver = false;
      config = new Config();
      InvokeRepeating("scoreUpdate", 0.5f, 0.5f);
      setSoud();

    }
  }

  void scoreUpdate()
  {
    if (!gameOver && pontuating)
    {
      score++;
      scoreText.text = "Score: " + score;
    }
  }

  void openMenu()
  {
    foreach (Button button in buttons)
    {
      button.gameObject.SetActive(true);
    }
  }

  void closeMenu()
  {
    foreach (Button button in buttons)
    {
      button.gameObject.SetActive(false);
    }
  }



  public void gameOverActivated()
  {
    gameOver = true;
    panelRecord.SetActive(true);
    pauseButton.gameObject.SetActive(false);
    panel.SetActive(true);
    soundOn.enabled = false;
    soundoff.enabled = false;
    retrieveScore();
    admob.ShowBanner();
   
  }

  public void Play()
  {
    Time.timeScale = 1;
    SceneManager.LoadScene(3);
  }

  public void Instructions()
  {
    Time.timeScale = 1;
    SceneManager.LoadScene(2);
  }



  public void Pause()
  {
    if (Time.timeScale == 1)
    {
      am.carSound.Stop();
      openMenu();
      Time.timeScale = 0;
    }
    else if (Time.timeScale == 0)
    {
      setSoud();
      closeMenu();
      Time.timeScale = 1;
    }
  }


  public void Exit()
  {
    if (admob != null)
    {
      admob.HideBanner();
    }
    Application.Quit();
  }

  public void Replay()
  {
    admob.HideBanner();
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    Time.timeScale = 1;
    foreach (Button button in buttons)
    {
      button.gameObject.SetActive(false);
    }
    panel.SetActive(false);
    pauseButton.gameObject.SetActive(true);
    setSoud();
  }

  public void Menu()
  {
    if (admob != null)
    {
      admob.HideBanner();
    }
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
      names.text = "";
      scores.text = "";
      foreach (JSONNode item in jsonResult.AsArray)
      {
        names.text += item["country_code"] + " - " + item["name"] + "\n";
        scores.text += item["score"] + "\n";
      }
    }
  }



  IEnumerator save(WWW www)
  {
    yield return www;
    // and check for errors
    if (www.error == null)
    {
      retrieveScore();
    }
    else {

    }


  }

  public string Md5Sum(string strToEncrypt)
  {
    System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
    byte[] bytes = ue.GetBytes(strToEncrypt);

    // encrypt bytes
    System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
    byte[] hashBytes = md5.ComputeHash(bytes);

    // Convert the encrypted bytes back to a string (base 16)
    string hashString = "";

    for (int i = 0; i < hashBytes.Length; i++)
    {
      hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
    }
    return hashString.PadLeft(32, '0');
  }

  public void saveScore()
  {

    var url = "http://bestdriver-moraes001.rhcloud.com/user";
    var form = new WWWForm();
    Dictionary<string, string> headers = form.headers;
    var hashValue = Md5Sum(recordName.text + score + config.getKey());
    headers.Add("hashvalue", hashValue);
    form.AddField("name", recordName.text);
    form.AddField("score", score);
    byte[] rawData = form.data;
    var www = new WWW(url, rawData, headers);


    // wait for request to complete
    StartCoroutine(save(www));
    recordName.text = "";
    panelRecord.SetActive(false);
  }

}
