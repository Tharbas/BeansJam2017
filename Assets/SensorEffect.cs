using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorEffect : MonoBehaviour {

    int step = 0;

    public delegate void FinishedHandler();

    public FinishedHandler OnAnimationDone;

	// Use this for initialization
	void Start () {
        step = -1;	
	}
	
    public void RestartEffect()
    {
        step = 0;
    }

	// Update is called once per frame
	void Update () {
		// X 0 -> 800
        // Z 0.3 -> 255
        // Ry 0 -> 90
        // X 800 -> 0
        if (step == 0)
        {
            if (transform.localScale.x < 750f)
            {
                Vector3 target = transform.localScale;
                target.x = 900f;
                transform.localScale = Vector3.Lerp(transform.localScale, target, 7f * Time.deltaTime);
            }
            else
            {
                step = 1;
            }
        }
        else if (step == 1)
        {
            if (transform.localScale.z < 255f)
            {
                Vector3 target = transform.localScale;
                target.z = 300f;
                transform.localScale = Vector3.Lerp(transform.localScale, target, 7f * Time.deltaTime);
            }
            else
            {
                step = 2;
            }
        }
        else if (step == 2)
        {
            if (transform.rotation.eulerAngles.y < 90f)
            {
                transform.Rotate(Vector3.up *  145f * Time.deltaTime);
            }
            else
            {
                step = 3;
            }
        }
        else if (step == 3)
        {
            if (transform.localScale.x > 0f)
            {
                Vector3 target = transform.localScale;
                target.x = -100f;
                transform.localScale = Vector3.Lerp(transform.localScale, target, 7f * Time.deltaTime);
            }
            else
            {
                step = -1;
                transform.localScale = new Vector3(1, 1, 1);
                transform.rotation = Quaternion.identity;
                OnAnimationDone();
            }
        }

    }
}
