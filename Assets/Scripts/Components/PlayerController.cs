﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
    public enum PlayerTypes
    {
        Cop,
        Mafioso
    }

    [SerializeField]
    public PlayerTypes PlayerType;

    [SerializeField]
    private float movementspeed = 100.0f;
     
    [SerializeField]
	private string verticalAxisInputKeyboard;

    [SerializeField]
	private string horizontalAxisInputKeyboard;

    [SerializeField]
    private string verticalAxisInputController1;

    [SerializeField]
    private string horizontalAxisInputController1;

    [SerializeField]
    private string verticalAxisInputController2;

    [SerializeField]
    private string horizontalAxisInputController2;

    [SerializeField]
    private string prefKeyCop;

    [SerializeField]
    private string prefKeyMafioso;

    [SerializeField]
    private char prefValueDelimiter;

    [SerializeField]
    private Animator animator;

    private bool isControledKeyboard = false;
    private bool isControledController1 = false;
    private bool isControledController2 = false;

    public bool IsControledKeyboard { get { return this.isControledKeyboard; } }
    public bool IsControledController1 { get { return this.isControledController1; } }
    public bool IsControledController2 { get { return this.isControledController2; } }

    public Vector3 CurrentMovementVector;
    
    public int Score;
    public bool WantToCollect;

    public bool WantToTaser;
    public bool WantToScan;
    public bool WantToArrest;
    public bool WantToSensor;
    public bool SensorVisible;

    public bool WasTasered;
    public float ActionTimer;
    public float ActionCooldown;
    public GameObject ArrowOverHead;
    public bool HasSpeedup;

    private GameSystem gameSystem;

    public void Start ()
    {
        switch (this.PlayerType)
        {
            case PlayerTypes.Cop:
                this.loadControlSettings(this.prefKeyCop);
                break;
            case PlayerTypes.Mafioso:
                this.loadControlSettings(this.prefKeyMafioso);
                this.GetComponentInChildren<Animator>().runtimeAnimatorController = FindObjectOfType<AISpawner>().GetRandomAnimatorController();
                break;
        }
        gameSystem = FindObjectOfType<GameSystem>();
        
        Vector3 startPos = new Vector3(Random.Range(-255, 255), 0, Random.Range(-130, 130));
        NavMeshHit hit;
        NavMesh.SamplePosition(startPos, out hit, 50, 1);
        transform.position = hit.position;
        if (ArrowOverHead)
        {
            ArrowOverHead.SetActive(false);
        }
        this.animator.SetInteger("WalkDirection", 0);
	}
	
    public void PostUpdate()
    {
        WantToCollect = false;
    }
    
	public void Update ()
    {
        if (this.PlayerType == PlayerTypes.Cop && gameSystem.CurrentGameState == GameState.WaitingToStart)
        {
            return;
        }  

        if (WasTasered)
        {
            ActionTimer -= Time.deltaTime;
            if (ActionTimer <= 0f)
            {
                WasTasered = false;
            }
            return;
        }

        float activespeed = this.movementspeed;
        if (HasSpeedup)
        {
            ActionTimer -= Time.deltaTime;
            if (ActionTimer <= 0f)
            {
                HasSpeedup = false;
            }
            activespeed = 69f;
        }

        float movementHorizontal = 0.0f;
        float movementVertical = 0.0f;

        if (this.isControledKeyboard)
        {
            movementHorizontal += Input.GetAxis(this.horizontalAxisInputKeyboard);
            movementVertical += Input.GetAxis(this.verticalAxisInputKeyboard);
        }

        if (this.isControledController1)
        {
            movementHorizontal += Input.GetAxis(this.horizontalAxisInputController1);
            movementVertical += Input.GetAxis(this.verticalAxisInputController1);
        }

        if (this.isControledController2)
        {
            movementHorizontal += Input.GetAxis(this.horizontalAxisInputController2);
            movementVertical += Input.GetAxis(this.verticalAxisInputController2);
        }

        Vector3 movement = new Vector3 (movementHorizontal, 0.0f,movementVertical);

        this.transform.Translate ((movement * activespeed * Time.deltaTime));

        this.UpdateWalkAnimation(movement);
    }

    private void loadControlSettings(string prefKey)
    {
        string value = PlayerPrefs.GetString(prefKey, "");
        string[] parts;

        if(value.Trim() != "")
        {
            parts = value.Split(this.prefValueDelimiter);

            for(int i = 0; i < parts.Length; i++)
            {
                switch (parts[i])
                {
                    case "Keyboard":
                        this.isControledKeyboard = true;
                        break;
                    case "Controller1":
                        this.isControledController1 = true;
                        break;
                    case "Controller2":
                        this.isControledController2 = true;
                        break;
                }
            }
        }
    }

    private void UpdateWalkAnimation(Vector3 movementVector)
    {
        if (movementVector == Vector3.zero)
        {
            this.animator.SetInteger("WalkDirection", 0);
        }
        else
        {
            Vector3 temp = new Vector3(movementVector.x, -movementVector.z, movementVector.y); //Adjust the numbers to the correct world space values again
            
            CurrentMovementVector = temp;

            float angle = Vector3.Angle(Camera.main.transform.forward, temp);

            if (temp.x < 0)
            {
                angle = 360 - angle;
            }

            if (angle > 315 || (angle >= 0 && angle <= 45))
            {
                this.animator.SetInteger("WalkDirection", 1);
            }
            else if (angle > 45 && angle <= 135)
            {
                this.animator.SetInteger("WalkDirection", 3);
            }
            else if (angle > 135 && angle <= 225)
            {
                this.animator.SetInteger("WalkDirection", 5);
            }
            else if (angle > 225 && angle <= 315)
            {
                this.animator.SetInteger("WalkDirection", 7);
            }
        }
    }
		
}


