using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurretEnemy : MonoBehaviour
{
    public Transform player;
    //public Animator anim;
    Transform target;

    EnemyHealthCon eh;
    public EnemyState state;

    [SerializeField] private AudioClip deathSound;
    public GameObject DeathParticles;
    private AudioSource audioSource;

    public int enemyID;
    public int health;
    public enum EnemyState
    {
        Turret, Death
    }

    public float attackRange = 10f;
    public float maxShootDistance = 10f;
    public float attackCooldown = 2f;
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float projectileSpeed = 10f;

    private bool isAttacking = false;
    private bool hasSpawnedCollectibles = false;

    private float lastAttackTime;

    public List<Transform> spawnPoints;
    public List<GameObject> dropsPrefabs;

    private void Start()
    {
        //anim = GetComponent<Animator>();
       
        eh = GetComponent<EnemyHealthCon>();
        audioSource = GetComponent<AudioSource>();

        lastAttackTime = -attackCooldown;

        float health = eh.currentHealth;
    }

    void FixedUpdate()
    {
        if (!player) return;

        switch (state)
        {
            case EnemyState.Turret:
                TurretBehavior();
                break;

            case EnemyState.Death:
                DeathBehavior();
                break;
        }
    }

    private void TurretBehavior()
    {
        target = player.transform;

        CheckForAttack();
        CheckForDeath();

        float distance = Vector3.Distance(transform.position, target.position);
        Vector3 direction = (target.position - transform.position).normalized;

        //Quaternion targetRotation = Quaternion.LookRotation(direction);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    private void DeathBehavior()
    {
        if (!hasSpawnedCollectibles)
        {
           
            hasSpawnedCollectibles = true;
            Instantiate(DeathParticles, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            SpawnCollectibles();          
            StartCoroutine(WaitAndReload());
        }

        target = null;

    }

    private void CheckForAttack()
    {
        float distance = Vector3.Distance(transform.position, player.position);

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
        }
    }

    
    void SpawnCollectibles()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {

            GameObject collectible = dropsPrefabs[Random.Range(0, dropsPrefabs.Count)];

            Instantiate(collectible, spawnPoint.position, Quaternion.identity);
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

   
    private IEnumerator WaitAndReload()
    {
        //audioSource.PlayOneShot(deathSound);
        yield return new WaitForSeconds(5f);
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

        isAttacking = true;

       // anim.SetTrigger("Fire");
        lastAttackTime = Time.time;

        if (projectilePrefab != null && shootPoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
          
            Vector3 direction = (player.position - shootPoint.position).normalized;
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = direction * projectileSpeed;
            }
        }
        yield return new WaitForSeconds(attackCooldown);

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

    // Function called when loading is complete
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
