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

    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        anim = GetComponent<Animator>();

        GameObject p = GameObject.FindGameObjectWithTag(playerTag);
        if (p != null)
        {
            player = p.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null /*|| isCaught*/) 
        {
            // Jika tidak ada player, pastikan animasi chase mati
            if (anim != null) anim.SetBool("isChasing", false);
            return;
        }
        ChasePlayer();
    }

     void ChasePlayer()
    {
        agent.SetDestination(player.position);

    
        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            if (anim != null) anim.SetBool("isChasing", true); 
            transform.rotation = Quaternion.LookRotation(agent.velocity);
        }
        else
        {
            if (anim != null) anim.SetBool("isChasing", false); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Player is Kill");
        }
    }
}
