using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIStates
{
    Idle,
    WalkingToStore,
    WaitingInLine,
    RandomMovement
}

public class AISystem : MonoBehaviour {

    private List<AIEntity> npcs;

	// Use this for initialization
	void Start () {
        npcs = new List<AIEntity>();
        npcs.AddRange(Component.FindObjectsOfType<AIEntity>());
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButtonUp(0))
        {
            Vector3 pos = Component.FindObjectOfType<WaypointComponent>().transform.position;
            pos.y = 0;
            foreach (AIEntity npc in npcs)
            {
                npc.GetComponent<NavMeshAgent>().SetDestination(pos);
            }
        }
	}
}
