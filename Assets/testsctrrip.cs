//using UnityEngine;

//public class EnemyFollow : MonoBehaviour
//{
//    public Transform player;  // Reference to the player's Transform
//    public float walkSpeed = 6f;  // Speed at which the enemy walks 
//    public float rotationSpeed = 5f;  // Speed at which the enemy rotates
//    private Animator animator;  // Reference to the Animator component

//      // Distance at which the enemy starts running
//    public float walkDistance = 10f;  // Distance at which the enemy starts walking (or follows)
//    public float sightRange = 15f;  // How far the enemy can "see" the player
//    public LayerMask obstacleLayer;  // Layer mask to detect obstacles in line of sight

//    void Start()
//    {
//        // Get the Animator component from the enemy object
//        animator = GetComponent<Animator>();
//    }

//    void Update()
//    {
//        // Check if player is assigned
//        if (player != null)
//        {
//            // Calculate the direction from the enemy to the player
//            Vector3 direction = player.position - transform.position;
//            direction.y = 0;  // Keep the enemy on the same vertical plane (flat ground)

//            // Calculate the distance from the enemy to the player
//            float distanceToPlayer = direction.magnitude;

//            // Rotate the enemy to face the player smoothly
//            Quaternion targetRotation = Quaternion.LookRotation(direction);
//            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

//            // Check if the player is within sight range and if there's no obstacle in the way
//            bool playerInSight = IsPlayerInSight();

//            // Debug log to check if player is in sight
//            Debug.Log("Player in sight: " + playerInSight);

            
//            if (playerInSight && distanceToPlayer <= walkDistance)  // If player is within sight and walking distance
//            {
//                // Walk towards the player
//                transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);

//                // Set walking animation if speed is greater than 0.1f
//                if (walkSpeed > 0.1f)
//                {
//                    animator.SetBool("isWalking", true);
                    
//                }
//            }
//            else  // If the player is not in sight or too far away, stop moving
//            {
//                transform.Translate(Vector3.zero);  // Ensure no movement when idle
//                animator.SetBool("isWalking", false);
                
//            }
//        }
//    }

//    // Function to check if the player is within sight and not blocked by obstacles
//    private bool IsPlayerInSight()
//    {
//        // Cast a ray from the enemy to the player
//        RaycastHit hit;
//        Vector3 directionToPlayer = player.position - transform.position;
//        if (Physics.Raycast(transform.position, directionToPlayer, out hit, sightRange, obstacleLayer))
//        {
//            // If the ray hits something other than the player, the player is not in sight
//            if (hit.collider.transform != player)
//            {
//                return false;
//            }
//        }
//        return true;  // The player is in sight
//    }
//}
