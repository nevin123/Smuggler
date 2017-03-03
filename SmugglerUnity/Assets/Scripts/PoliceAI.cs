using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceAI : MonoBehaviour {

    public Transform Target;
    NavMeshAgent agent;
    NavMeshPath path = new NavMeshPath();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        Target = PlayerManager.instance.Players["Player 2"].transform;
    }

    void Update()
    {
        if(Target == null)
        {
            Target = PlayerManager.instance.Players["Player 2"].transform;
            return;
        }

        agent.SetDestination(Target.position);
    }
}
