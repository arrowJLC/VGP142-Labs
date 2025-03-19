
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class WeaponController : MonoBehaviour, ThirdPersonInputs.IOverworldActions
{
    public GameObject SwordPolyart;
    public GameObject DogPolyart;
    //BoxCollider bc;
    public bool CanAttack = true;
    public float AttackCooldown = 1.0f;
    public bool isAttacking = false;

    public GameObject attackEffectPrefab; 
    public float attackEffectDuration = 1.5f;

    private ThirdPersonInputs inputActions;

    void Awake()
    {
        //bc = GetComponent<BoxCollider>();
        inputActions = new ThirdPersonInputs();
        inputActions.Overworld.Enable();
        inputActions.Overworld.SetCallbacks(this);  // Set the callbacks for the actions.
    }

    void OnDestroy()
    {
        inputActions.Overworld.Disable();  // Ensure the input actions are disabled when the object is destroyed.
    }

    
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)  
        {
            if (CanAttack)
            {
                //bc.isTrigger = true;
                SwordAttack();
            }
        }
    }

    public void SwordAttack()
    {
        isAttacking = true;
        CanAttack = false;

       // GameObject attackEffect = Instantiate(attackEffectPrefab, SwordPolyart.transform.position, Quaternion.identity);

       // Destroy(attackEffect, attackEffectDuration);

        Animator anim = DogPolyart.GetComponent<Animator>();
        anim.SetTrigger("Attack");
        StartCoroutine(ResetAttackCooldown());
    }

    IEnumerator ResetAttackCooldown()
    {
        StartCoroutine(ResetAttackBool());
        yield return new WaitForSeconds(AttackCooldown);
        CanAttack = true;

    }

    IEnumerator ResetAttackBool()
    {
        
        yield return new WaitForSeconds(1.0f);
        isAttacking = false;
    }

    public void OnMove(InputAction.CallbackContext context) { }
    public void OnJump(InputAction.CallbackContext context) { }
    public void OnDropWeapon(InputAction.CallbackContext context) { }
    public void OnDefend(InputAction.CallbackContext context) { }
    public void OnPause(InputAction.CallbackContext context) { }
}
