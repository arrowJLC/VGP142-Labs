using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AttackEffect : MonoBehaviour
{
    EnemyController ec;
    TurretEnemy te;
    EnemyHealthCon eh;
    private AudioSource audioSource;
    public AudioClip hitSound;

    private List<EnemyController> enemyControllers = new List<EnemyController>();
    private List<EnemyHealthCon> enemyHealths = new List<EnemyHealthCon>();
    private List<TurretEnemy> turretEnemies = new List<TurretEnemy>();

    public List<Transform> enemyTransforms;


    //public List<Transform> enemyTransform;
    private void Start()
    {
        foreach (Transform enemy in enemyTransforms)
        {
            EnemyController ec = enemy.GetComponent<EnemyController>();
            EnemyHealthCon eh = enemy.GetComponent<EnemyHealthCon>();
            TurretEnemy te = enemy.GetComponent<TurretEnemy>();
            audioSource = GetComponent<AudioSource>();

            if (ec != null) enemyControllers.Add(ec); 
            if (te != null) turretEnemies.Add(te);
            if(eh != null) enemyHealths.Add(eh);

        }
        //ec = enemyTransform.GetComponent<EnemyController>();
        //eh = enemyTransform.GetComponent<EnemyHealthCon>();
        // te = enemyTransform.GetComponent<TurretEnemy>();

        //Destroy(gameObject, lifetime);
    }

    //void OnTriggerEnter(Collider collider)
    //{
    //    var e = collider.gameObject.GetComponent<EnemyController>();
    //    var t = collider.gameObject.GetComponent<TurretEnemy>();
    //    if (e != null || t != null)
    //    {
    //        foreach (var ec in enemyControllers)
    //        {
    //            if (collider.gameObject == ec.gameObject)
    //            {
    //                if (enemyHealths.Count > 0)
    //                {
    //                    foreach (var eh in enemyHealths)
    //                    {
    //                        eh.TakeDamage(50f);
    //                        audioSource.PlayOneShot(hitSound);
    //                        Debug.Log("Enemy Hit 50 dam");
    //                    }
    //                }

    //                if (ec != null)
    //                {
    //                    ec.anim.SetTrigger("Hit");
    //                    Debug.Log("Enemy Hit");
    //                }
    //            }

    //        }      
    //    }
    //}
    void OnTriggerEnter(Collider collider)
    {
        var e = collider.gameObject.GetComponent<EnemyController>();
        var t = collider.gameObject.GetComponent<TurretEnemy>();

        // Check if the collided object is an enemy or turret
        if (e != null || t != null)
        {
            // If the enemy is found, apply damage only to the hit enemy
            if (e != null) // For regular enemies
            {
                // Apply damage to the hit enemy only
                e.anim.SetTrigger("Hit");  // Trigger the hit animation
                var hitEnemyHealth = collider.gameObject.GetComponent<EnemyHealthCon>();
                if (hitEnemyHealth != null)
                {
                    hitEnemyHealth.TakeDamage(50f);  // Apply 50 damage to the specific enemy
                    audioSource.PlayOneShot(hitSound);  // Play hit sound
                    Debug.Log("Enemy Hit 50 damage");
                }
            }

        }
    }

}


//foreach (var tc in turretEnemies)
//{
//    if (collider.gameObject == tc.gameObject)
//    {
//        if (enemyHealths.Count > 0 && eh != null)
//        {
//            eh.TakeDamage(10f);
//            Debug.Log("Turret Enemy Hit 10 damage");
//        }
//    }
//}
//foreach (var te in turretEnemies)
//{
//    if (collider.gameObject == te.gameObject)
//    {
//        Debug.Log("turret hit");
//        var eh = collider.gameObject.GetComponent<EnemyHealthCon>();
//        if (eh != null)
//        {
//            eh.TakeDamage(10f);
//            Debug.Log("Turret Enemy Hit 10 damage");
//        }

//        if (te != null)
//        {
//            Debug.Log("Turret Enemy Hit");
//        }
//    }
//}

//using UnityEngine;

//public class AttackEffect : MonoBehaviour
//{
//    EnemyController ec;
//    TurretEnemy te;
//    EnemyHealthCon eh;

//    public Transform enemyTransform;

//    private void Start()
//    {
//        // Initially disable the GameObject (effect is not active)
//        gameObject.SetActive(false); // Effect is off until called
//    }

//    public void ActivateEffect()
//    {
//        // Activate the effect
//        gameObject.SetActive(true);
//    }

//    public void DeactivateEffect()
//    {
//        // Deactivate the effect
//        gameObject.SetActive(false);
//    }

//    void OnTriggerEnter(Collider collider)
//    {
//        var e = collider.gameObject.GetComponent<EnemyController>();
//        //var t = collider.gameObject.GetComponent<TurretEnemy>();

//        // If the effect is not active, exit the method
//        if (!gameObject.activeSelf)
//        {
//            return;
//        }

//        if (e != null)
//        {
//            if (ec != null)
//            {
//                ec.anim.SetTrigger("Hit");
//            }

//            if (eh != null)
//            {
//                eh.TakeDamage(50f);
//            }

//            Debug.Log("Enemy Hit");
//        }

//        //if (t != null)
//        //{
//        //    if (te != null)
//        //    {
//        //        eh.TakeDamage(10f);
//        //    }
//        //}
//        //Debug.Log("Turret Hit");
//    }
//}

//if (eh != null)
//{
//    eh.TakeDamage(50f);
//    Debug.Log("Enemy Hit 50 dam");
//}

//if (ec != null)
//{
//    ec.anim.SetTrigger("Hit");
//}

//Debug.Log("Enemy Hit");

////if (te != null)
////{
////    eh.TakeDamage(10f);
////    Debug.Log("Enemy Hit 10 dam");
////}
////Debug.Log("Turret Hit");
