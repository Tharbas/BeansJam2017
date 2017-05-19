using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuGuiController : MonoBehaviour
{
    public enum WindowTypes
    {
        ChooseControls,
        Instructions,
        MainMenu
    }

    [SerializeField]
    private GameObject mainMenuWindow;

    [SerializeField]
    private GameObject chooseControlsWindow;

    [SerializeField]
    private GameObject instructionsWindow;


    public void Start ()
    {
        this.HideAllWindows();
        this.mainMenuWindow.SetActive(true);
	}

    private void HideAllWindows()
    {
        this.instructionsWindow.SetActive(false);
        this.chooseControlsWindow.SetActive(false);
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
        }
    }

}
