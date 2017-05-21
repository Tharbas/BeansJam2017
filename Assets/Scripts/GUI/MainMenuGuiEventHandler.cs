using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuGuiEventHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private MainMenuGuiController guiController;

    public void OnPointerClick(PointerEventData eventData)
    {
        this.guiController.PlayButtonBeep();

        switch (eventData.pointerPress.gameObject.name)
        {
            case "Button_StartGame":
                this.guiController.ShowWindow(MainMenuGuiController.WindowTypes.ChooseControls);
                break;
            case "Button_Instructions":
                this.guiController.ShowWindow(MainMenuGuiController.WindowTypes.Instructions);
                this.guiController.ShowWindow(MainMenuGuiController.WindowTypes.InstructionsGeneric);
                break;
            case "Button_Instructions_Back":
                this.guiController.HideWindow(MainMenuGuiController.WindowTypes.Instructions);
                this.guiController.HideWindow(MainMenuGuiController.WindowTypes.InstructionsGeneric);
                this.guiController.HideWindow(MainMenuGuiController.WindowTypes.InstructionsCop);
                this.guiController.HideWindow(MainMenuGuiController.WindowTypes.InstructionsMafioso);
                this.guiController.ShowWindow(MainMenuGuiController.WindowTypes.MainMenu);
                break;
            case "Button_Instructions_Generic":
                this.guiController.HideWindow(MainMenuGuiController.WindowTypes.InstructionsCop);
                this.guiController.HideWindow(MainMenuGuiController.WindowTypes.InstructionsMafioso);
                this.guiController.ShowWindow(MainMenuGuiController.WindowTypes.InstructionsGeneric);
                break;
            case "Button_Instructions_Cop":
                this.guiController.HideWindow(MainMenuGuiController.WindowTypes.InstructionsGeneric);
                this.guiController.HideWindow(MainMenuGuiController.WindowTypes.InstructionsMafioso);
                this.guiController.ShowWindow(MainMenuGuiController.WindowTypes.InstructionsCop);
                break;
            case "Button_Instructions_Mafioso":
                this.guiController.HideWindow(MainMenuGuiController.WindowTypes.InstructionsGeneric);
                this.guiController.HideWindow(MainMenuGuiController.WindowTypes.InstructionsCop);
                this.guiController.ShowWindow(MainMenuGuiController.WindowTypes.InstructionsMafioso);
                break;
            case "Button_ChooseControls_Back":
                this.guiController.HideWindow(MainMenuGuiController.WindowTypes.ChooseControls);
                break;
            case "Button_ErrorMessage_Back":
                this.guiController.HideWindow(MainMenuGuiController.WindowTypes.ErrorMessage);
                break;
            case "Button_Controls":
                this.guiController.HideWindow(MainMenuGuiController.WindowTypes.ControlsMafioso);
                this.guiController.ShowWindow(MainMenuGuiController.WindowTypes.ControlsGeneric);
                this.guiController.ShowWindow(MainMenuGuiController.WindowTypes.ControlsCop);
                break;
            case "Button_Controls_Back":
                this.guiController.HideWindow(MainMenuGuiController.WindowTypes.ControlsGeneric);
                break;
            case "Button_Controls_Cop":
                this.guiController.HideWindow(MainMenuGuiController.WindowTypes.ControlsMafioso);
                this.guiController.ShowWindow(MainMenuGuiController.WindowTypes.ControlsCop);
                break;
            case "Button_Controls_Mafioso":
                this.guiController.HideWindow(MainMenuGuiController.WindowTypes.ControlsCop);
                this.guiController.ShowWindow(MainMenuGuiController.WindowTypes.ControlsMafioso);
                break;
            case "Button_QuitGame":
                Application.Quit();
                break;
        }
    }
}
