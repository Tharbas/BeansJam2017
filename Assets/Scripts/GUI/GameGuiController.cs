using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class GameGuiController : MonoBehaviour {

    [SerializeField]
    private float startTimeBlink;

    [SerializeField]
    private Text textTimeLeft;

    [SerializeField]
    private Text textTimeCount;

    [SerializeField]
    private Text textMoneyStashed;

    [SerializeField]
    private Text textMoneyCarried;

    private bool isBlinking = false;

    void Start()
	{
		
	}

    public void SetTimeLeft(TimeSpan span)
    {
        this.textTimeCount.text =  string.Format("{0:00}:{1:00}", span.Minutes, span.Seconds);

        if(!this.isBlinking && span.Seconds <= this.startTimeBlink)
        {
            this.isBlinking = true;
            this.textTimeLeft.gameObject.GetComponent<Animator>().SetTrigger("StartBlink");
            this.textTimeCount.gameObject.GetComponent<Animator>().SetTrigger("StartBlink");
        }
    }

}
