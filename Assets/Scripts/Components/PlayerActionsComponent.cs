using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionsComponent : MonoBehaviour
{
    public enum PlayerTypes
    {
        Cop,
        Mafioso
    }

    public enum PossibleActions
    {
        CollectStashMoney,
        DropSmokeBomb,
        ScanPerson,
        ArrestPerson,
        UseSensors,
        Shoot
    }

    [SerializeField]
    private PlayerTypes playerType;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private GameObject shotPrefab;

    [SerializeField]
    private ParticleSystem smokeBombSystem;

    [SerializeField]
    private GameObject sensorArrow;

    [SerializeField]
    private AudioSource audiosource;

    [SerializeField]
    private AudioClip audioClipSmokeBomb;

    [SerializeField]
    private float smokeBombRadius;

    private AISystem aiSystem;

    public GameObject CurrentHighlightedTarget;

    private bool hasSmokeBomb = true;

    public void Start()
    {
        this.aiSystem = FindObjectOfType<AISystem>();
    }

    public void Update()
    {
        if (this.playerController.IsControledKeyboard)
        {
            switch (this.playerType)
            {
                case PlayerTypes.Cop:
                    if (Input.GetKeyUp(KeyCode.Space))
                    {
                        this.DoAction(PossibleActions.Shoot);
                    }

                    if (Input.GetKeyUp(KeyCode.Return))
                    {
                        this.DoAction(PossibleActions.UseSensors);
                    }

                    if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
                    {
                        this.DoAction(PossibleActions.ScanPerson);
                    }

                    if (Input.GetKeyUp(KeyCode.LeftAlt))
                    {
                        this.DoAction(PossibleActions.ArrestPerson);
                    }
                    break;

                case PlayerTypes.Mafioso:
                    if (Input.GetKeyUp(KeyCode.Space))
                    {
                        this.DoAction(PossibleActions.CollectStashMoney);
                    }

                    if (Input.GetKeyUp(KeyCode.Return))
                    {
                        this.DoAction(PossibleActions.DropSmokeBomb);
                    }
                    break;
            }
        }

        if (this.playerController.IsControledController1)
        {
            switch (this.playerType)
            {
                case PlayerTypes.Cop:
                    if (Input.GetButtonUp("CopShootGun_Controller1"))
                    {
                        this.DoAction(PossibleActions.Shoot);
                    }

                    if (Input.GetButtonUp("CopSensorArray_Controller1"))
                    {
                        this.DoAction(PossibleActions.UseSensors);
                    }

                    if (Input.GetButtonUp("CopScanPerson_Controller1"))
                    {
                        this.DoAction(PossibleActions.ScanPerson);
                    }

                    if (Input.GetButtonUp("CopArrestPerson_Controller1"))
                    {
                        this.DoAction(PossibleActions.ArrestPerson);
                    }
                    break;

                case PlayerTypes.Mafioso:
                    if (Input.GetButtonUp("MafCollectMoney_Controller1"))
                    {
                        this.DoAction(PossibleActions.CollectStashMoney);
                    }

                    if (Input.GetButtonUp("MafSmokeBomb_Controller1"))
                    {
                        this.DoAction(PossibleActions.DropSmokeBomb);
                    }
                    break;
            }
        }

        if (this.playerController.IsControledController2)
        {
            switch (this.playerType)
            {
                case PlayerTypes.Cop:
                    if (Input.GetButtonUp("CopShootGun_Controller2"))
                    {
                        this.DoAction(PossibleActions.Shoot);
                    }

                    if (Input.GetButtonUp("CopSensorArray_Controller2"))
                    {
                        this.DoAction(PossibleActions.UseSensors);
                    }

                    if (Input.GetButtonUp("CopScanPerson_Controller2"))
                    {
                        this.DoAction(PossibleActions.ScanPerson);
                    }

                    if (Input.GetButtonUp("CopArrestPerson_Controller2"))
                    {
                        this.DoAction(PossibleActions.ArrestPerson);
                    }
                    break;

                case PlayerTypes.Mafioso:
                    if (Input.GetButtonUp("MafCollectMoney_Controller2"))
                    {
                        this.DoAction(PossibleActions.CollectStashMoney);
                    }

                    if (Input.GetButtonUp("MafSmokeBomb_Controller2"))
                    {
                        this.DoAction(PossibleActions.DropSmokeBomb);
                    }
                    break;
            }
        }

        if (this.playerType == PlayerTypes.Cop)
        {
            HighlightTarget();
        }
    }

    private void HighlightTarget()
    {
        Vector3 lookDir = new Vector3(playerController.CurrentMovementVector.x, playerController.CurrentMovementVector.z, -playerController.CurrentMovementVector.y);
        RaycastHit hitinfo;
        bool hit = Physics.Raycast(playerController.transform.position, lookDir, out hitinfo);

        if (hit && hitinfo.distance < 50)
        {
            CurrentHighlightedTarget = hitinfo.collider.gameObject;

            HighlightComponent highlighter = CurrentHighlightedTarget.GetComponent<HighlightComponent>();
            if (highlighter)
            {
                highlighter.DoHighlight();
            }
        }
        else
        {
            this.CurrentHighlightedTarget = null;
        }
    }

    public void DoAction(PossibleActions action)
    {
        switch (this.playerType)
        {
            case PlayerTypes.Cop:
                switch (action)
                {
                    case PossibleActions.ArrestPerson:
                        playerController.WantToArrest = true;
                        break;
                    case PossibleActions.ScanPerson:
                        playerController.WantToScan = true;
                        break;
                    case PossibleActions.Shoot:
                        playerController.WantToTaser = true;
                        break;
                    case PossibleActions.UseSensors:
                        playerController.WantToSensor = true;
                        break;
                }
                break;
            case PlayerTypes.Mafioso:
                switch (action)
                {
                    case PossibleActions.CollectStashMoney:
                        playerController.WantToCollect = true;
                        break;
                    case PossibleActions.DropSmokeBomb:
                        if (this.hasSmokeBomb)
                        {
                            this.hasSmokeBomb = false;
                            this.audiosource.clip = this.audioClipSmokeBomb;
                            this.audiosource.Play();
                            this.smokeBombSystem.Play();
                            this.aiSystem.ScareNpcs(this.transform.position, this.smokeBombRadius);
                        }
                        break;
                }
                break;
        }
    }
}
