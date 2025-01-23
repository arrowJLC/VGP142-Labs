using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    public enum PickupType
    {
        
    }

    public PickupType type;
    SpriteRenderer sr;
    


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
       
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();

            //switch (type)
            //{
            //    case //PickupType.//Life:
                    
            //        break;
                
            //}
            //sr.enabled = false;
          
        }
    }
}
