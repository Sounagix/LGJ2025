using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SCENES: int
{
    MAIN_MENU = 0,
    CREDITS = 1,
    GAME = 2,
    COMBAT = 3,
    REWARD = 4,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(SCENES scene)
    {
        SceneManager.LoadScene((int)scene);
    }

    public void LoadAditiveScene(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
    }

    public void UnloadScene(string scene)
    {
        SceneManager.UnloadSceneAsync(scene);
    }
}
