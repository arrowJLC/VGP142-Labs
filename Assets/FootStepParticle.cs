using JetBrains.Annotations;
using UnityEngine;

public class FootStepParticle : MonoBehaviour
{
    //public Transform leftFootPos;
    //public Transform rightFootPos;
    [SerializeField] public GameObject particleEffectPrefab;


    void SpawnFootEffect(GameObject footPos) => Instantiate(particleEffectPrefab, footPos.transform.position, Quaternion.identity);
    

    //void SpawnFootEffect(GameObject footPos)
    //{
    //    Instantiate(particleEffectPrefab, footPos.transform.position, Quaternion.identity);
    //}
}
