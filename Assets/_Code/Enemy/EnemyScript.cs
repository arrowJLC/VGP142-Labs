using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        Chase, Patrol
    }

    public Transform player;
    private Animator anim;
    NavMeshAgent agent;
    Transform target;
    public EnemyState state;

    public float attackRange = 10f;
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

    private void Start()
    {

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        lastAttackTime = -attackCooldown;
    }

    void FixedUpdate()
    {

        if (!player) return;

        // Update behavior based on the current state
        switch (state)
        {
            case EnemyState.Patrol:
                PatrolBehavior();
                break;

            case EnemyState.Chase:
                ChaseBehavior();
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
    }

    private void CheckForAttack()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= attackRange && Time.time > lastAttackTime + attackCooldown && !isAttacking)
        {
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        anim.SetBool("isWalking", false);  
        anim.SetFloat("Speed", 0);

        isAttacking = true;

        
        anim.SetTrigger("Fire");  // Trigger the shooting animation
        lastAttackTime = Time.time; // Set the time of the last attack

        // Instantiate the projectile
        if (projectilePrefab != null && shootPoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

            // Move the projectile towards the player
            Vector3 direction = (player.position - shootPoint.position).normalized;
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = direction * projectileSpeed;
            }
        }

        yield return new WaitForSeconds(attackCooldown);  // Cooldown time before the next attack

        if (state == EnemyState.Chase)  // Only resume walking if we're still in the chase state
        {
            anim.SetBool("isWalking", true);
            anim.SetFloat("Speed", 1);   // Resume walking speed animation
        }

        isAttacking = false;
    }
}

