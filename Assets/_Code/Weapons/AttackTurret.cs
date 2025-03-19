using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AttackTurret : MonoBehaviour
{
    TurretEnemy te;
    EnemyHealthCon eh;

    private AudioSource audioSource;
    public AudioClip hitSound;

    private List<EnemyHealthCon> enemyHealths = new List<EnemyHealthCon>();
    private List<TurretEnemy> turretEnemies = new List<TurretEnemy>();

    public List<Transform> enemyTransforms;


    //public List<Transform> enemyTransform;
    private void Start()
    {
        foreach (Transform enemy in enemyTransforms)
        {
            
            EnemyHealthCon eh = enemy.GetComponent<EnemyHealthCon>();
            TurretEnemy te = enemy.GetComponent<TurretEnemy>();
            audioSource = GetComponent<AudioSource>();

            if (te != null) turretEnemies.Add(te);
            if (eh != null) enemyHealths.Add(eh);
        }
      
    }

    void OnTriggerEnter(Collider collider)
    {

        var t = collider.gameObject.GetComponent<TurretEnemy>();
        if (t != null)
        {
            foreach (var ec in turretEnemies)
            {
                if (collider.gameObject == ec.gameObject)
                {
                    if (enemyHealths.Count > 0)
                    {
                        foreach (var eh in enemyHealths)
                        {
                            eh.TakeDamage(10f);
                            audioSource.PlayOneShot(hitSound);
                            Debug.Log("Enemy Hit 10 dam");
                        }
                    }
                    if (ec != null)
                    {  
                        Debug.Log("Enemy Hit");
                    }
                }

            }
        }
    }
}
