using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
    public float OriginalRoundTime;
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

    private List<GameObject> bullets;
    private List<AIEntity> npcs;
    public GameObject ColliderRoot;
    private List<BoxCollider> walls;
    List<PlayerController> cops;
    public GameObject StunEffectPrefab;
    public GameObject ScanEffectPrefab;
    public GameObject BulletPrefab;

    private AudioSystem audioSystem;

    private List<GameObject> activeEffects;

    [SerializeField]
    private GameGuiController guiController;

    private bool startupPhase1 = true;
    private bool startupPhase2 = true;
    private bool startupPhase3 = true;

    public int MaxTaserBullets = 1;
    bool firstStart;
    public int TaserNpcPunishScore = 100;
    public float MafiosoTaserTime = 2.5f;
    public float CopSensorCooldown = 10f;
    public int WrongArrestPunishScore = 500;

    
    private int currentRound = 1;

    // Use this for initialization
    void Start()
    {
        CurrentGameState = GameState.WaitingToStart;
        this.startupTimer = 0.0f;
        this.startupObjects.SetActive(false);
        this.startupArrow.SetActive(false);
        if (this.guiController)
        {
            TimeSpan st = TimeSpan.FromSeconds(roundTime);
            this.guiController.SetTimeLeft(st);
        }
        activeEffects = new List<GameObject>();
        shops = new List<ShopHotspotComponent>();
        shops.AddRange(FindObjectsOfType<ShopHotspotComponent>());
        safe = FindObjectOfType<MoneySafeComponent>();
        npcs = new List<AIEntity>();
        walls = new List<BoxCollider>();
        if (ColliderRoot != null)
        {
            walls.AddRange(ColliderRoot.GetComponentsInChildren<BoxCollider>());
        }
        audioSystem = FindObjectOfType<AudioSystem>();
        bullets = new List<GameObject>();
        firstStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (firstStart)
        {
            audioSystem.PlaySound("Atmo_Loop");
            cops = new List<PlayerController>();
            foreach (PlayerController cop in FindObjectsOfType<PlayerController>())
            {
                if (cop.PlayerType == PlayerController.PlayerTypes.Cop)
                {
                    cops.Add(cop);
                }
            }

            this.OriginalRoundTime = this.roundTime;
            firstStart = false;
        }
        if (npcs.Count == 0)
        {
            npcs.AddRange(FindObjectsOfType<AIEntity>());
        }

        switch (CurrentGameState)
        {
            case GameState.WaitingToStart:
                this.CountDownStartTime();
                break;
            case GameState.Gamplay:
                CountDownGameTime();
                break;
            case GameState.GameOver:
                if (!this.guiController.IsScoreScreenOpen)
                {
                    this.guiController.OnOpenScoreScreen(cops[0].Score, Mafioso.Score + safe.SavedMoney, this.currentRound);
                }
                break;
        }
        UpdateShops();
        ProcessCops();
        ProcessBullets();
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
                if (Vector3.Distance(shop.transform.position, Mafioso.gameObject.transform.position) < 30)
                {
                    Vector3 playerPos = Mafioso.gameObject.transform.position;
                    Vector3 lookDir = shop.transform.position - playerPos;
                    lookDir.Normalize();
                    playerPos += (lookDir * 2);

                    RaycastHit hitinfo;
                    if (Physics.Linecast(shop.transform.position, playerPos, out hitinfo, 1 << 8))
                    {
                        Debug.Log("Blocked! by " + hitinfo.transform.gameObject.name);
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
                    safe.StashMoney(Mafioso.Score, Mafioso);
                }
            }
        }
        guiController.ReportMoneyCollected(Mafioso.Score);
        guiController.ReportMoneyStashed(safe.SavedMoney);
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


        if (this.startupTimer == 0.0f)
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
            TimeSpan st = TimeSpan.FromSeconds(roundTime);
            this.guiController.SetTimeLeft(st);
        }
        else
        {
            timerRunnin = false;
            Debug.Log("Game Over");
            CurrentGameState = GameState.GameOver;
        }
    }

    private void ProcessBullets()
    {
        List<GameObject> removeMe = new List<GameObject>();
        foreach (GameObject bullet in bullets)
        {
            BulletComponent bulletC = bullet.GetComponent<BulletComponent>();
            bullet.transform.Translate(bulletC.Direction * bulletC.Speed);

            bool hit = false;
            foreach (BoxCollider collider in walls)
            {
                Bounds bound = new Bounds(collider.bounds.center, collider.bounds.extents);
                bound.Expand(10);
                if (bound.Contains(bullet.transform.position))
                {
                    hit = true;
                }
            }
            if (!hit)
            {
                if (Vector3.Distance(Mafioso.transform.position, bullet.transform.position) < 10)
                {
                    hit = true;
                    Mafioso.WasTasered = true;
                    Mafioso.ActionTimer = MafiosoTaserTime;
                    activeEffects.Add(Instantiate(StunEffectPrefab, Mafioso.transform.position + new Vector3(0, 0, 10), Quaternion.identity));
                }
            }
            if (!hit)
            {
                foreach (AIEntity npc in npcs)
                {
                    if (Vector3.Distance(npc.transform.position, bullet.transform.position) < 10)
                    {
                        hit = true;
                        bulletC.Owner.Score -= TaserNpcPunishScore;
                        npc.CurrentSate = AIStates.Stunned;
                        npc.ActionTimer = npc.MaxIdleTime;
                        activeEffects.Add(Instantiate(StunEffectPrefab, npc.transform.position + new Vector3(0, 0, 10), Quaternion.identity));
                        break;
                    }
                }
            }

            if ((Mathf.Abs(bullet.transform.position.x) > 500 ||
                 Mathf.Abs(bullet.transform.position.z) > 300))
            {
                hit = true;
            }
            if (hit)
            {
                removeMe.Add(bullet);
            }
        }
        foreach (GameObject del in removeMe)
        {
            bullets.Remove(del);
            GameObject.Destroy(del);
        }
    }

    private void ProcessCops()
    {
        foreach (PlayerController cop in cops)
        {
            if (cop.WantToTaser)
            {
                cop.WantToTaser = false;
                if (bullets.Count < MaxTaserBullets)
                {
                    GameObject bullet = Instantiate(BulletPrefab, cop.transform.position + (Vector3.forward * 2), new Quaternion(1, 0, 0, 1));
                    Vector3 dir = cop.CurrentMovementVector.normalized;
                    dir.y = -dir.y;
                    BulletComponent bc = bullet.GetComponent<BulletComponent>();
                    bc.Direction = dir;
                    bc.Owner = cop;
                    audioSystem.PlaySound("PlasmaShot_0" + UnityEngine.Random.Range(1, 5));
                    bullets.Add(bullet);
                }
            }
            if (cop.WantToScan)
            {
                cop.WantToScan = false;
                GameObject target = cop.GetComponent<PlayerActionsComponent>().CurrentHighlightedTarget;
                if (target != null)
                {
                    AIEntity targetNpc = target.GetComponent<AIEntity>();
                    if(targetNpc != null)
                    {
                        audioSystem.PlaySound("Scanner");
                        activeEffects.Add(Instantiate(ScanEffectPrefab, target.transform.position + new Vector3(0, 0, 10), Quaternion.identity));
                        targetNpc.IsBeingScanned();
                    }
                    else
                    {
                        PlayerController targetPlayer = target.GetComponent<PlayerController>();
                        if (targetPlayer != null && targetPlayer.PlayerType == PlayerController.PlayerTypes.Mafioso)
                        {
                            targetPlayer.WasTasered = true;
                            targetPlayer.ActionTimer = 2f; // sfx length
                            audioSystem.PlaySound("Scanner");
                            activeEffects.Add(Instantiate(ScanEffectPrefab, target.transform.position + new Vector3(0, 0, 10), Quaternion.identity));
                        }
                    }
                }
            }
            if (cop.WantToSensor)
            {
                cop.WantToSensor = false;
                if (cop.SensorVisible == false && cop.ActionCooldown <= 0f)
                {
                    cop.ActionTimer = 3f;
                    cop.SensorVisible = true;
                    audioSystem.PlaySound("DroneActivated");
                    cop.ArrowOverHead.SetActive(true);
                }
            }
            if (cop.WantToArrest)
            {
                cop.WantToArrest = false;
                GameObject target = cop.GetComponent<PlayerActionsComponent>().CurrentHighlightedTarget;
                if (target != null)
                {
                    PlayerController targetPlayer = target.GetComponent<PlayerController>();
                    if (targetPlayer != null && targetPlayer.PlayerType == PlayerController.PlayerTypes.Mafioso)
                    {
                        audioSystem.PlaySound("BodyDeath_0" + UnityEngine.Random.Range(1, 4));
                        cop.Score += targetPlayer.Score;
                        targetPlayer.Score = 0;
                        SwitchGameState(GameState.GameOver);
                    }
                    else
                    {
                        AIEntity targetAI = target.GetComponent<AIEntity>();
                        if (targetAI != null)
                        {
                            cop.Score -= WrongArrestPunishScore;
                            audioSystem.PlaySound("BodyImpact_0" + UnityEngine.Random.Range(1, 7));
                        }
                    }
                }
            }


            if (cop.SensorVisible)
            {
                if (cop.ActionTimer > 0f)
                {
                    cop.ActionTimer -= Time.deltaTime;
                    if (cop.ActionTimer < 0f)
                    {
                        cop.SensorVisible = false;
                        cop.ArrowOverHead.SetActive(false);
                        cop.ActionCooldown = CopSensorCooldown;
                    }
                    else
                    {
                        Vector3 target = new Vector3(Mafioso.transform.position.x, cop.ArrowOverHead.transform.position.y, Mafioso.transform.position.z);
                        cop.ArrowOverHead.transform.LookAt(target, Vector3.up);
                        cop.ArrowOverHead.transform.Rotate(Vector3.left, 90f);
                    }
                }
            }

            if (cop.ActionCooldown > 0f)
            {
                cop.ActionCooldown -= Time.deltaTime;
                guiController.ReportCopSensorCooldown(1f - (cop.ActionCooldown / CopSensorCooldown));
            }

            List<GameObject> deadEfects = new List<GameObject>();
            foreach (GameObject effect in activeEffects)
            {
                if (effect.GetComponentInChildren<ParticleSystem>().IsAlive() == false)
                {
                    deadEfects.Add(effect);
                }
            }
            foreach (GameObject effect in deadEfects)
            {
                activeEffects.Remove(effect);
                GameObject.Destroy(effect);
            }
        }
    }

    public void StartSecondRound()
    {
        this.currentRound++;
        this.startupPhase1 = true;
        this.startupPhase2 = true;
        this.startupPhase3 = true;

        this.startupTimer = 0.0f;

        safe.SavedMoney = 0;

        foreach (GameObject bullet in bullets)
        {
            GameObject.Destroy(bullet);
        }
        bullets.Clear();

        FindObjectOfType<AISystem>().Reset();

        Mafioso.ActionCooldown = 0;
        Mafioso.CurrentMovementVector = Vector3.zero;
        Mafioso.Score = 0;
        Vector3 startPos = new Vector3(UnityEngine.Random.Range(-255, 255), 0, UnityEngine.Random.Range(-130, 130));
        NavMeshHit hit;
        NavMesh.SamplePosition(startPos, out hit, 50, 1);
        Mafioso.transform.position = hit.position;
        Mafioso.GetComponentInChildren<Animator>().runtimeAnimatorController = FindObjectOfType<AISpawner>().GetRandomAnimatorController();
        Mafioso.GetComponent<PlayerActionsComponent>().hasSmokeBomb = true;

        foreach (PlayerController cop in cops)
        {
            cop.ActionCooldown = 0;
            cop.CurrentMovementVector = Vector3.zero;
            cop.Score = 0;
            cop.SensorVisible = false;
            cop.ArrowOverHead.SetActive(false);
            cop.GetComponent<PlayerActionsComponent>().CurrentHighlightedTarget = null;

            startPos = new Vector3(UnityEngine.Random.Range(-255, 255), 0, UnityEngine.Random.Range(-130, 130));
            NavMesh.SamplePosition(startPos, out hit, 50, 1);
            cop.transform.position = hit.position;
        }

        foreach (GameObject effect in activeEffects)
        {
            activeEffects.Remove(effect);
            GameObject.Destroy(effect);
        }

        this.timerRunnin = true;
        this.roundTime = this.OriginalRoundTime;

        this.Start();
    }
}
