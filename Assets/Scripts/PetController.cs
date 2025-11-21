using UnityEngine;
using UnityEngine.AI;

public class PetMovement : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent navMeshAgent;
    public float FollowDistance = 2.0f;
    public Vector3 distance = new Vector3(-1f, 0.0f, -1f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > FollowDistance){    
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(player.position + distance);
       }
        else
        {
            navMeshAgent.isStopped = true;
        }
    }
} 
