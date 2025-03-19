using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
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
    PlayerHealthCon ph;

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

    public GameObject DeathParticles;

    public List<Transform> spawnPoints;
    public List<GameObject> dropsPrefabs;

    [SerializeField] private AudioClip deathSound;
    private AudioSource audioSource;

    private int enemyID;
    public int health;
    private void Start()
    {

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        eh = GetComponent<EnemyHealthCon>(); 
        ph = GetComponent<PlayerHealthCon>();
        audioSource = GetComponent<AudioSource>();

        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        lastAttackTime = -attackCooldown;

        float health = eh.currentHealth;
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
        //    if (path != null && path.Length > 0)
        //    {
        //        target = path[pathIndex];

        //        // Transition to Chase state if player is within range
        //        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        //        {
        //            state = EnemyState.Chase;
        //        }

        //        // Move to the next patrol point if the agent reaches the current one
        //        if (agent.remainingDistance < distThreshold)
        //        {
        //            pathIndex++;
        //            pathIndex %= path.Length;
        //        }

        //        anim.SetBool("isWalking", true);
        //        anim.SetFloat("Speed", 1);
        //        agent.SetDestination(target.position);
        //        //eh.currentHealth; // = 100f;
        //    }

        //    else MoveRandomly();
        //    Debug.Log("Enemy movinmg at random");



        if (path != null && path.Length > 0)
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
        //else
        //{
        //    // If no path is assigned, move randomly
        //    Debug.Log("No path assigned. Moving randomly.");
        //    MoveRandomly();
        //}
    }

    private void DeathBehavior()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        if (!hasSpawnedCollectibles)
        {
            Instantiate(DeathParticles, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            hasSpawnedCollectibles = true; 
            SpawnCollectibles();
            StartCoroutine(WaitAndReload());
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

    private void MoveRandomly()
    {
        
        Vector3 randomDirection = Random.insideUnitSphere * 10f; 
        randomDirection += transform.position; 

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        anim.SetBool("isWalking", true);
        anim.SetFloat("Speed", 1);
    }
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
            if (deathSound != null)
            {
                audioSource.PlayOneShot(deathSound);
            }

            Debug.Log("Enemy is dead!");
            state = EnemyState.Death;
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Projectile"))
        {
            Debug.Log("Collided with Player Projectile");
            eh.TakeDamage(20f);
        }
    }

    IEnumerator WaitAndReload()
    {
        yield return new WaitForSeconds(10f);
        Debug.Log("Coroutine is triggered!");
        // destroying destroys collectable ... fix first
        Destroy(gameObject); 
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

    public void SaveGamePrepare()
    {
        //Create enemy data for this enemy
        LoadSaveManager.GameStateData.DataEnemy data = new LoadSaveManager.GameStateData.DataEnemy();

        //Fill in data for current enemy
        data.enemyID = enemyID;
        data.health = health;

        data.posRotScale.posX = transform.position.x;
        data.posRotScale.posY = transform.position.y;
        data.posRotScale.posZ = transform.position.z;

        data.posRotScale.rotX = transform.localEulerAngles.x;
        data.posRotScale.rotY = transform.localEulerAngles.y;
        data.posRotScale.rotZ = transform.localEulerAngles.z;

        data.posRotScale.scaleX = transform.localScale.x;
        data.posRotScale.scaleY = transform.localScale.y;
        data.posRotScale.scaleZ = transform.localScale.z;

        //Add enemy to Game State
        GameManager.StateManager.gameState.enemies.Add(data);
    }

    public void LoadGameComplete()
    {
        // Cycle through enemies and find matching ID
        List<LoadSaveManager.GameStateData.DataEnemy> enemies = GameManager.StateManager.gameState.enemies;


        // Reference to this enemy
        LoadSaveManager.GameStateData.DataEnemy dataEnemy = null;

        // not a good idea if the game will have multiple enemies!! maybe use an array? and remove enemies that are dead...

        foreach (LoadSaveManager.GameStateData.DataEnemy enemy in enemies)
        {
            if (enemyID == enemy.enemyID)
            {
                dataEnemy = enemy;
                break;
            }
        }

        // If here and no enemy is found, then it was destroyed when saved. So destroy.
        if (dataEnemy == null)
        {
            Destroy(gameObject);
            return;
        }

        // Else load enemy data
        enemyID = dataEnemy.enemyID;
        health = dataEnemy.health;

        // Set position
        transform.position = new Vector3(dataEnemy.posRotScale.posX, dataEnemy.posRotScale.posY, dataEnemy.posRotScale.posZ);

        // Set rotation
        transform.localRotation = Quaternion.Euler(dataEnemy.posRotScale.rotX, dataEnemy.posRotScale.rotY, dataEnemy.posRotScale.rotZ);

        // Set scale
        transform.localScale = new Vector3(dataEnemy.posRotScale.scaleX, dataEnemy.posRotScale.scaleY, dataEnemy.posRotScale.scaleZ);

        enemies.Remove(dataEnemy);
    }

}

