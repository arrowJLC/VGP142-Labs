using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class WinConditions : MonoBehaviour
{
    public Transform playerTransform;
    void OnTriggerEnter(Collider collider)
    {
        {
            var p = collider.gameObject.GetComponent<Player>();

            if (p != null)
            {
                SceneManager.LoadScene("WinScreen");
            }
        }
    }
}


