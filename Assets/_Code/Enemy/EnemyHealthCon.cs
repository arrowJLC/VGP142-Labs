using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyHealthCon : MonoBehaviour
{

    public float maxHealth = 100f;
    public float currentHealth =100f;
    public Slider healthSlider;
    EnemyController ec;

    public Transform enemyTransform;

    void Start()
    {
        //if (enemyTransform != null)
        //{
        //    ec = GetComponent<EnemyController>();
        //}

        if (enemyTransform != null)
        {
            ec = enemyTransform.GetComponent<EnemyController>();
        }
        else
        {
            ec = GetComponent<EnemyController>();  // If enemyTransform is not assigned, look on the same GameObject
            Debug.LogError("enemyTransform is not assigned!");
        }

        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        //if (currentHealth <= 0)
        //{
        //    Debug.Log("Enemy is dead!");
        //    if (ec != null)
        //    {
        //        //ec.state = EnemyState.Death;
        //    }
        //    StartCoroutine(WaitAndReload());
        //}
    }
    //IEnumerator WaitAndReload()
    //{
    //    yield return new WaitForSeconds(10f);
    //    Debug.Log("Coroutine is triggered!");
    //    // Destroy(gameObject); 
    //}
}



