using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameGuiController : MonoBehaviour {

    [SerializeField]
    private float startTimeBlink;

    [SerializeField]
    private Text textTimeLeft;

    [SerializeField]
    private Text textTimeCount;

    [SerializeField]
    private Text textMoneyStashed;

    [SerializeField]
    private Text textMoneyCarried;

    [SerializeField]
    private GameObject pauseMenu;

    private bool isBlinking = false;

    void Start()
	{
        this.pauseMenu.SetActive(false);
	}

    public void SetTimeLeft(TimeSpan span)
    {
        this.textTimeCount.text =  string.Format("{0:00}:{1:00}", span.Minutes, span.Seconds);

        if(!this.isBlinking && span.TotalSeconds <= this.startTimeBlink)
        {
            this.isBlinking = true;
            this.textTimeLeft.gameObject.GetComponent<Animator>().SetTrigger("StartBlink");
            this.textTimeCount.gameObject.GetComponent<Animator>().SetTrigger("StartBlink");
        }
    }

    public void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape) || Input.GetButtonUp("PauseGame"))
        {
            if (this.pauseMenu.activeInHierarchy)
            {
                this.OnClickResumeGame();
            }
            else
            {
                this.OnOpenPauseMenu();
            }
        }
    }

    public void ReportMoneyStashed(int newAmount)
    {
        this.textMoneyStashed.text = newAmount.ToString() + " $";
    }

    public void ReportMoneyCollected(int newAmount)
    {
        this.textMoneyCarried.text = newAmount.ToString() + " $";
    }

    public void OnClickQuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnClickResumeGame()
    {
        this.pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnOpenPauseMenu()
    {
        this.pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

}
