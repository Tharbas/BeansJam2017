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
    private ParticleEmitter smokeBombEmitter;

    [SerializeField]
    private GameObject sensorArrow;

    private void Update()
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
    }

    public void DoAction(PossibleActions action)
    {
        switch (this.playerType)
        {
            case PlayerTypes.Cop:
                switch (action)
                {
                    case PossibleActions.ArrestPerson:
                        Debug.Log("ArrestPerson");
                        break;
                    case PossibleActions.ScanPerson:
                        Debug.Log("ScanPerson");
                        break;
                    case PossibleActions.Shoot:
                        Debug.Log("Shoot");
                        break;
                    case PossibleActions.UseSensors:
                        Debug.Log("UseSensors");
                        break;
                }
                break;
            case PlayerTypes.Mafioso:
                switch (action)
                {
                    case PossibleActions.CollectStashMoney:
                        Debug.Log("CollectMoney");
                        break;
                    case PossibleActions.DropSmokeBomb:
                        Debug.Log("SmokeBomb");
                        break;
                }
                break;
        }
    }
}
