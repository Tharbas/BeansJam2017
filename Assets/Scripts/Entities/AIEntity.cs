using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEntity : MonoBehaviour {

    public Vector3 CurrentTarget;
    public AIStates CurrentSate;

    public float MinIdleTime;
    public float MaxIdleTime;

    public float ActionTimer;

    public float RandomMovementPercentage;

    // Use this for initialization
	void Start () {
        CurrentSate = AIStates.Idle;
    }	
}
