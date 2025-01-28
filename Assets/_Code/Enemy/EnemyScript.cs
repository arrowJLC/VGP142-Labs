//using NUnit.Framework;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;
//using UnityEngine.SceneManagement;

//public enum EnemyState
//{
//    Idle,
//    Patrolling,
//    Chasing,
//    Attacking,
//    Dead
//}

//public class Enemy : MonoBehaviour
//{
//    protected int health;
//    [SerializeField] protected int maxHealth;
//    protected EnemyState currentState;

//    private Transform playerTransform;
//    private float followDistance = 10f;  // Distance at which the enemy will follow the player
//    private float attackDistance = 20f;   // Distance within which the enemy will shoot fireballs
//    private float fireballCooldown = 2f; // Cooldown between fireball shots
//    private float lastFireballTime = 0f;

//    private Rigidbody rb; // Rigidbody for movement control
//    [SerializeField] private float moveSpeed = 3f; // Movement speed

//    Animator anim;

//    // Fireball-related variables
//    [SerializeField] private GameObject fireballPrefab;
//    [SerializeField] private Transform fireballSpawnPoint;
//    [SerializeField] private float fireballSpeed = 10f;
//    private Renderer enemyRenderer;

//    // Patrol-related variables
//    float timer;
//    List<Transform> barrierPoints = new List<Transform>();
//    NavMeshAgent agent;

//    // Distance threshold for attack fire rate
//    private float timeSinceLastFire = 0;

//    public virtual void Start()
//    {
//        health = maxHealth;
//        currentState = EnemyState.Idle;
//        playerTransform = GameObject.FindWithTag("Player")?.transform;

//        rb = GetComponent<Rigidbody>(); // Get Rigidbody component
//        anim = GetComponent<Animator>(); // Get Animator component
//        enemyRenderer = GetComponent<Renderer>(); // Get Renderer component for visibility

//        agent = GetComponent<NavMeshAgent>(); // For patrol pathfinding

//        if (fireballPrefab == null || fireballSpawnPoint == null)
//        {
//            Debug.LogWarning("Fireball Prefab or Spawn Point is not set in the inspector.");
//        }

//        // Initialize patrol points (optional, adjust as needed)
//        GameObject bp = GameObject.FindGameObjectWithTag("Barrier");
//        foreach (Transform t in bp.transform)
//            barrierPoints.Add(t);
//    }

//    public virtual void Update()
//    {
//        if (health <= 0)
//        {
//            currentState = EnemyState.Dead;
//        }

//        // Manage visibility based on player's position
//        UpdateVisibility();

//        switch (currentState)
//        {
//            case EnemyState.Idle:
//                LookForPlayer();
//                break;
//            case EnemyState.Patrolling:
//                Patrol();
//                LookForPlayer();
//                break;
//            case EnemyState.Chasing:
//                ChasePlayer();
//                break;
//            case EnemyState.Attacking:
//                AttackPlayer();
//                break;
//            case EnemyState.Dead:
//                HandleDeath();
//                break;
//        }
//    }

//    private void UpdateVisibility()
//    {
//        Vector3 directionToPlayer = playerTransform.position - transform.position;
//        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer); // Get the angle between forward and direction to player

//        if (angleToPlayer <= 90f) // If the player is in front (within 90 degrees)
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

//    private void Patrol()
//    {
//        if (agent.remainingDistance <= agent.stoppingDistance)
//        {
//            agent.SetDestination(barrierPoints[Random.Range(0, barrierPoints.Count)].position);
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

//    private void HandleDeath()
//    {
//        anim.SetBool("IsDead", true);
//        GetComponent<NavMeshAgent>().enabled = false;
//        GetComponent<Collider>().enabled = false;
//        Destroy(gameObject, 2f); // Adjust based on how long the death animation lasts
//    }

//    public virtual void TakeDamage(int damageValue)
//    {
//        health -= damageValue;
//        if (health <= 0)
//        {
//            currentState = EnemyState.Dead;
//        }
//    }

//    // The following method will be used for detecting and handling the fireball hitting the player
//    public void OnFireballHitPlayer(Collider playerCollider)
//    {
//        if (playerCollider.CompareTag("Player"))
//        {
//            SceneManager.LoadScene("Level");
//            Debug.Log("Player hit by fireball!");
//            // Implement damage to player or game restart logic
//        }
//    }
//}









////using NUnit.Framework;
////using System.Collections.Generic;
////using System.Threading;
////using UnityEngine;
////using UnityEngine.AI;

////public enum EnemyState
////{
////    Idle,
////    Patrolling,
////    Chasing,
////    Attacking,
////    Dead
////}

////public class Enemy : MonoBehaviour
////{
////    protected int health;
////    [SerializeField] protected int maxHealth;
////    protected EnemyState currentState;

////    private Transform playerTransform;
////    private float followDistance = 30f;
////    private float attackDistance = 2f;
////    private float attackCooldown = 1f;
////    private float lastAttackTime = 0f;

////    private Rigidbody rb; // Rigidbody for movement control
////    [SerializeField] private float moveSpeed = 10f; // Movement speed

////    Animator anim;

////    float timer;
////    List<Transform> barrierPoints = new List<Transform>();
////    NavMeshAgent agent;

////    public virtual void Start()
////    {
////        //agent = anim.GetComponent<NavMeshAgent>();
////        health = maxHealth;
////        currentState = EnemyState.Idle;
////        playerTransform = GameObject.FindWithTag("Player")?.transform;

////        rb = GetComponent<Rigidbody>(); // Get Rigidbody component
////        anim = GetComponent<Animator>(); // Get Animator component

////        agent.SetDestination(barrierPoints[Random.Range(0, barrierPoints.Count)].position);
////    }

////    public virtual void Update()
////    {
////        if (health <= 0)
////        {
////            currentState = EnemyState.Dead;
////        }

////        switch (currentState)
////        {
////            case EnemyState.Idle:
////                // Idle behavior
////                LookForPlayer();
////                break;
////            case EnemyState.Patrolling:
////                // Patrolling behavior
////                Patrol();
////                LookForPlayer();
////                break;
////            case EnemyState.Chasing:
////                // Chasing behavior
////                ChasePlayer();
////                break;
////            case EnemyState.Attacking:
////                // Attacking behavior
////                AttackPlayer();
////                break;
////            case EnemyState.Dead:
////                // Handle death (disable colliders, etc.)
////                Destroy(gameObject, 2f); // Or trigger death animation
////                break;
////        }
////    }

////    private void LookForPlayer()
////    {
////        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

////        if (distanceToPlayer <= followDistance)
////        {
////            currentState = EnemyState.Chasing;
////        }
////    }

////    private void Patrol()
////    {
////        timer = 0;
////        barrierPoints.Clear();
////        GameObject bp = GameObject.FindGameObjectWithTag("Barrier");
////        foreach (Transform t in bp.transform)
////            barrierPoints.Add(t);

////        //if (agent.remainingDistance <= agent.stoppingDistance)
////        //    agent.SetDestination(barrierPoints[Random.Range(0, barrierPoints.Count)].position);
////        //UpdateAnimationState();

////        //timer = 0;
////        //GameObject bp = GameObject.FindGameObjectWithTag("Barrier");
////        //foreach(Transform t in bp.transform)
////        //    barrierPoints.Add(t);

////        //if (agent.remainingDistance <= agent.stoppingDistance)
////        //    agent.SetDestination(barrierPoints[Random.Range(0, barrierPoints.Count)].position);



////        //AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);
////        //if (curPlayingClips[0].clip.name.Contains("TigerStates"))
////        //{
////        //    rb.velocity = (sr.flipX) ? new Vector2(-xVel, rb.velocity.y) : new Vector2(xVel, rb.velocity.y);
////        //}

////        //timer += Time.deltaTime;
////        //if(timer > 20) curPlayingClips[0].clip.name.Contains("TigerStates");

////        //// Logic for moving between points or wandering around
////        //// When the player is detected, change to Chasing state
////    }

////    private void ChasePlayer()
////    {
////        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

////        // When the enemy is close enough, switch to attacking
////        if (distanceToPlayer <= attackDistance)
////        {
////            currentState = EnemyState.Attacking;
////        }
////        // If the player moves out of the follow range, return to patrolling
////        else if (distanceToPlayer > followDistance)
////        {
////            currentState = EnemyState.Patrolling;
////        }

////        // Move towards the player
////        Vector3 direction = (playerTransform.position - transform.position).normalized;

////        // Rotate the enemy to always face the player
////        Quaternion lookRotation = Quaternion.LookRotation(direction);
////        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smooth rotation

////        // Move towards the player using velocity
////        rb.linearVelocity = direction * moveSpeed; // Adjust with movement speed

////        // Set animation based on movement velocity
////        anim.SetFloat("Speed", rb.linearVelocity.magnitude); // Set the speed parameter for the animator

////        //agent.SetDestination(playerTransform.position);

////        //UpdateAnimationState();

////    }

////    private void AttackPlayer()
////    {
////        float timeSinceLastAttack = Time.time - lastAttackTime;

////        // Check if enough time has passed for the next attack
////        if (timeSinceLastAttack > attackCooldown)
////        {
////            lastAttackTime = Time.time;
////           // playerTransform.GetComponent<PlayerController>().TakeDamage(10);
////            // Add attack logic (e.g., deal damage to player)
////            Debug.Log("Enemy Attacks Player!");
////            // Example: call a TakeDamage method on the player
////        }
////    }

////    public virtual void TakeDamage(int damageValue)
////    {
////        health -= damageValue;
////        if (health <= 0)
////        {
////            currentState = EnemyState.Dead;
////        }
////    }

////    private void HandleDeath()
////    {
////        anim.SetBool("IsDead", true);
////        GetComponent<NavMeshAgent>().enabled = false;
////        GetComponent<Collider>().enabled = false;
////        Destroy(gameObject, 2f); // Adjust this based on how long you want the death animation to last
////    }

////    //private void UpdateAnimationState()
////    //{
////    //    //float speed = agent.velocity.magnitude;

////    //    // Idle if the speed is very low
////    //    if (speed == 0)
////    //    {
////    //        anim.SetFloat("Speed", 0);
////    //    }
////    //    // Walking animation if the speed is moderate
////    //    else if (speed > 0 && speed < moveSpeed / 2)
////    //    {
////    //        anim.SetFloat("Speed", 0.5f); // Set a value for walking
////    //    }
////    //    // Running animation if the speed is high
////    //    else if (speed >= moveSpeed / 2)
////    //    {
////    //        anim.SetFloat("Speed", 1); // Set a value for running
////    //    }
////    //}
////}

/////*using UnityEngine;

////public enum EnemyState
////{
////    Idle,
////    Patrolling,
////    Chasing,
////    Attacking,
////    Dead
////}

////public class Enemy : MonoBehaviour
////{
////    protected int health;
////    [SerializeField] protected int maxHealth;
////    protected EnemyState currentState;

////    private Transform playerTransform;
////    private float followDistance = 30f;
////    private float attackDistance = 2f;
////    private float attackCooldown = 1f;
////    private float lastAttackTime = 0f;

////    private Rigidbody rb; // Rigidbody for movement control
////    [SerializeField] private float moveSpeed = 3f; // Movement speed

////    Animator anim;

////    // Boo-like variables
////    [SerializeField] private GameObject fireballPrefab;
////    [SerializeField] private Transform fireballSpawnPoint;
////    [SerializeField] private float fireballSpeed = 10f;
////    private Renderer renderer;

////    public virtual void Start()
////    {
////        health = maxHealth;
////        currentState = EnemyState.Idle;
////        playerTransform = GameObject.FindWithTag("Player")?.transform;

////        rb = GetComponent<Rigidbody>(); // Get Rigidbody component
////        anim = GetComponent<Animator>(); // Get Animator component
////        renderer = GetComponent<Renderer>(); // Get Renderer component for visibility

////        if (fireballPrefab == null || fireballSpawnPoint == null)
////        {
////            Debug.LogWarning("Fireball Prefab or Spawn Point is not set in the inspector.");
////        }
////    }

////    public virtual void Update()
////    {
////        if (health <= 0)
////        {
////            currentState = EnemyState.Dead;
////        }

////        switch (currentState)
////        {
////            case EnemyState.Idle:
////                // Idle behavior
////                LookForPlayer();
////                break;
////            case EnemyState.Patrolling:
////                // Patrolling behavior
////                Patrol();
////                LookForPlayer();
////                break;
////            case EnemyState.Chasing:
////                // Chasing behavior
////                ChasePlayer();
////                break;
////            case EnemyState.Attacking:
////                // Attacking behavior
////                AttackPlayer();
////                break;
////            case EnemyState.Dead:
////                // Handle death (disable colliders, etc.)
////                Destroy(gameObject, 2f); // Or trigger death animation
////                break;
////        }
////    }

////    private void LookForPlayer()
////    {
////        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

////        // If the player is within the follow distance, start chasing or attacking
////        if (distanceToPlayer <= followDistance)
////        {
////            currentState = EnemyState.Chasing;
////        }
////    }

////    private void Patrol()
////    {
////        // Logic for wandering around. If the player is detected, switch to chasing state
////    }

////    private void ChasePlayer()
////    {
////        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

////        // When the enemy is close enough, switch to attacking
////        if (distanceToPlayer <= attackDistance)
////        {
////            currentState = EnemyState.Attacking;
////        }
////        // If the player moves out of the follow range, return to patrolling
////        else if (distanceToPlayer > followDistance)
////        {
////            currentState = EnemyState.Patrolling;
////        }

////        // Move towards the player
////        Vector3 direction = (playerTransform.position - transform.position).normalized;

////        // Rotate the enemy to always face the player
////        Quaternion lookRotation = Quaternion.LookRotation(direction);
////        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smooth rotation

////        // Determine visibility based on whether the Boo is facing the player or not
////        UpdateVisibility(direction);

////        // Move towards the player using velocity
////        rb.velocity = direction * moveSpeed;

////        // Set animation based on movement velocity
////        anim.SetFloat("Speed", rb.velocity.magnitude);
////    }

////    private void UpdateVisibility(Vector3 direction)
////    {
////        // Determine if the enemy is facing the player or not
////        float dotProduct = Vector3.Dot(transform.forward, direction);

////        if (dotProduct < 0)  // If facing away from player
////        {
////            // Make the Boo invisible
////            renderer.enabled = false;
////        }
////        else  // If facing the player
////        {
////            // Make the Boo visible
////            renderer.enabled = true;
////        }
////    }

////    private void AttackPlayer()
////    {
////        // Check if enough time has passed since the last attack
////        float timeSinceLastAttack = Time.time - lastAttackTime;

////        if (timeSinceLastAttack > attackCooldown)
////        {
////            lastAttackTime = Time.time;
////            ShootFireballAtPlayer();
////        }
////    }

////    private void ShootFireballAtPlayer()
////    {
////        if (fireballPrefab != null && fireballSpawnPoint != null)
////        {
////            // Create the fireball object
////            GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, fireballSpawnPoint.rotation);

////            // Get the direction to the player and apply force to shoot
////            Vector3 directionToPlayer = (playerTransform.position - fireball.transform.position).normalized;
////            fireball.GetComponent<Rigidbody>().velocity = directionToPlayer * fireballSpeed;
////        }

////[SerializeField] private float distThreshold = 5;

////    [SerializeField] private float projectileFireRate = 2;
////    private float timeSinceLastFire = 0;
////    public override void Start()
////    {
////        base.Start();

////        if (projectileFireRate <= 0)
////            projectileFireRate = 2;

////        if (distThreshold <= 0)
////            distThreshold = 5;
////    }

////    private void Update()
////    {
////        if (!GameManager.Instance.PlayerInstance) return;

////        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);

////        sr.flipX = (transform.position.x > GameManager.Instance.PlayerInstance.transform.position.x);

////        float distance = Vector2.Distance(transform.position, GameManager.Instance.PlayerInstance.transform.position);

////        if (distance <= distThreshold)
////        {
////            sr.color = Color.red;
////            if (curPlayingClips[0].clip.name.Contains("Idle"))
////            {
////                if (Time.time >= timeSinceLastFire + projectileFireRate)
////                {
////                    anim.SetTrigger("Fire");
////                    timeSinceLastFire = Time.time;
////                }
////            }
////        }
////        else
////            sr.color = Color.white;
////    }


////}

////    }

////    // Detect when the fireball hits the player
////    public void OnFireballHitPlayer(Collider playerCollider)
////    {
////        if (playerCollider.CompareTag("Player"))
////        {
////            // Trigger player hit behavior, such as restarting the level
////            Debug.Log("Player hit by fireball! Restarting...");
////            // Example: restart the level or disable the player
////            // You can implement a RestartGame() method or similar functionality
////        }
////    }

////    public virtual void TakeDamage(int damageValue)
////    {
////        health -= damageValue;
////        if (health <= 0)
////        {
////            currentState = EnemyState.Dead;
////        }
////    }
////}
////*/



































////////using UnityEngine;

////////public class Enemy : MonoBehaviour
////////{
////////    protected int health;
////////    [SerializeField] protected int maxHealth;

////////    // Player reference
////////    private Transform playerTransform;

////////    // Distance at which the enemy will stop following the player
////////    [SerializeField] private float followDistance = 10f;

////////    // When true, the enemy will hide when the player is looking at them
////////    [SerializeField] private bool makeInvisibleWhenSeen = true;

////////    public virtual void Start()
////////    {
////////        if (maxHealth <= 0) maxHealth = 5;

////////        health = maxHealth;

////////        // Find the player object in the scene
////////        playerTransform = GameObject.FindWithTag("Player")?.transform;
////////    }

////////    public virtual void Update()
////////    {
////////        if (playerTransform != null)
////////        {
////////            FollowPlayer();
////////            CheckPlayerFacingEnemy();
////////        }
////////    }

////////    // Method to make the enemy follow the player
////////    private void FollowPlayer()
////////    {
////////        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

////////        if (distanceToPlayer <= followDistance)
////////        {
////////            Vector3 direction = (playerTransform.position - transform.position).normalized;
////////            transform.position += direction * Time.deltaTime; // Move the enemy towards the player

////////            // Optionally, flip the enemy's facing direction (for 3D or 2D view)
////////            if (direction.x > 0)
////////                transform.rotation = Quaternion.LookRotation(Vector3.forward); // or set appropriate rotation
////////            else
////////                transform.rotation = Quaternion.LookRotation(Vector3.back);
////////        }
////////    }

////////    // Method to check if the player is facing the enemy
////////    private void CheckPlayerFacingEnemy()
////////    {
////////        Vector3 directionToEnemy = transform.position - playerTransform.position;
////////        float angle = Vector3.Angle(playerTransform.forward, directionToEnemy);

////////        if (angle < 45f) // You can adjust this threshold as needed
////////        {
////////            // The player is looking at the enemy
////////            if (makeInvisibleWhenSeen)
////////            {
////////                // Optionally, hide the enemy (you can disable the Renderer or set transparency)
////////                GetComponent<Renderer>().enabled = false;
////////            }
////////        }
////////        else
////////        {
////////            // The player is not facing the enemy
////////            GetComponent<Renderer>().enabled = true;
////////        }
////////    }

////////    // Method to trigger the enemy attack (optional)
////////    private void AttackPlayer()
////////    {
////////        // Trigger attack behavior here
////////    }

////////    // Method to handle taking damage
////////    public virtual void TakeDamage(int damageValue)
////////    {
////////        health -= damageValue;

////////        if (health <= 0)
////////        {
////////            // Trigger death behavior here
////////            Destroy(gameObject); // Or use an animation before destroying
////////        }
////////    }
////////}


//////using UnityEngine;

//////public enum EnemyState
//////{
//////    Idle,
//////    Patrolling,
//////    Chasing,
//////    Attacking,
//////    Dead
//////}

//////public class Enemy : MonoBehaviour
//////{
//////    protected int health;
//////    [SerializeField] protected int maxHealth;
//////    protected EnemyState currentState;

//////    private Transform playerTransform;
//////    private float followDistance = 30f;
//////    private float attackDistance = 2f;
//////    private float attackCooldown = 1f;
//////    private float lastAttackTime = 0f;

//////    Animator anim;

//////    public virtual void Start()
//////    {
//////        health = maxHealth;
//////        currentState = EnemyState.Idle;
//////        playerTransform = GameObject.FindWithTag("Player")?.transform;
//////    }

//////    public virtual void Update()
//////    {

//////        //Vector2 moveVel = new Vector2(velocity.x, velocity.z);
//////       // anim.SetFloat("Blend", moveVel.magnitude);

//////        if (health <= 0)
//////        {
//////            currentState = EnemyState.Dead;
//////        }

//////        switch (currentState)
//////        {
//////            case EnemyState.Idle:
//////                // Idle behavior
//////                LookForPlayer();
//////                break;
//////            case EnemyState.Patrolling:
//////                // Patrolling behavior
//////                Patrol();
//////                LookForPlayer();
//////                break;
//////            case EnemyState.Chasing:
//////                // Chasing behavior
//////                ChasePlayer();
//////                break;
//////            case EnemyState.Attacking:
//////                // Attacking behavior
//////                AttackPlayer();
//////                break;
//////            case EnemyState.Dead:
//////                // Handle death (disable colliders, etc.)
//////                Destroy(gameObject, 2f); // Or trigger death animation
//////                break;
//////        }
//////    }

//////    private void LookForPlayer()
//////    {
//////        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

//////        if (distanceToPlayer <= followDistance)
//////        {
//////            currentState = EnemyState.Chasing;
//////        }
//////    }

//////    private void Patrol()
//////    {
//////        // Logic for moving between points or wandering around
//////        // When the player is detected, change to Chasing state
//////    }

//////    private void ChasePlayer()
//////    {
//////        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
//////        if (distanceToPlayer <= attackDistance)
//////        {
//////            currentState = EnemyState.Attacking;
//////        }
//////        else if (distanceToPlayer > followDistance)
//////        {
//////            currentState = EnemyState.Patrolling; // or idle state
//////        }

//////        // Move towards the player
//////        Vector3 direction = (playerTransform.position - transform.position).normalized;
//////        transform.position += direction * Time.deltaTime; // Adjust with movement speed
//////    }

//////    private void AttackPlayer()
//////    {
//////        float timeSinceLastAttack = Time.time - lastAttackTime;
//////        if (timeSinceLastAttack > attackCooldown)
//////        {
//////            // Perform attack animation and damage
//////            lastAttackTime = Time.time;
//////            Debug.Log("Enemy Attacks Player!");
//////            // Add attack logic (e.g., damage player)
//////        }
//////    }

////{
////    // If you are using 3D objects, change SpriteRenderer to MeshRenderer or another appropriate 3D renderer
////    MeshRenderer mr;

////    // Changed from Vector2 to Vector3 for 3D space
////    public Vector3 initialShotVelocity = Vector3.zero;

////// Fire point is a transform in 3D space
////public Transform firePoint;

////// Optional: Projectile prefab if you're instantiating projectiles
////public GameObject projectilePrefab;

////// Start is called before the first frame update
////void Start()
////{
////    mr = GetComponent<MeshRenderer>(); // Changed for 3D object

////    // Set default initial shot velocity in 3D space if not set
////    if (initialShotVelocity == Vector3.zero)
////    {
////        initialShotVelocity = new Vector3(10.0f, 0.0f, 0.0f); // Example direction in X
////    }

////    Debug.Log($"for {gameObject.name}");
////}

////public void Fire()
////{
////    if (firePoint != null && projectilePrefab != null)
////    {
////        // Instantiate the projectile at the fire point's position and rotation
////        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

////        // Assuming the projectile has a Rigidbody component to apply velocity
////        Rigidbody rb = projectile.GetComponent<Rigidbody>();
////        if (rb != null)
////        {
////            // Set the velocity of the projectile in 3D space
////            rb.velocity = initialShotVelocity;
////        }
////    }
////    else
////    {
////        Debug.LogWarning("FirePoint or ProjectilePrefab is not assigned!");
////    }
////}
////}

//////    public virtual void TakeDamage(int damageValue)
//////    {
//////        health -= damageValue;
//////        if (health <= 0)
//////        {
//////            currentState = EnemyState.Dead;
//////        }
//////    }
//////}
