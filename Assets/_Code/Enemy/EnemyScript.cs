using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        Chase, Patrol, Death
    }

    public Transform player;
    public Animator anim;
    NavMeshAgent agent;
    Transform target;

    EnemyHealthCon eh;
    public EnemyState state;

    public float attackRange = 10f;
    public float maxShootDistance = 10f;
    public float attackCooldown = 2f;
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float projectileSpeed = 10f;

    private bool isAttacking = false;
    private float lastAttackTime;

    public float stoppingDistance = 1.5f;

    public Transform[] path;
    public int pathIndex = 0;
    public float distThreshold = 0.2f;

    private int hitCount = 0;

    private bool hasSpawnedCollectibles = false;

    public List<Transform> spawnPoints;
    public List<GameObject> dropsPrefabs;

    private void Start()
    {

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        eh = GetComponent<EnemyHealthCon>();    

        lastAttackTime = -attackCooldown;
    }

    void FixedUpdate()
    {

        if (!player) return;

        switch (state)
        {
            case EnemyState.Patrol:
                PatrolBehavior();
                break;

            case EnemyState.Chase:
                ChaseBehavior();
                break;

            case EnemyState.Death:
                DeathBehavior();
                break;
        }

        //if (state == EnemyState.Chase)
        //{
        //    target = player;

        //    CheckForAttack();

        //    float distance = Vector3.Distance(transform.position, target.position);
        //    Vector3 direction = (target.position - transform.position).normalized;

        //    if (distance > attackRange)
        //    {
        //        agent.SetDestination(player.position);


        //        anim.SetBool("isWalking", true);


        //        if (distance > stoppingDistance)
        //        {
        //            anim.SetFloat("Speed", 1);
        //        }
        //    }
        //    else
        //    {

        //        anim.SetBool("isWalking", false);
        //        anim.SetFloat("Speed", 0);
        //    }
        //}


        //if (state == EnemyState.Patrol)
        //{
        //    if (target == player) target = path[pathIndex];

        //    if (Vector3.Distance(transform.position, player.position) <= attackRange)
        //    {
        //        (state == EnemyState.Chase);
        //    }

        //    if (agent.remainingDistance < distThreshold)
        //    {
        //        pathIndex++;
        //        pathIndex %= path.Length;
        //        target = path[pathIndex];
        //    }
        //}
        //agent.SetDestination(target.position);

    }
    private void ChaseBehavior()
    {
        target = player;

        CheckForAttack();
        CheckForDeath();

        float distance = Vector3.Distance(transform.position, target.position);
        Vector3 direction = (target.position - transform.position).normalized;

        if (distance > attackRange)
        {
            agent.SetDestination(player.position);

            anim.SetBool("isWalking", true);

            if (distance > stoppingDistance)
            {
                anim.SetFloat("Speed", 1);
            }
        }

        else
        {
            anim.SetBool("isWalking", false);
            anim.SetFloat("Speed", 0);
        }

    }
    private void PatrolBehavior()
    {
        target = path[pathIndex];

        // Transition to Chase state if player is within range
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            state = EnemyState.Chase;
        }

        // Move to the next patrol point if the agent reaches the current one
        if (agent.remainingDistance < distThreshold)
        {
            pathIndex++;
            pathIndex %= path.Length;
        }

        anim.SetBool("isWalking", true);
        anim.SetFloat("Speed", 1);
        agent.SetDestination(target.position);
        eh.currentHealth = 100f;
    }

    private void DeathBehavior()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero; 

        if (!hasSpawnedCollectibles)
        {
            hasSpawnedCollectibles = true; 
            SpawnCollectibles();
        }

        target = null;

        anim.SetTrigger("Death");
        anim.SetBool("isWalking", false);
        anim.SetFloat("Speed", 0);

        // Optionally, you can add a line here to destroy the enemy after it dies:
        // Destroy(gameObject, 2f);  // Destroys the enemy after a delay of 2 seconds
    }
    //private void DeathBehavior()
    //{
    //    target = null;

    //    anim.SetTrigger("Death");
    //    anim.SetBool("isWalking", false);
    //    anim.SetFloat("Speed", 0);
    //    //agent.SetDestination(target.position);

    //    SpawnCollectibles();
    //}

    void SpawnCollectibles()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {

            GameObject collectible = dropsPrefabs[Random.Range(0, dropsPrefabs.Count)];

            Instantiate(collectible, spawnPoint.position, Quaternion.identity);
        }
    }

    private void CheckForAttack()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= attackRange && Time.time > lastAttackTime + attackCooldown && !isAttacking)
        {
            StartCoroutine(Shoot());
        }
    }

    private void CheckForDeath()
    {
        if (eh.currentHealth <= 0)
        {
            Debug.Log("Enemy is dead!");
            state = EnemyState.Death;
            StartCoroutine(WaitAndReload());
        }
    }

    IEnumerator WaitAndReload()
    {
        yield return new WaitForSeconds(10f);
        Debug.Log("Coroutine is triggered!");
        // destroying destroys collectable ... fix first
        //Destroy(gameObject); 
    }

    private IEnumerator Shoot()
    {
        if (Vector3.Distance(transform.position, player.position) > maxShootDistance)
        {
            yield break;  
        }

        anim.SetBool("isWalking", false);
        anim.SetFloat("Speed", 0);

        isAttacking = true;

        anim.SetTrigger("Fire");
        lastAttackTime = Time.time;

        if (projectilePrefab != null && shootPoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            agent.velocity = Vector3.zero;

            // Move the projectile towards the player
            Vector3 direction = (player.position - shootPoint.position).normalized;
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = direction * projectileSpeed;  
            }
        }

        yield return new WaitForSeconds(attackCooldown);

        if (state == EnemyState.Chase)
        {
            anim.SetBool("isWalking", true);
            anim.SetFloat("Speed", 1);
        }

        isAttacking = false;
    }
}

