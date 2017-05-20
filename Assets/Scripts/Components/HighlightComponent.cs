using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightComponent : MonoBehaviour {

    public GameObject HighlightMaterial;

    private Material spriteMaterial;
    public float HighlightDuration;
    float highlightTimer;

	// Use this for initialization
	void Start () {
        spriteMaterial = HighlightMaterial.GetComponent<SpriteRenderer>().material;
    }
	
    public void DoHighlight()
    {
        highlightTimer = HighlightDuration;
        spriteMaterial.SetFloat("_EnableOutline", 1f);
    }

	// Update is called once per frame
	void Update () {
        if (highlightTimer > 0f)
        {
            highlightTimer -= Time.deltaTime;
            if (highlightTimer <= 0f)
            {
                spriteMaterial.SetFloat("_EnableOutline", 0f);
            }
        }
	}
}
