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
    public float startupTimer;
    private bool timerRunnin = true;

    public float ShopUpdateTimer = 10f;

    public GameState CurrentGameState;

    public Text TimeLabel;

    private List<ShopHotspotComponent> shops;
    private MoneySafeComponent safe;

    public PlayerController Mafioso;

    [SerializeField]
    private GameObject startupObjects;

    [SerializeField]
    private GameObject startupArrow;

    private bool startupPhase1 = true;
    private bool startupPhase2 = true;
    private bool startupPhase3 = true;

    bool firstStart;

    // Use this for initialization
    void Start()
    {
        CurrentGameState = GameState.WaitingToStart;
        this.startupTimer = 0.0f;
        this.startupObjects.SetActive(false);
        this.startupArrow.SetActive(false);
        shops = new List<ShopHotspotComponent>();
        shops.AddRange(FindObjectsOfType<ShopHotspotComponent>());
        safe = FindObjectOfType<MoneySafeComponent>();
        firstStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (firstStart)
        {
            FindObjectOfType<AudioSystem>().PlaySound("Atmo_Loop");
        }

        switch (CurrentGameState)
        {
            case GameState.WaitingToStart:
                // Countdown to start ? / Show "START" label ?
                this.CountDownStartTime();
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
        ProcessCops();
    }

    void UpdateShops()
    {
        foreach (ShopHotspotComponent shop in shops)
        {
            //Debug.DrawRay(shop.transform.position, Vector3.up * 250, shop.WasRobbed ? Color.red : Color.green);
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
                    shop.CurrentValue += (int)UnityEngine.Random.Range(shop.ValueIncreasePerUpdate * 0.25f, shop.ValueIncreasePerUpdate * 1.25f);
                }
            }

            if (Mafioso.WantToCollect && shop.CurrentValue > 0)
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
                        Mafioso.WantToCollect = false;
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
        if (Mafioso.WantToCollect)
        {
            if (Vector3.Distance(safe.transform.position, Mafioso.gameObject.transform.position) < 15)
            {
                Vector3 playerPos = Mafioso.gameObject.transform.position;
                Vector3 lookDir = safe.transform.position - playerPos;
                lookDir.Normalize();
                playerPos += (lookDir * 2);

                RaycastHit hitinfo;
                if (Physics.Linecast(safe.transform.position, playerPos, out hitinfo))
                {
                    //Debug.Log("Blocked! by " + hitinfo.transform.gameObject.name);
                }
                else
                {
                    Mafioso.WantToCollect = false;
                    safe.SavedMoney += Mafioso.Score;
                    Mafioso.Score = 0;
                }
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

    private void CountDownStartTime()
    {
        

        if(this.startupTimer == 0.0f)
        {
            this.startupObjects.SetActive(true);
        }

        this.startupTimer += Time.deltaTime;

        if (this.startupPhase1)
        {
            if (this.startupTimer > 4.0f)
            {
                if (!this.startupArrow.activeInHierarchy)
                {
                    this.startupArrow.SetActive(true);
                }
                else if (this.startupTimer > 5.8f)
                {
                    this.startupArrow.SetActive(false);
                    this.startupPhase1 = false;
                }
            }
        }

        if (this.startupPhase2)
        {
            if (this.startupTimer > 6.0f)
            {
                this.startupPhase2 = false;
                FindObjectOfType<AudioSystem>().PlaySound("RobotCountdown");
            }
        }

        if (this.startupPhase3)
        {
            if (this.startupTimer > 11.0f)
            {
                this.startupPhase3 = false;
                FindObjectOfType<AudioSystem>().PlaySound("InGame_PoliceSiren");
                this.SwitchGameState(GameState.Gamplay);
            }
        }


    }

    private void CountDownGameTime()
    {
        if (timerRunnin && roundTime > 0.0f)
        {
            roundTime -= Time.deltaTime;
            //TimeSpan st = TimeSpan.FromSeconds(roundTime);
            //TimeLabel.text = "Time Left : " + string.Format("{0:00}:{1:00}", st.Minutes, st.Seconds);
        }
        else
        {
            timerRunnin = false;
            Debug.Log("Game Over");
            CurrentGameState = GameState.GameOver;
        }
    }

    public GameObject StunEffectPrefab;
    public GameObject BulletPrefab;

    private void ProcessCops()
    {
        // - move
        List<PlayerController> cops = new List<PlayerController>();
        foreach (PlayerController cop in FindObjectsOfType<PlayerController>())
        {
            if (cop.PlayerType == PlayerController.PlayerTypes.Cop)
            {
                cops.Add(cop);
            }
        }
        

        //--

        foreach (PlayerController cop in cops)
        {
            if (cop.WantToTaser)
            {
                cop.WantToTaser = false;
                GameObject target = cop.GetComponent<PlayerActionsComponent>().CurrentHighlightedTarget;
                if (target != null)
                {
                    AIEntity aiTarget = target.GetComponent<AIEntity>();
                    if (aiTarget != null)
                    {
                        cop.Score -= 200;
                        aiTarget.CurrentSate = AIStates.Stunned;
                        aiTarget.ActionTimer = aiTarget.MaxIdleTime;
                        GameObject bullet = Instantiate(BulletPrefab, aiTarget.transform.position + (Vector3.forward * 6) , Quaternion.identity);
                        bullet.GetComponent<BulletComponent>().Direction = cop.CurrentMovementVector.normalized;
                    }
                }
            }
        }
    }


}
