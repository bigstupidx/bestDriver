using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;
using System.Net;
using SimpleJSON;


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

    public void closePanel()
    {
        panel.SetActive(false);
        openMenu();
    }


    // Use this for initialization
    void Start () {
        score = 0;
        gameOver = false;
   
        InvokeRepeating("scoreUpdate", 1.0f, 0.5f);
       

    }
	
	// Update is called once per frame
	void Update () {
        if(scoreText != null)
        scoreText.text = "Score: " + score;
    }

    void scoreUpdate()
    {
        if (!gameOver)
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
        var request = (HttpWebRequest)WebRequest.Create(new Uri("http://default-environment-5zhficjk2s.elasticbeanstalk.com/users/limit/10"));
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
                names.text += item["name"] + "\n";
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
            Debug.Log("WWW Error: " + www.error);
        }

       
    }

    public void saveScore()
    {

        Debug.Log("saving");
        var url = "http://default-environment-5zhficjk2s.elasticbeanstalk.com/user";
        var form = new WWWForm();
        form.AddField("name", recordName.text);
        //string
        form.AddField("score", score);
        form.AddField("ip", GetIP());

        var www = new WWW(url, form);


        // wait for request to complete
        StartCoroutine(save(www));
        recordName.text = "";
        panelRecord.SetActive(false);
    }


    private string GetIP()
    {
        string strHostName = "";
        strHostName = System.Net.Dns.GetHostName();

        IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

        IPAddress[] addr = ipEntry.AddressList;

        return addr[addr.Length - 1].ToString();

    }



}
