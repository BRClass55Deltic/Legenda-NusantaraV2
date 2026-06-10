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

    public GameObject gameOverScreen;

    private bool isGameOver = false;

    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        anim = GetComponent<Animator>();

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }

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
        // 3. Ketika musuh menyentuh player dan game belum over
        if (other.CompareTag(playerTag) && !isGameOver)
        {
            Debug.Log("Player is Kill");
            
            isGameOver = true; // Tandai game sudah berakhir

           if (AudioManager.instance != null)
            {
                // BGM langsung mati, SFX dead langsung masuk!
                AudioManager.instance.TriggerGameOverAudio(); 
            }

            // Munculkan layar Game Over
            if (gameOverScreen != null)
            {
                gameOverScreen.SetActive(true); 
            }

            // (Opsional) Menghentikan waktu game jika kamu ingin game nge-freeze saat Game Over
            // Time.timeScale = 0f; 
        }
    }
}
