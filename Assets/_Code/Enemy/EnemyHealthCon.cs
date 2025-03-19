using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyHealthCon : MonoBehaviour
{

    [SerializeField, Range (100, 500)] public float maxHealth; // = 100f;
    [SerializeField] public float currentHealth;// = 200;
    public Slider healthSlider;
    EnemyController ec;
    TurretEnemy te;

    public Transform enemyTransform;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            Debug.Log("Slider is active");
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        //if (enemyTransform != null)
        //{
        //    ec = GetComponent<EnemyController>();
        //}

        if (enemyTransform != null)
        {
            ec = enemyTransform.GetComponent<EnemyController>();
            te = enemyTransform.GetComponent<TurretEnemy>();
        }
        else
        {
            ec = GetComponent<EnemyController>();
            te = GetComponent<TurretEnemy>();   // If enemyTransform is not assigned, look on the same GameObject
            Debug.LogError("enemyTransform is not assigned!");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
        {
            Debug.Log("slider is current health");
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

    public void Update()
    {
        healthSlider.value = currentHealth;
    }
    //IEnumerator WaitAndReload()
    //{
    //    yield return new WaitForSeconds(10f);
    //    Debug.Log("Coroutine is triggered!");
    //    // Destroy(gameObject); 
    //}
}



