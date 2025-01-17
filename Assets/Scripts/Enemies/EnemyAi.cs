using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [Header("Detection")]
    public float sightRange;
    public float attackRange;
    public LayerMask whatIsPlayer;
    private Transform player;

    [Header("Components")]
    private NavMeshAgent agent;
    private bool playerInSightRange, playerInAttackRange;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Check for player in sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            // Patrolling behavior here if not in sight/attack range
        }

        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }

        if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        // Stop movement and attack behavior here
        agent.SetDestination(transform.position);
        transform.LookAt(player);
    }

    // Gizmos to visualize sight and attack range in the Editor
    private void OnDrawGizmosSelected()
    {
        // Visualize sight range in yellow
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        // Visualize attack range in red
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
