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

            //SceneManager.LoadScene("Level");
            //        Debug.Log("Player hit by projectile! Scene reloaded.");

            OnCollisionWithPlayer?.Invoke(collision.collider);

            
            Destroy(gameObject);
        }

        // void HandlePlayerCollision(Collider playerCollider)
        //{
        //    if (playerCollider.CompareTag("Player"))
        //    {
        //        SceneManager.LoadScene("Level");
        //        Debug.Log("Player hit by projectile! Scene reloaded.");
        //    }
        //}
        
    }
}

