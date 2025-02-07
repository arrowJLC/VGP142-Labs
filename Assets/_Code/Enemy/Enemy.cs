

//using System.Security.Cryptography;
//using UnityEngine;
//using UnityEngine.AI;

//public class Enemy : MonoBehaviour
//{
//    public enum EnemyState
//    {
//        Chase, Patrol

//    }
//    public Transform player;
//    public EnemyState state;

//    NavMeshAgent agent;
//    Transform target;



//    public Transform[] path;
//    public int pathIndex = 0;
//    public float distThreshold = 0.2f;

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        //rb = GetComponent<Rigidbody>();
//        agent = GetComponent<NavMeshAgent>();
//    }

//    // Update is called once per frame
//    void FixedUpdate()
//    {

//        if (!player) return;

//        if (state == EnemyState.Chase) target = player;

//        if (state == EnemyState.Patrol)
//        {
//            if (target == player) target = path[pathIndex];
//            if (agent.remainingDistance < distThreshold)
//            {
//                pathIndex++;
//                pathIndex %= path.Length;
//                target = path[pathIndex];
//            }
//        }
//        agent.SetDestination(target.position);

//    }
//}

//using System.Collections;
//using System.IO;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.AI;

//public class EnemyController : MonoBehaviour
//{
//    public float moveSpeed = 3f;
//    public float attackRange = 10f; // Range for shooting
//    public float attackCooldown = 2f; // Time between attacks
//    public GameObject projectilePrefab; // Reference to the projectile prefab
//    public Transform shootPoint; // The point from where the projectile will shoot
//    public float projectileSpeed = 10f; // Speed of the projectile
//    public float rotationSpeed = 5f;

//    private Animator anim;
//    private Transform player;
//   // private Rigidbody rb;
//    NavMeshAgent agent;

//    private bool isAttacking = false;
//    private float lastAttackTime;

//    public float stoppingDistance = 1.5f;
//    public float visibilityAngle = 45f;
//    private SkinnedMeshRenderer enemyRenderer;

//    private void Start()
//    {
//        //rb = GetComponent<Rigidbody>();
//        anim = GetComponent<Animator>();
//        agent = GetComponent<NavMeshAgent>();

//        enemyRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

//        player = GameObject.FindWithTag("Player").transform; // Assuming the player has the "Player" tag
//        lastAttackTime = -attackCooldown; // Ensures the enemy can attack right away at the start
//    }

//    private void Update()
//    {
//        if (isAttacking) return;  // Prevent movement during attack animation

//        MoveAgent2();
//        //MoveTowardsPlayer();
//        CheckForAttack();
//    }

//    private void MoveAgent2()
//    {
//        if (!player) return;

//        float distance = Vector3.Distance(transform.position, player.position);
//        Vector3 direction = (player.position - transform.position).normalized;

//        //agent.SetDestination(player.position);

//        if (distance > attackRange)
//        {
//            agent.SetDestination(player.position);

//            // Set walking animation
//            anim.SetBool("isWalking", true);

//            // If the player is beyond stopping distance, keep moving
//            if (distance > stoppingDistance)
//            {
//                anim.SetFloat("Speed", 1);
//            }
//        }
//        else
//        {
//            // Set idle animation if in attack range
//            anim.SetBool("isWalking", false);
//            anim.SetFloat("Speed", 0);
//        }
//    }
//    private void MoveTowardsPlayer()
//    {
//        float distance = Vector3.Distance(transform.position, player.position);
//        Vector3 direction = (player.position - transform.position).normalized;

//        // Move and rotate if the player is outside the attack range
//        if (distance > attackRange)
//        {
//            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

//            Quaternion targetRotation = Quaternion.LookRotation(direction);
//            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

//            // Set walking animation
//            anim.SetBool("isWalking", true);

//            // If the player is beyond stopping distance, keep moving
//            if (distance > stoppingDistance)
//            {
//                anim.SetFloat("Speed", 1);
//            }
//        }
//        else
//        {
//            // Set idle animation if in attack range
//            anim.SetBool("isWalking", false);
//            anim.SetFloat("Speed", 0);
//        }
//    }

//    private void HandleVisibility()
//    {

//        Vector3 directionToPlayer = (player.position - transform.position).normalized;

//        Vector3 booForward = transform.forward;

//        float dotProduct = Vector3.Dot(booForward, directionToPlayer);

//        float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;

//        if (angle <= visibilityAngle)
//        {
//            enemyRenderer.enabled = true;
//        }
//        else
//        {
//            enemyRenderer.enabled = false; 
//        }
//    }


//    private void CheckForAttack()
//    {
//        float distance = Vector3.Distance(transform.position, player.position);

//        if (distance <= attackRange && Time.time > lastAttackTime + attackCooldown && !isAttacking)
//        {
//            StartCoroutine(Shoot());
//        }
//    }

//    private IEnumerator Shoot()
//    {
//        isAttacking = true;

//        if (isAttacking)
//        {
//            anim.SetFloat("Speed", 0);
//        }
//        anim.SetTrigger("Fire");  // Trigger the shooting animation
//        lastAttackTime = Time.time; // Set the time of the last attack

//        // Instantiate the projectile
//        if (projectilePrefab != null && shootPoint != null)
//        {
//            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

//            // Move the projectile towards the player
//            Vector3 direction = (player.position - shootPoint.position).normalized;
//            Rigidbody rb = projectile.GetComponent<Rigidbody>();
//            if (rb != null)
//            {
//                rb.linearVelocity = direction * projectileSpeed;
//            }
//        }

//        yield return new WaitForSeconds(attackCooldown);  // Cooldown time before the next attack
//        isAttacking = false;
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("Enemy") && CompareTag("Player"))
//        {
//            //anim.SetBool("isDizzed", true);
//            //anim.SetFloat("isDizzed", 0);
//            //anim.SetTrigger("isDizzed");
//        }
//    }
//}