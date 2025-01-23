using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;


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


