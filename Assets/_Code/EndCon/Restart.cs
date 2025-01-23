using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class CanvasManager : MonoBehaviour
{
    [Header("Button")]
    public Button playButton;
    public Button quitButton;
   

    [Header("Menus")]
    public GameObject gameMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        //Button Bindings
        if (quitButton) quitButton.onClick.AddListener(QuitGame);
        if (playButton) playButton.onClick.AddListener(() => SceneManager.LoadScene("Level"));
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}

   