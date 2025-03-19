using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Pickups : MonoBehaviour
{
    public enum PickupType
    {
        Life,
        winPill,
        Grow,
        Shrink
    }

    public PickupType type;
    CapsuleCollider cc;
    PlayerHealthCon ph;
    private AudioSource audioSource;
    public AudioClip attackSound;

    public GameObject onPickupParticles;

    public List<Transform> spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CapsuleCollider>();
        ph = GetComponent<PlayerHealthCon>();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();

            switch (type)
            {
                case PickupType.Life:
                    //ph.Heal(30f);
                    pc.healPlayer();
                    break;
                case PickupType.winPill:
                    pc.winCondition();
                    break;
                case PickupType.Grow:
                    pc.growPowerUp();
                    break;
                case PickupType.Shrink:
                     pc.shrinkPowerUp();
                    break;
            }
            cc.enabled = false;
            
            Instantiate(onPickupParticles, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            StartCoroutine(PlaySound());
            Destroy(gameObject);

        }
    }

    private IEnumerator PlaySound()
    {
        yield return null;
        audioSource.PlayOneShot(attackSound);
    }
}
