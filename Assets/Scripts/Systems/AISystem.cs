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

    private List<WaypointComponent> waypoints;


	// Use this for initialization
	void Start () {
        npcs = new List<AIEntity>();
        npcs.AddRange(Component.FindObjectsOfType<AIEntity>());

        waypoints = new List<WaypointComponent>();
        waypoints.AddRange(Component.FindObjectsOfType<WaypointComponent>());

    }

    public void AddNpc(AIEntity npc)
    {
        npcs.Add(npc);
    }
	
	// Update is called once per frame
	void Update () {
        foreach (AIEntity npc in npcs)
        {
            if(npc.CurrentSate == AIStates.WaitingInLine)
            {
                if (npc.IdleTime > 0f)
                {
                    npc.IdleTime -= Time.deltaTime;
                    continue;
                }
                else
                {
                    npc.CurrentSate = AIStates.Idle;
                }
            }
            NavMeshAgent agent = npc.GetComponent<NavMeshAgent>();
            if (agent.path == null || agent.remainingDistance <= agent.stoppingDistance)
            {
                if (npc.CurrentSate == AIStates.WalkingToStore)
                {
                    npc.CurrentSate = AIStates.WaitingInLine;
                    npc.IdleTime = Random.Range(npc.MinIdleTime, npc.MaxIdleTime);
                    continue;
                }

                agent.ResetPath();
                int rand = Random.Range(0, waypoints.Count);
                WaypointComponent nextWaypoint = waypoints[rand];
                while (npc.CurrentTarget != null && nextWaypoint == npc.CurrentTarget)
                {
                    rand = Random.Range(0, waypoints.Count);
                    nextWaypoint = waypoints[rand];
                }
                npc.CurrentTarget = nextWaypoint;
                npc.CurrentSate = AIStates.WalkingToStore;
                npc.GetComponent<NavMeshAgent>().SetDestination(npc.CurrentTarget.transform.position);
            }
        }
	}
}
