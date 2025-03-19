using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    //private static GameManager instance = null;
    //private static LoadSaveManager statemanager = null;
    public static GameManager Instance
    {
        get
        {
            if (!instance)
                instance = new GameObject("GameManager").AddComponent<GameManager>();
            return instance;
        }
    }

    public static LoadSaveManager StateManager
    {
        get
        {
            if (!statemanager)
                statemanager = instance.GetComponent<LoadSaveManager>();

            return statemanager;
        }
    }

    void Awake()
    {
        //Check if there is an existing instance of this object
        if ((instance) && (instance.GetInstanceID() != GetInstanceID()))
            Destroy(gameObject); //Delete duplicate
        else
        {
            instance = this; //Make this object the only instance
            DontDestroyOnLoad(gameObject); //Set as do not destroy
        }
    }
    private static GameManager instance = null;
    private static LoadSaveManager statemanager = null;

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SaveGame()
    {
        // Print the path where the XML is save
        Debug.Log(Application.persistentDataPath);

        // Call save game functionality
        StateManager.Save(Application.persistentDataPath + "/SaveGame.xml");
    }

    // Load Game
    public void LoadGame()
    {

        StateManager.Load(Application.persistentDataPath + "/SaveGame.xml");
    }

}
