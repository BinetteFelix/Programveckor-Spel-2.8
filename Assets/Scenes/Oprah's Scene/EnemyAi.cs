using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    // Patrullering
    private Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    // Attackerar
    public float timeBetweenAttacks;
    private bool alreadyAttacked;
    public GameObject projectile;

    // Tillst�nd
    public float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Kontrollera om spelaren �r inom syn- och attackr�ckvidd
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) Chasing();
        if (playerInSightRange && playerInAttackRange) AttackingPlayer();
    }

    private void Patrolling()
    {
        if (!walkPointSet)
            SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);

            // Kontrollera om fienden har n�tt patrullpunkten
            Vector3 distanceToWalkPoint = transform.position - walkPoint;
            if (distanceToWalkPoint.sqrMagnitude < 1f)
            {
                walkPointSet = false;
            }
        }
    }

    private void SearchWalkPoint()
    {
        // Generera en slumpm�ssig patrullpunkt
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        Vector3 potentialPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // Kontrollera att punkten �r giltig p� NavMesh
        if (NavMesh.SamplePosition(potentialPoint, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            walkPoint = hit.position;
            walkPointSet = true;
        }
    }

    private void Chasing()
    {
        agent.SetDestination(player.position);

        // N�r fienden f�rlorar synen p� spelaren, �terg� till patrullering
        if (!playerInSightRange)
        {
            walkPointSet = false; // Tvinga patrulleringen att generera en ny punkt
        }
    }

    private void AttackingPlayer()
    {
        // Stanna r�relsen
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            PerformAttack();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void PerformAttack()
    {
        // Skapa och skjut projektil
        Rigidbody rb = Instantiate(projectile, transform.position + transform.forward, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
        rb.AddForce(transform.up * 8f, ForceMode.Impulse);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        // Visualisera r�ckvidder i editorn
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }


}