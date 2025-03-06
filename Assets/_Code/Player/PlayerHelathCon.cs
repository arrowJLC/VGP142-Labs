using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealthCon : MonoBehaviour
{
    PlayerController pc;
    Animator anim;

    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthSlider; 

    void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        pc = GetComponent<PlayerController>();
        anim = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        { 
            Debug.Log("Player is dead!");
            pc.anim.SetTrigger("getHit");
            pc.canMove = false;
            pc.velocity = Vector3.zero;
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }
}

