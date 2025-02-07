//using System.Collections;
//using UnityEngine;

//public class EnemyVisibility : MonoBehaviour
//{
//    public Transform player;
//    private MeshRenderer mr;
//    public Renderer enemyRenderer;
//    public float fieldOfViewAngle = 45f;
//    public float maxSightDistance = 20f;
//    private Material enemyMaterial;
//    public float detectionRadius = 10f;
//    public float invisibilityDistance = 5f;
//    private bool enemyLookingAtPlayer = false;

//    void Start()
//    {

//        enemyRenderer = GetComponent<Renderer>();
//        enemyMaterial = enemyRenderer.material;


//        StartCoroutine(CheckPlayerProximity());
//    }

//    void Update()
//    {

//        Vector3 toPlayer = player.position - transform.position;


//        float angle = Vector3.Angle(transform.forward, toPlayer);
//        RaycastHit hit;
//        bool isPlayerVisible = angle < fieldOfViewAngle &&
//                               Physics.Raycast(transform.position, toPlayer.normalized, out hit, maxSightDistance) &&
//                               hit.transform == player;

//        if (isPlayerVisible)
//        {
//            enemyRenderer.enabled = true;
//            SetTransparency(1f);
//        }
//        else
//        {
//            enemyRenderer.enabled = true;
//            SetTransparency(0f); // Ft
//        }
//    }

//    private IEnumerator CheckPlayerProximity()
//    {
//        while (true)
//        {
//            if (player != null)
//            {
//                float distanceToPlayer = Vector3.Distance(transform.position, player.position);


//                enemyLookingAtPlayer = IsPlayerInFrontOfEnemy();

//                if (distanceToPlayer <= invisibilityDistance && !enemyLookingAtPlayer)
//                {
//                    SetEnemyVisibility(false);
//                }
//                else
//                {
//                    SetEnemyVisibility(true);
//                }
//            }

//            yield return new WaitForSeconds(0.1f);
//        }
//    }

//    private bool IsPlayerInFrontOfEnemy()
//    {
//        Vector3 directionToPlayer = (player.position - transform.position).normalized;
//        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);
//        return dotProduct > 0.5f;
//    }

//    private void SetTransparency(float alpha)
//    {
//        Color color = enemyMaterial.color;
//        color.a = alpha;
//        enemyMaterial.color = color;
//    }

//    private void SetEnemyVisibility(bool visible)
//    {
//        if (enemyRenderer != null)
//        {
//            enemyRenderer.enabled = visible;
//        }
//    }
//}


using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.VisualScripting;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;


public class Enemy2 : MonoBehaviour
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

        if (state == EnemyState.Chase)
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

       
        if (state == EnemyState.Patrol)
        {
            if (target == player) target = path[pathIndex];
            if (agent.remainingDistance < distThreshold)
            {
                pathIndex++;
                pathIndex %= path.Length;
                target = path[pathIndex];
            }
        }
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
        isAttacking = true;

        if (isAttacking)
        {
            anim.SetFloat("Speed", 0);
        }
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
        isAttacking = false;
    }

}