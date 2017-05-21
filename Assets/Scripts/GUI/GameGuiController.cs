using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class GameGuiController : MonoBehaviour {

    [SerializeField]
    private Text textTimeLeft;

    [SerializeField]
    private Text textTimeCount;

    [SerializeField]
    private Text textMoneyStashed;

    [SerializeField]
    private Text textMoneyCarried;

    void Start()
	{
		
	}

    public void SetTimeLeft(TimeSpan span)
    {
        //TimeLabel.text = "Time Left : " + string.Format("{0:00}:{1:00}", st.Minutes, st.Seconds);
    }

}
