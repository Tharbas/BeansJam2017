using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour {

	public float roundTime;
	private bool timerRunnin = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		CountDownGameTime (roundTime);
		}




	private void CountDownGameTime(float rT)
	{
		if (timerRunnin && roundTime > 0.0f) {
			rT -= Time.deltaTime;
		} else {
			timerRunnin = false;
			Debug.Log ("Game Over");
		}
	}


}
