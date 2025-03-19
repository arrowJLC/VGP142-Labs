using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Windows;


public class WeaponShootScript : MonoBehaviour
{
    PlayerController pc;
    EnemyHealthCon eh;
    EnemyController ec;

    public Transform FirePoint;
    public Transform Enemy;
    public GameObject projectilePrefab;

    public float projectSpeed = 10f;

    ThirdPersonInputs inputActions;

    private void Start()
    {
        pc = GetComponent<PlayerController>();
        eh = GetComponent<EnemyHealthCon>();
        ec = GetComponent<EnemyController>();
        inputActions = new ThirdPersonInputs();
    }

    //public void OnAttack(InputAction.CallbackContext context)
    //{
    //    Debug.Log("Fire Triggered");
    //    if (projectilePrefab != null && FirePoint != null)
    //    {
    //        GameObject projectile = Instantiate(projectilePrefab, FirePoint.position, Quaternion.identity);

    //        Vector3 direction = (Enemy.position - FirePoint.position).normalized;
    //        Rigidbody rb = projectile.GetComponent<Rigidbody>();
    //        if (rb != null)
    //        {
    //            rb.linearVelocity = direction * projectSpeed;
    //        }
    //    }

    //}
    public void fireAttack()
    {
        Debug.Log("Fire Triggered");
        if (projectilePrefab != null && FirePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, FirePoint.position, Quaternion.identity);

            Vector3 direction = (Enemy.position - FirePoint.position).normalized;
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = direction * projectSpeed;
            }
        }
    }
}
