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

public enum GAME_STATE : int
{
    SELECTION_STATE,
    MAP_STATE,
    COMBAT_STATE,
    REWARD_STATE,
    NULL
}

public static class  GameManagerActions
{
    public static Action<SCENES> OnSceneChange;

    public static Action<GAME_STATE> OnGameStateChange;
}

public class GameManager : MonoBehaviour
{
    private GAME_STATE _gAME_sTATE = GAME_STATE.SELECTION_STATE;

    public static GameManager Instance { get; private set; }

    private void OnEnable()
    {
        GameManagerActions.OnGameStateChange += OnGameStateChange;
    }



    private void OnDisable()
    {
        GameManagerActions.OnGameStateChange -= OnGameStateChange;
    }


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

    private void OnGameStateChange(GAME_STATE sTATE)
    {
        _gAME_sTATE = sTATE;
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

    public void LoadAditiveScene(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
    }

    public void UnloadScene(string scene)
    {
        SceneManager.UnloadSceneAsync(scene);
    }

    public GAME_STATE GetGAME_STATE()
    {
        return _gAME_sTATE;
    }
}
