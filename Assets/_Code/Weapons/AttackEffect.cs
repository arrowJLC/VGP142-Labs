using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    EnemyController ec;
    EnemyHealthCon eh;

    public Transform enemyTransform;
    private void Start()
    {
        ec = enemyTransform.GetComponent<EnemyController>();
        eh = enemyTransform.GetComponent<EnemyHealthCon>();

        //Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider collider)
    {
        {
            var e = collider.gameObject.GetComponent<EnemyController>();
            if (e != null)
            {
                if (ec != null)
                {
                    ec.anim.SetTrigger("Hit");
                }

                if (eh != null)
                {
                    eh.TakeDamage(50f);
                }

                Debug.Log("Enemy Hit");
   
            }
        }
    }
}
