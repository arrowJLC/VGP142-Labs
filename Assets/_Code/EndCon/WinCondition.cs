using UnityEngine;
using UnityEngine.SceneManagement;

public class WinConditions : MonoBehaviour
{
    public Transform playerTransform;
    void OnTriggerEnter(Collider collider)
    {
        {
            var p = collider.gameObject.GetComponent<PlayerController>();

            if (p != null)
            {
                SceneManager.LoadScene("WinScreen");
            }
        }
    }
}


