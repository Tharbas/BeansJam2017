using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuGuiController : MonoBehaviour
{
    public enum WindowTypes
    {
        ChooseControls,
        Instructions,
        MainMenu,
        ErrorMessage
    }

    [SerializeField]
    private GameObject mainMenuWindow;

    [SerializeField]
    private GameObject chooseControlsWindow;

    [SerializeField]
    private GameObject instructionsWindow;

    [SerializeField]
    private GameObject errorWindow;

    [SerializeField]
    private Text errorText;

    [SerializeField]
    private ControlsSliderComponent keyboardSlider;

    [SerializeField]
    private ControlsSliderComponent gamepadSlider1;

    [SerializeField]
    private ControlsSliderComponent gamepadSlider2;

    [SerializeField]
    private GameObject confirmControlsButton;

    [SerializeField]
    private string prefKeyMafioso;

    [SerializeField]
    private string prefKeyCop;

    [SerializeField]
    private string prefValueKeyboard;

    [SerializeField]
    private string prefValueController1;

    [SerializeField]
    private string prefValueController2;

    [SerializeField]
    private string prefValueDelimiter;


    public void Start ()
    {
        this.HideAllWindows();
        this.confirmControlsButton.SetActive(false);
        this.mainMenuWindow.SetActive(true);

        PlayerPrefs.DeleteKey(this.prefKeyCop);
        PlayerPrefs.DeleteKey(this.prefKeyMafioso);

        string[] connectedJoysticks = Input.GetJoystickNames();

        if(connectedJoysticks.Length < 2)
        {
            this.gamepadSlider2.gameObject.SetActive(false);
        }

        if(connectedJoysticks.Length < 1 || (connectedJoysticks.Length == 1 && connectedJoysticks[0].Trim() == ""))
        {
            this.gamepadSlider1.gameObject.SetActive(false);
            this.errorText.text = "No connected controllers detected!\r\n\r\nYou need at least one controller in order to play the game.\r\n\r\nPlease check your connected devices and restart the game.";
            this.ShowWindow(WindowTypes.ErrorMessage);
            this.HideWindow(WindowTypes.MainMenu);
        }
	}

    public void Update()
    {
        this.checkControlsInput();
    }

    private void HideAllWindows()
    {
        this.instructionsWindow.SetActive(false);
        this.chooseControlsWindow.SetActive(false);
        this.errorWindow.SetActive(false);
    }

    public void ShowWindow(WindowTypes type)
    {
        switch (type)
        {
            case WindowTypes.ChooseControls:
                this.chooseControlsWindow.SetActive(true);
                break;
            case WindowTypes.Instructions:
                this.instructionsWindow.SetActive(true);
                break;
            case WindowTypes.MainMenu:
                this.mainMenuWindow.SetActive(true);
                break;
            case WindowTypes.ErrorMessage:
                this.errorWindow.SetActive(true);
                break;
        }
    }

    public void HideWindow(WindowTypes type)
    {
        switch (type)
        {
            case WindowTypes.ChooseControls:
                this.chooseControlsWindow.SetActive(false);
                break;
            case WindowTypes.Instructions:
                this.instructionsWindow.SetActive(false);
                break;
            case WindowTypes.MainMenu:
                this.mainMenuWindow.SetActive(false);
                break;
            case WindowTypes.ErrorMessage:
                this.errorWindow.SetActive(false);
                break;
        }
    }

    private void checkControlsInput()
    {
        if (this.chooseControlsWindow.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                this.keyboardSlider.MoveSliderInDirection(ControlsSliderComponent.SliderPositions.Left);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                this.keyboardSlider.MoveSliderInDirection(ControlsSliderComponent.SliderPositions.Right);
            }

            if(Input.GetAxis("Horizontal_Controller1") != 0)
            {
                if(Input.GetAxis("Horizontal_Controller1") < 0)
                {
                    this.gamepadSlider1.MoveSliderInDirection(ControlsSliderComponent.SliderPositions.Left);
                }
                else
                {
                    this.gamepadSlider1.MoveSliderInDirection(ControlsSliderComponent.SliderPositions.Right);
                }
            }

            if (Input.GetAxis("Horizontal_Controller2") != 0)
            {
                if (Input.GetAxis("Horizontal_Controller2") < 0)
                {
                    this.gamepadSlider1.MoveSliderInDirection(ControlsSliderComponent.SliderPositions.Left);
                }
                else
                {
                    this.gamepadSlider1.MoveSliderInDirection(ControlsSliderComponent.SliderPositions.Right);
                }
            }

            if (Input.GetButtonUp("Submit"))
            {
                if (this.IsReadyControlsSetupAllowed())
                {
                    this.StartGame();
                }
            }
        }
    }

    private bool IsReadyControlsSetupAllowed()
    {
        if (PlayerPrefs.GetString(this.prefKeyMafioso, "") != "")
        {
            if (PlayerPrefs.GetString(this.prefKeyCop, "") != "")
            {
                return true;
            }
        }

        return false;
    }

    public void SetControlsPlayerPref()
    {
        string valueCop = "";
        string valueMafioso = "";

        switch (this.keyboardSlider.CurrentPositon)
        {
            case ControlsSliderComponent.SliderPositions.Left:
                valueCop += this.prefValueKeyboard;
                break;
            case ControlsSliderComponent.SliderPositions.Right:
                valueMafioso += this.prefValueKeyboard;
                break;
        }

        switch (this.gamepadSlider1.CurrentPositon)
        {
            case ControlsSliderComponent.SliderPositions.Left:
                if(valueCop != "")
                {
                    valueCop += this.prefValueDelimiter;
                }

                valueCop += this.prefValueController1;
                break;
            case ControlsSliderComponent.SliderPositions.Right:
                if (valueMafioso != "")
                {
                    valueMafioso += this.prefValueDelimiter;
                }

                valueMafioso += this.prefValueController1;
                break;
        }

        switch (this.gamepadSlider2.CurrentPositon)
        {
            case ControlsSliderComponent.SliderPositions.Left:
                if (valueCop != "")
                {
                    valueCop += this.prefValueDelimiter;
                }

                valueCop += this.prefValueController2;
                break;
            case ControlsSliderComponent.SliderPositions.Right:
                if (valueMafioso != "")
                {
                    valueMafioso += this.prefValueDelimiter;
                }

                valueMafioso += this.prefValueController2;
                break;
        }

        PlayerPrefs.SetString(this.prefKeyCop, valueCop);
        PlayerPrefs.SetString(this.prefKeyMafioso, valueMafioso);

        this.confirmControlsButton.SetActive(this.IsReadyControlsSetupAllowed());
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

}
