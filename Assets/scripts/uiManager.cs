using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;
using System.Net;
using SimpleJSON;
using System.Collections.Generic;
using UnityEngine.Advertisements;
using UnityEngine.Analytics;


public class uiManager : MonoBehaviour {

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


    public void setPontuating(bool value)
    {
        pontuating = value;
    }

    public void closePanel()
    {
        panel.SetActive(false);
        openMenu();
    }


    // Use this for initialization
    void Start () {
        score = 0;
        gameOver = false;
        config = new Config();
        InvokeRepeating("scoreUpdate", 1.0f, 0.5f);
       

    }
	
	// Update is called once per frame
	void Update () {
        if(scoreText != null)
        scoreText.text = "Score: " + score;
    }

    void scoreUpdate()
    {
        if (!gameOver && pontuating)
        {
            score++;
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
        int totalPotions = 5;
        int totalCoins = 100;
        string weaponID = "Weapon_102";
        Analytics.CustomEvent("gameOver", new Dictionary<string, object>
          {
            { "potions", totalPotions },
            { "coins", totalCoins },
            { "activeWeapon", weaponID }
          });
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
        gameOver = true;
        panelRecord.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        panel.SetActive(true);
        retrieveScore();
    }

    public void Play()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void Instructions()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }



    public void Pause() {
        if(Time.timeScale ==1)
        {
            am.carSound.Stop();
            openMenu();
            Time.timeScale = 0;
        }else if (Time.timeScale == 0)
        {
            am.carSound.Play();
            closeMenu();
            Time.timeScale = 1;
        }
    }


    public void Exit()
    {
        Application.Quit();
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        panel.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    void retrieveScore()
    {
        Debug.Log("retrieveScore");
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
                //Instantiate(flags[Random.Range(0, 5)], carPos, transform.rotation);
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
            Debug.Log("WWW Error: " + www.error);
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
        Debug.Log(hashString.PadLeft(32, '0'));
        return hashString.PadLeft(32, '0');
    }

    public void saveScore()
    {

        Debug.Log("saving");
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
