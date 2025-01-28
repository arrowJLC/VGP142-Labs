//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//public class Projectile : MonoBehaviour
//{
//    [SerializeField] private int damage = 1;
//    [SerializeField, Range(1, 50)] private float lifetime = 5f;
//    [SerializeField] private float detectionRadius = 10f;  // The range within which the player is detected
//    [SerializeField] private float shootDelay = 1f; // Delay between detection and shooting
//    [SerializeField] private float invisibilityDistance = 15f;  // Distance at which the enemy becomes invisible if not looking at the player

//    // Define an event for collision with the player
//    public delegate void PlayerCollisionHandler(Collider playerCollider);
//    public event PlayerCollisionHandler OnCollisionWithPlayer;

//    private Transform playerTransform;
//    private bool playerInRange = false;
//    private bool enemyLookingAtPlayer = true;

//    // Start is called before the first frame update
//    void Start()
//    {
//        if (damage <= 0) damage = 1;
//        Destroy(gameObject, lifetime);  // Destroys the projectile after 'lifetime' seconds

//        // Find the player in the scene
//        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

//        // Start the coroutine to check for player proximity
//        StartCoroutine(CheckPlayerProximity());
//    }

//    // Method to set the velocity of the projectile
//    public void SetVelocity(Vector2 velocity)
//    {
//        GetComponent<Rigidbody>().linearVelocity = velocity; // Set the Rigidbody's velocity
//    }

//    private void Update()
//    {
//        if (playerInRange && enemyLookingAtPlayer)
//        {
//            // Shoot the projectile when the player is near and the enemy is looking at them
//            ShootProjectile();
//        }
//        else
//        {
//            // Make the enemy invisible if it's not looking at the player or if the player is far
//            SetEnemyVisibility(false);
//        }
//    }

//    // Method to check for player proximity and whether the enemy is looking at the player
//    private IEnumerator CheckPlayerProximity()
//    {
//        while (true)
//        {
//            if (playerTransform != null)
//            {
//                float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
//                playerInRange = distanceToPlayer <= detectionRadius;

//                // Check if the enemy is facing the player
//                Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
//                float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);  // Dot product to determine visibility

//                enemyLookingAtPlayer = dotProduct > 0.5f;  // If the dot product is positive, enemy is facing the player

//                // If the enemy is not facing the player and is within a certain distance, set them invisible
//                if (distanceToPlayer <= invisibilityDistance && !enemyLookingAtPlayer)
//                {
//                    SetEnemyVisibility(false);
//                }
//                else
//                {
//                    SetEnemyVisibility(true);
//                }
//            }

//            yield return new WaitForSeconds(0.1f);  // Check every 0.1 second
//        }
//    }

//    // Method to set the enemy's visibility
//    private void SetEnemyVisibility(bool visible)
//    {
//        Renderer enemyRenderer = GetComponent<Renderer>();
//        if (enemyRenderer != null)
//        {
//            enemyRenderer.enabled = visible;  // Enable or disable the enemy's renderer
//        }
//    }

//    // Shoot the projectile (this can be customized based on specific mechanics)
//    private void ShootProjectile()
//    {
//        // Create a new projectile instance and set its velocity
//        Projectile newProjectile = Instantiate(this, transform.position, Quaternion.identity);
//        Vector2 velocity = (playerTransform.position - transform.position).normalized * 10f; // Shoot towards the player
//        newProjectile.SetVelocity(velocity);

//        // Trigger the event when the projectile is fired
//        OnCollisionWithPlayer?.Invoke(playerTransform.GetComponent<Collider>());
//    }

//    // Handle collisions with the player
//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.collider.CompareTag("Player"))
//        {
//            // Trigger the event when hitting the player
//            OnCollisionWithPlayer?.Invoke(collision.collider);
//            Destroy(gameObject);  // Destroy the projectile after collision
//        }
//    }
//}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField, Range(1, 50)] private float lifetime;

 
    public delegate void PlayerCollisionHandler(Collider playerCollider);
    public event PlayerCollisionHandler OnCollisionWithPlayer;

    // Start is called before the first frame update
    void Start()
    {
        if (damage <= 0) damage = 1;
        Destroy(gameObject, lifetime); 
    }

    
    public void SetVelocity(Vector2 velocity)
    {
        GetComponent<Rigidbody>().linearVelocity = velocity;
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.collider.CompareTag("Player"))
        {
           
            OnCollisionWithPlayer?.Invoke(collision.collider);

            
            Destroy(gameObject);
        }

         void HandlePlayerCollision(Collider playerCollider)
        {
            if (playerCollider.CompareTag("Player"))
            {
                SceneManager.LoadScene("Level");
                Debug.Log("Player hit by projectile! Scene reloaded.");
            }
        }
        
    }
}

