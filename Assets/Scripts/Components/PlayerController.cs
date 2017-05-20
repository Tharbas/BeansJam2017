using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public enum PlayerTypes
    {
        Cop,
        Mafioso
    }

    [SerializeField]
    private PlayerTypes playerType;

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

    public void Start ()
    {
        switch (this.playerType)
        {
            case PlayerTypes.Cop:
                this.loadControlSettings(this.prefKeyCop);
                break;
            case PlayerTypes.Mafioso:
                this.loadControlSettings(this.prefKeyMafioso);
                break;
        }

        this.animator.SetInteger("WalkDirection", 0);
	}
	
	public void Update ()
    {
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
        movement.Normalize();

        this.transform.Translate ((movement * this.movementspeed * Time.deltaTime));

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
        if(this.playerType == PlayerTypes.Mafioso)
        {
            if (movementVector == Vector3.zero)
            {
                this.animator.SetInteger("WalkDirection", 0);
            }
            else
            {
                Vector3 temp = new Vector3(movementVector.x, -movementVector.z, movementVector.y); //Adjust the numbers to the correct world space values again

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
		
}


