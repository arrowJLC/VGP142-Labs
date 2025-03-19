using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;

public class SaveMenuManager : MonoBehaviour
{
    public GameObject pausePanel;
    public bool gamePaused;

    PlayerController pRef;
    EnemyController eRef;
    TurretEnemy tRef;

    void Start()
    {
        pRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        eRef = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
        tRef = GameObject.FindGameObjectWithTag("Enemy").GetComponent<TurretEnemy>();
    }

    //void OnPause(InputAction.CallbackContext context)
    //{
    //    PauseGame();
    //}
    public void PauseGame()
    {
        gamePaused = !gamePaused;
        pausePanel.SetActive(gamePaused);

        if (gamePaused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void SaveGame()
    {
        pRef.SaveGamePrepare();
        if (eRef != null)
            eRef.LoadGameComplete();
        else
            Debug.LogWarning("EnemyController reference is null.");
        tRef.SaveGamePrepare();

        GameManager.Instance.SaveGame();
    }

    public void LoadGame()
    {
        GameManager.Instance.LoadGame();

        pRef.LoadGameComplete();
        if (eRef != null)
            eRef.LoadGameComplete();
        else
            Debug.LogWarning("EnemyController reference is null.");
        tRef.LoadGameComplete();
    }
}
