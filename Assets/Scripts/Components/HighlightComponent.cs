using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightComponent : MonoBehaviour {

    public float HighlightDuration;
    float highlightTimer;

	// Use this for initialization
	void Start () {
		
	}
	
    public void DoHighlight()
    {
        highlightTimer = HighlightDuration;
    }

	// Update is called once per frame
	void Update () {
        if (HighlightDuration > 0f)
        {
                
        }

	}
}
