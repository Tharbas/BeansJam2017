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

    [SerializeField]
    private GameObject ScoreScreen;

    [SerializeField]
    private Text textScoreRound1Player1;

    [SerializeField]
    private Text textScoreRound1Player2;

    [SerializeField]
    private Text textScoreRound2Player1;

    [SerializeField]
    private Text textScoreRound2Player2;

    [SerializeField]
    private Text textScoreTotalPlayer1;

    [SerializeField]
    private Text textScoreTotalPlayer2;

    [SerializeField]
    private Text textSwitchGamepads;

    [SerializeField]
    private Text textThankYouForPlaying;

    [SerializeField]
    private GameObject buttonStartNextRound;

    [SerializeField]
    private Image copSensorCooldown;

    [SerializeField]
    private GameSystem gameSystem;

    private bool isBlinking = false;
    private int scorePlayer1 = 0;
    private int scorePlayer2 = 0;

    public bool IsScoreScreenOpen { get { return this.ScoreScreen.activeInHierarchy; } }

    void Start()
	{
        this.pauseMenu.SetActive(false);
        this.ScoreScreen.SetActive(false);
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

    public void ReportCopSensorCooldown(float amount)
    {
        this.copSensorCooldown.fillAmount = amount;
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

    public void OnOpenScoreScreen(int scoreCop, int scoreMafioso, int round)
    {
        switch (round)
        {
            case 1:
                this.ScoreScreen.SetActive(true);

                this.scorePlayer1 += scoreCop;
                this.scorePlayer2 += scoreMafioso;

                this.textScoreRound1Player1.text = this.scorePlayer1.ToString();
                this.textScoreRound1Player2.text = this.scorePlayer2.ToString();

                this.textScoreRound2Player1.text = "-";
                this.textScoreRound2Player2.text = "-";

                this.textScoreTotalPlayer1.text = this.scorePlayer1.ToString();
                this.textScoreTotalPlayer2.text = this.scorePlayer2.ToString();

                this.textSwitchGamepads.gameObject.SetActive(true);
                this.textThankYouForPlaying.gameObject.SetActive(false);
                this.buttonStartNextRound.SetActive(true);

                break;
            case 2:
                this.ScoreScreen.SetActive(true);

                this.scorePlayer1 += scoreMafioso;
                this.scorePlayer2 += scoreCop;

                this.textScoreRound2Player1.text = scoreMafioso.ToString();
                this.textScoreRound2Player2.text = scoreCop.ToString();

                this.textScoreTotalPlayer1.text = this.scorePlayer1.ToString();
                this.textScoreTotalPlayer2.text = this.scorePlayer2.ToString();

                this.textSwitchGamepads.gameObject.SetActive(false);
                this.textThankYouForPlaying.gameObject.SetActive(true);
                this.buttonStartNextRound.SetActive(false);

                break;
        }
    }

    public void OnClickScoreScreenQuitGame()
    {
        this.ScoreScreen.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }

    public void OnClickScoreScreenReady()
    {
        this.ScoreScreen.SetActive(false);
        this.gameSystem.StartSecondRound();
    }

}
