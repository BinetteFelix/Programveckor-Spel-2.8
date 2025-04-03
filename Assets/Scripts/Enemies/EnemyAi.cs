using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [Header("Detection")]
    public float sightRange;
    public float attackRange;
    public LayerMask whatIsPlayer;
    [SerializeField] private Transform Player;
    

    [Header("Components")]
    private NavMeshAgent agent;
    private bool playerInSightRange, playerInAttackRange;

    [Header("Attacking")]
    [SerializeField] GameObject Projectile;
    [SerializeField] Transform ProjectileSpawnPoint;
    [SerializeField] private MuzzleFlash muzzleFlash;
    private float ProjectileSpeed = 500f;
    private bool isShooting;

    public static EnemyAi Instance;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
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
        agent.SetDestination(Player.position);
    }
    private void AttackPlayer()
    {
        // Stop movement and attack behavior here
        agent.SetDestination(transform.position);
        transform.LookAt(Player);
        if (!isShooting)
        StartCoroutine("Shoot");
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
    IEnumerator Shoot()
    {
        //Shoot at the player in certain intervals
        isShooting = true;
        float shotIntervals = 6.5f;
        float originalShotIntervals = shotIntervals;

        if (playerInAttackRange && Player.gameObject.activeSelf)
        {
            while (shotIntervals > 0f)
            {
                GameObject projectile = Instantiate(Projectile, ProjectileSpawnPoint.position, gameObject.transform.rotation);
                EnemyProjectiles projectileComponent = projectile.GetComponent<EnemyProjectiles>();


                if (projectile.TryGetComponent<Rigidbody>(out var rb))
                {
                    rb.linearVelocity = ProjectileSpawnPoint.position + ProjectileSpawnPoint.transform.forward * ProjectileSpeed;

                }
                if (projectile.TryGetComponent<EnemyProjectiles>(out EnemyProjectiles EP))
                {
                    EP.Initialize(EP.Damage, EP.HeadshotMultiplier);
                }
                shotIntervals -= originalShotIntervals;

                // Trigger muzzle flash
                if (muzzleFlash != null)
                {
                    muzzleFlash.Flash();
                }
            }

            yield return new WaitForSeconds(1.25f);
            shotIntervals = originalShotIntervals;
        }
        isShooting = false;
    }
}