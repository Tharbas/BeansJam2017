using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightComponent : MonoBehaviour {

    public GameObject HighlightMaterial;

    public float HighlightDuration;
    float highlightTimer;

	// Use this for initialization
	void Start () {
        HighlightMaterial.SetActive(false);
    }
	
    public void DoHighlight()
    {
        highlightTimer = HighlightDuration;
        HighlightMaterial.SetActive(true);
    }

	// Update is called once per frame
	void Update () {
        if (highlightTimer > 0f)
        {
            highlightTimer -= Time.deltaTime;
            if (highlightTimer <= 0f)
            {
                HighlightMaterial.SetActive(false);
            }
        }
	}
}
