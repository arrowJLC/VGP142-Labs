//using NUnit.Framework;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;
//using UnityEngine.SceneManagement;

//public enum EnemyStates
//{
//    Idle,
//    Patrolling,
//    Chasing,
//    Attacking,
//    Dead
//}

//public class trackerEnemy : MonoBehaviour
//{
//    protected int health;
//    [SerializeField] protected int maxHealth;
//    protected EnemyState currentState;

//    private Transform playerTransform;
//    private float followDistance = 10f;  
//    private float attackDistance = 20f;   
//    private float fireballCooldown = 2f;
//    private float lastFireballTime = 0f;

//    private Rigidbody rb;
//    [SerializeField]
//    private float moveSpeed = 10f;

//    Animator anim;


//    [SerializeField] private GameObject fireballPrefab;
//    [SerializeField] private Transform fireballSpawnPoint;
//    [SerializeField] private float fireballSpeed = 10f;

//    private Renderer enemyRenderer;

//    private float timeSinceLastFire = 0;

//    public virtual void Start()
//    {

//        playerTransform = GameObject.FindWithTag("Player")?.transform;

//        rb = GetComponent<Rigidbody>();
//        anim = GetComponent<Animator>();
//        //enemyRenderer = GetComponent<Renderer>(); 


//        if (fireballPrefab == null || fireballSpawnPoint == null)
//        {
//            Debug.LogWarning("Fireball Prefab or Spawn Point is not set in the inspector.");
//        }
//    }

//    public virtual void Update()
//    {

//        // Manage visibility based on player's position
//        UpdateVisibility();

//        switch (currentState)
//        {

//            case EnemyState.Chasing:
//                ChasePlayer();
//                break;
//            case EnemyState.Attacking:
//                AttackPlayer();
//                break;

//        }
//    }

//    private void UpdateVisibility()
//    {
//        Vector3 directionToPlayer = playerTransform.position - transform.position;
//        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer); // Get the angle between forward and direction to player

//        if (angleToPlayer <= 45f) // If the player is in front (within 90 degrees)
//        {
//            enemyRenderer.enabled = true; // Make visible
//        }
//        else
//        {
//            enemyRenderer.enabled = false; // Make invisible
//        }
//    }

//    private void LookForPlayer()
//    {
//        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

//        if (distanceToPlayer <= followDistance)
//        {
//            currentState = EnemyState.Chasing;
//        }
//    }

//    private void ChasePlayer()
//    {
//        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

//        // If within attack range, switch to attack state
//        if (distanceToPlayer <= attackDistance)
//        {
//            currentState = EnemyState.Attacking;
//        }
//        else if (distanceToPlayer <= followDistance)
//        {
//            // If within follow range but not attack range, follow player
//            FollowPlayer();
//        }
//        else
//        {
//            // If the player is too far away, shoot fireballs
//            if (Time.time - lastFireballTime > fireballCooldown)
//            {
//                ShootFireballAtPlayer();
//                lastFireballTime = Time.time;
//            }
//        }
//    }

//    private void FollowPlayer()
//    {
//        Vector3 direction = (playerTransform.position - transform.position).normalized;
//        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
//        rb.linearVelocity = direction * moveSpeed;
//        //anim.SetFloat("Speed", rb.linearVelocity.magnitude); // Set animation speed based on movement
//    }
//    private void AttackPlayer()
//    {
//        // If the player is in attack range, shoot fireballs
//        if (Time.time - lastFireballTime > fireballCooldown)
//        {
//            ShootFireballAtPlayer();
//            lastFireballTime = Time.time;
//        }
//    }

//    private void ShootFireballAtPlayer()
//    {
//        if (fireballPrefab != null && fireballSpawnPoint != null)
//        {
//            GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, fireballSpawnPoint.rotation);
//            Vector3 directionToPlayer = (playerTransform.position - fireball.transform.position).normalized;
//            fireball.GetComponent<Rigidbody>().linearVelocity = directionToPlayer * fireballSpeed;
//        }

//    }

//    public void OnFireballHitPlayer(Collider playerCollider, ControllerColliderHit hit)
//    {
//        if (playerCollider.CompareTag("Player") && hit.collider.name == gameObject.name) 
//        {
//            SceneManager.LoadScene("Level");
//            Debug.Log("Player hit by fireball!");
            
//        }
//    }
//}





