using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuGuiEventHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.pointerPress.gameObject.name)
        {
            case "Button_StartGame":
                GuiController.Instance.HideWindow(MainMenuGuiController.WindowTypes.MainMenu);
                GuiController.Instance.ShowWindow(MainMenuGuiController.WindowTypes.ChooseControls);
                break;
            case "Button_Instructions":
                GuiController.Instance.HideWindow(MainMenuGuiController.WindowTypes.MainMenu);
                GuiController.Instance.ShowWindow(MainMenuGuiController.WindowTypes.Instructions);
                break;
            case "Button_Instructions_Back":
                GuiController.Instance.HideWindow(MainMenuGuiController.WindowTypes.Instructions);
                GuiController.Instance.ShowWindow(MainMenuGuiController.WindowTypes.MainMenu);
                break;
            case "Button_ChooseControls_Back":
                GuiController.Instance.HideWindow(MainMenuGuiController.WindowTypes.ChooseControls);
                GuiController.Instance.ShowWindow(MainMenuGuiController.WindowTypes.MainMenu);
                break;
            case "Button_ErrorMessage_Back":
                GuiController.Instance.HideWindow(MainMenuGuiController.WindowTypes.ErrorMessage);
                GuiController.Instance.ShowWindow(MainMenuGuiController.WindowTypes.MainMenu);
                break;
            case "Button_QuitGame":
                Application.Quit();
                break;
        }
    }
}
