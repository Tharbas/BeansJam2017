using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIStates
{
    Idle,
    WalkingToStore,
    WaitingInLine,
    RandomMovement,
    Fleeing,
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

    public void ScareNpcs(Vector3 position, float radius)
    {
        foreach (AIEntity npc in npcs)
        {
            if (Vector3.Distance(npc.transform.position, position) < radius)
            {
                npc.CurrentSate = AIStates.Fleeing;
                npc.ActionTimer = Random.Range(npc.MaxIdleTime * 0.5f, npc.MaxIdleTime);
                npc.GetComponent<NavMeshAgent>().ResetPath();

                Vector3 startPos = new Vector3(Random.Range(-255, 255), 0, Random.Range(-130, 130));
                NavMeshHit hit;
                NavMesh.SamplePosition(startPos, out hit, 50, 1);
                npc.CurrentTarget = hit.position;
                npc.GetComponent<NavMeshAgent>().SetDestination(npc.CurrentTarget);
            }
                
        }
    }

    public void AddNpc(AIEntity npc)
    {
        npcs.Add(npc);
    }
	
	// Update is called once per frame
	void Update () {
        foreach (AIEntity npc in npcs)
        {
            if (npc.CurrentSate == AIStates.Fleeing)
            {
                npc.GetComponent<NavMeshAgent>().speed = 69 + (npc.ActionTimer * 20);     
            }
            else
            {
                npc.GetComponent<NavMeshAgent>().speed = 69;
            }


            if (npc.CurrentSate == AIStates.WaitingInLine || npc.CurrentSate == AIStates.Fleeing)
            {   
                if (npc.ActionTimer > 0f)
                {
                    npc.ActionTimer -= Time.deltaTime;
                    continue;
                }
                else
                {
                    npc.CurrentSate = AIStates.Idle;
                }
            }
            else if (npc.CurrentSate == AIStates.WalkingToStore)
            {
                if (Random.Range(0f, 1f) < npc.RandomMovementPercentage)
                {
                    npc.CurrentSate = AIStates.WaitingInLine;
                    npc.ActionTimer = Random.Range(npc.MinIdleTime, npc.MaxIdleTime * 0.33f);
                    npc.GetComponent<NavMeshAgent>().ResetPath();
                }
            }
            else if (npc.CurrentSate == AIStates.Idle)
            {
                NavMeshAgent agent = npc.GetComponent<NavMeshAgent>();
                if (agent.path == null || agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (npc.CurrentSate == AIStates.WalkingToStore)
                    {
                        npc.CurrentSate = AIStates.WaitingInLine;
                        npc.ActionTimer = Random.Range(npc.MinIdleTime, npc.MaxIdleTime);
                        continue;
                    }

                    agent.ResetPath();
                    int rand = Random.Range(0, waypoints.Count);
                    WaypointComponent nextWaypoint = waypoints[rand];
                    while (nextWaypoint.transform.position == npc.CurrentTarget)
                    {
                        rand = Random.Range(0, waypoints.Count);
                        nextWaypoint = waypoints[rand];
                    }
                    npc.CurrentTarget = nextWaypoint.transform.position;
                    npc.CurrentSate = AIStates.WalkingToStore;
                    npc.GetComponent<NavMeshAgent>().SetDestination(npc.CurrentTarget);
                }
            }
        }
	}
}
