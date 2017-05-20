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

    public float ShopUpdateTimer = 10f;

    public GameState CurrentGameState;

    public Text TimeLabel;

    private List<ShopHotspotComponent> shops;

    public PlayerController Mafioso;

    // Use this for initialization
    void Start()
    {
        CurrentGameState = GameState.WaitingToStart;
        shops = new List<ShopHotspotComponent>();
        shops.AddRange(FindObjectsOfType<ShopHotspotComponent>());
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
        UpdateShops();
    }

    void UpdateShops()
    {
        foreach (ShopHotspotComponent shop in shops)
        {
            Debug.DrawRay(shop.transform.position, Vector3.up * 250, shop.WasRobbed ? Color.red : Color.green);
            if (shop.UpdateTimer > 0f)
            {
                shop.UpdateTimer -= Time.deltaTime;
            }
            else if (shop.UpdateTimer <= 0)
            {
                shop.UpdateTimer = UnityEngine.Random.Range(ShopUpdateTimer * 0.33f, ShopUpdateTimer * 1.25f);
                if (shop.WasRobbed)
                {
                    // ALERT !!
                    Mafioso.Score += shop.CurrentValue;
                    shop.CurrentValue = 0;
                    shop.WasRobbed = false;
                }
                else
                {
                    shop.CurrentValue += shop.ValueIncreasePerUpdate;
                }
            }

            if (shop.CurrentValue > 0)
            {
                if (Vector3.Distance(shop.transform.position, Mafioso.gameObject.transform.position) < 15)                    
                {
                    Vector3 playerPos = Mafioso.gameObject.transform.position;
                    Vector3 lookDir = shop.transform.position - playerPos;
                    lookDir.Normalize();
                    playerPos += (lookDir * 2);

                    RaycastHit hitinfo;
                    if (Physics.Linecast(shop.transform.position, playerPos, out hitinfo))
                    {
                        //Debug.Log("Blocked! by " + hitinfo.transform.gameObject.name);
                    }
                    else
                    {
                        shop.WasRobbed = true;
                        shop.UpdateTimer = ShopUpdateTimer * 0.25f;
                    }
                }
            }
            TextMesh text = shop.GetComponentInChildren<TextMesh>();
            if (text)
            { 
                text.text = shop.CurrentValue + " $";
            }
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
