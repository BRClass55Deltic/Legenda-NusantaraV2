using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChasing : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public string playerTag = "Player";
    private NavMeshAgent agent;
    private Transform player;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        GameObject p = GameObject.FindGameObjectWithTag(playerTag);
        if (p != null)
        {
            player = p.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ChasePlayer()
    }

     void ChasePlayer()
    {
        agent.SetDestination(player.position);

        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(agent.velocity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            
        }
    }
}
