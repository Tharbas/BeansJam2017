using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GuiController
{
    private static MainMenuGuiController instance;

    public static MainMenuGuiController Instance { get { return instance != null ? instance : CreateInstance(); } }

    private static MainMenuGuiController CreateInstance()
    {
        instance = GameObject.Find("Controllers").GetComponent<MainMenuGuiController>();
        return instance;
    }
	
}
