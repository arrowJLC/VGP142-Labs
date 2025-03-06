using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WaterScript : MonoBehaviour
{
    PlayerController pc;
    PlayerHealthCon ph;

    public Transform playerTransform;

    void Start()
    {
        //pc.anim = playerTransform.GetComponentInChildren<Animator>();
        pc = playerTransform.GetComponent<PlayerController>();
        ph = playerTransform.GetComponent<PlayerHealthCon>();
    }
    void OnTriggerEnter(Collider collider)
    {
        {
            var p = collider.gameObject.GetComponent<PlayerController>();

            if (p != null)
            {
                if (pc != null)
                {
                    pc.anim.SetTrigger("getHit");
                }

                if (ph != null)
                {
                    ph.TakeDamage(30f);
                }

                Debug.Log("Player drowned! Scene reloaded.");
                StartCoroutine(WaitAndReload());

            }
        }
    }

    IEnumerator WaitAndReload()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Level");
    }
}
