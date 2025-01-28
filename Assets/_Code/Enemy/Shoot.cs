using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shoot : MonoBehaviour
{
   
    private MeshRenderer mr;

    
    public Vector3 initialShotVelocity = Vector3.zero;

    
    public Transform firePoint;

   
    public GameObject projectilePrefab;

    
    public float projectileLife = 5f;

    
    [SerializeField] private float detectionRadius = 10f;  
    [SerializeField] private float shootDelay = 1f; 
    [SerializeField] private float invisibilityDistance = 15f;  

    
    public delegate void PlayerCollisionHandler(Collider playerCollider);
    public event PlayerCollisionHandler OnCollisionWithPlayer;

    private Transform playerTransform;
    private bool playerInRange = false;
    private bool enemyLookingAtPlayer = true;
    private bool canShoot = true;  

   
    void Start()
    {
        mr = GetComponent<MeshRenderer>(); 

        
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        
        StartCoroutine(CheckPlayerProximity());
    }

   
    public void SetVelocity(Vector2 velocity)
    {
        GetComponent<Rigidbody>().linearVelocity = velocity; 
    }

    private void Update()
    {
        if (playerInRange && enemyLookingAtPlayer && canShoot)
        {
            
            StartCoroutine(DelayShoot());
        }
        else
        {
            
           // SetEnemyVisibility(false);
        }
    }

  
    private IEnumerator CheckPlayerProximity()
    {
        while (true)
        {
            if (playerTransform != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
                playerInRange = distanceToPlayer <= detectionRadius;

               
                Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
                float dotProduct = Vector3.Dot(transform.forward, directionToPlayer); 

                enemyLookingAtPlayer = dotProduct > 0.5f;  

              
            }

            yield return new WaitForSeconds(0.1f); 
        }
    }

    private IEnumerator DelayShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootDelay); 

        
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            
            Vector3 velocity = (playerTransform.position - transform.position).normalized * 10f; 

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = velocity; 
            }

            Projectile newProjectile = projectile.GetComponent<Projectile>();
            if (newProjectile != null)
            {
                newProjectile.OnCollisionWithPlayer += HandlePlayerCollision;
            }

            
            Destroy(projectile, projectileLife);
        }

      
        OnCollisionWithPlayer?.Invoke(playerTransform.GetComponent<Collider>());

       
        canShoot = true;
    }

    
    private void HandlePlayerCollision(Collider playerCollider)
    {
        if (playerCollider.CompareTag("Player"))
        {
            SceneManager.LoadScene("Level");
            Debug.Log("Player hit by projectile! Scene reloaded.");
        }
    }

    
    public void FireProjectile()
    {
        if (firePoint != null && projectilePrefab != null)
        {
            
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            
            Vector3 velocity = (playerTransform.position - transform.position).normalized * 10f;

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = velocity; 
            }

            
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.OnCollisionWithPlayer += HandlePlayerCollision;
            }
        }
    }
}


