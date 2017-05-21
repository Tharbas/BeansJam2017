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

    private float scanTime = 2.0f;
    private float scanTimeCount = 0.0f;

    // Use this for initialization
	void Start () {
        CurrentSate = AIStates.Idle;
    }

    public void Update()
    {
        if(this.scanTimeCount >= this.scanTime)
        {
            this.GetComponent<NavMeshAgent>().isStopped = false;
        }
        else
        {
            this.scanTimeCount += Time.deltaTime;
        }
    }

    public void IsBeingScanned()
    {
        this.scanTimeCount = 0;
        this.GetComponent<NavMeshAgent>().isStopped = true;
    }
}
