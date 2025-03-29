using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SCENES: int
{
    MAIN_MENU   = 0,
    CREDITS     = 1,
    INIT_SCENE  = 2,
    GAME        = 3,
    COMBAT      = 4,
    REWARD      = 5,
}

public static class  GameManagerActions
{
    public static Action<SCENES> OnSceneChange;
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
        if (scene.Equals(SCENES.MAIN_MENU))
        {
            Destroy(Player.Instance.gameObject);
            Destroy(BoardManager.Instance.gameObject);
        }
        GameManagerActions.OnSceneChange?.Invoke(scene);

        SceneManager.LoadScene((int)scene);
    }

    public SCENES GetScene()
    {
        return (SCENES)SceneManager.GetActiveScene().buildIndex;
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
