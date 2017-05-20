using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    WaitingToStart,
    Gamplay,
    GameOver
}

public class GameSystem : MonoBehaviour
{
    public float roundTime;
    public float MaxRoundTime;
    private bool timerRunnin = true;

    public GameState CurrentGameState;

    public Text TimeLabel;

    // Use this for initialization
    void Start()
    {
        CurrentGameState = GameState.WaitingToStart;
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentGameState)
        {
            case GameState.WaitingToStart:
                // Countdown to start ? / Show "START" label ?
                SwitchGameState(GameState.Gamplay);
                break;
            case GameState.Gamplay:
                CountDownGameTime();
                break;
            case GameState.GameOver:
                if (Input.anyKeyDown)
                {
                    SceneManager.LoadScene("MainMenu");
                }
                break;
        }
        
    }

    void SwitchGameState(GameState newState)
    {
        switch (CurrentGameState)
        {
            case GameState.Gamplay:
                // Hide start, show hud, stuff
                roundTime = MaxRoundTime;
                timerRunnin = true;
                break;
            case GameState.GameOver:
                // Show gameover screen
                break;
        }
        CurrentGameState = newState;
    }



    private void CountDownGameTime()
    {
        if (timerRunnin && roundTime > 0.0f)
        {
            roundTime -= Time.deltaTime;
            TimeSpan st = TimeSpan.FromSeconds(roundTime);
            TimeLabel.text = "Time Left : " + string.Format("{0:00}:{1:00}", st.Minutes, st.Seconds);
        }
        else
        {
            timerRunnin = false;
            Debug.Log("Game Over");
            CurrentGameState = GameState.GameOver;
        }
    }


}
