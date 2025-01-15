using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private float patrolRange = 10f;  // Range within which the enemy can randomly patrol
    [SerializeField] private float timeBetweenPatrols = 2f;  // Time to wait before picking the next random point

    [Header("Components")]
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        PatrolRandomly();
    }

    private void Update()
    {
        // If the enemy has reached the target point, start a new patrol
        if (agent.remainingDistance <= 0.2f && !agent.pathPending)
        {
            PatrolRandomly();
        }
    }

    private void PatrolRandomly()
    {
        // Wait for a moment before selecting the next random point
        Invoke(nameof(SelectRandomPatrolPoint), timeBetweenPatrols);
    }

    private void SelectRandomPatrolPoint()
    {
        // Generate a random position within a defined patrol range
        Vector3 randomDirection = Random.insideUnitSphere * patrolRange;

        // Keep the random point within the current height (y position) of the enemy
        randomDirection += transform.position;

        // Ensure that the random point is on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 1f, NavMesh.AllAreas))
        {
            // Set the agent's destination to the randomly selected point
            agent.SetDestination(hit.position);
        }
    }
}
