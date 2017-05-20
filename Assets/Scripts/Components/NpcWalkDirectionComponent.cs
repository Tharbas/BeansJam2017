using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcWalkDirectionComponent : MonoBehaviour {

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private NavMeshAgent navMeshAgent;

    public void Update()
    {
        if (!this.navMeshAgent.hasPath)
        {            
            this.animator.SetInteger("WalkDirection", 0);
        }
        else
        {

            float angle = Vector3.Angle(Camera.main.transform.up, this.transform.forward);

            if (this.transform.forward.normalized.x < 0)
            {
                angle = 360 - angle;
            }

            if (angle > 315 || (angle >= 0 && angle <= 45))
            {
                this.animator.SetInteger("WalkDirection", 1);
            }
            else if (angle > 45 && angle <= 135)
            {                
                this.animator.SetInteger("WalkDirection", 3);
            }
            else if (angle > 135 && angle <= 225)
            {                
                this.animator.SetInteger("WalkDirection", 5);
            }
            else if (angle > 225 && angle <= 315)
            {
                this.animator.SetInteger("WalkDirection", 7);
            }
        }

        this.animator.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }
}
