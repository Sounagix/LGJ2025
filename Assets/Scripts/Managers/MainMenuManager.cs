using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private Button _playButton, _creditButton, _quitButton;

    private void Awake()
    {
        _playButton.onClick.AddListener(
            delegate ()
            {
                SceneManager.LoadScene((int)SCENES.GAME);
            });
        _creditButton.onClick.AddListener(
            delegate ()
            {
                SceneManager.LoadScene((int)SCENES.CREDITS);
            });
        _quitButton.onClick.AddListener(
            delegate ()
            {
                Application.Quit();
            });
    }
}
