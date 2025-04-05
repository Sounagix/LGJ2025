using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private Button _playButton, _fullScreenButton, _quitButton;

    private void Awake()
    {
        _playButton.onClick.AddListener(
            delegate ()
            {
                SceneManager.LoadScene((int)SCENES.INIT_SCENE);
            });
        _fullScreenButton.onClick.AddListener(
            delegate ()
            {
                Screen.fullScreen = !Screen.fullScreen;
            });
        _quitButton.onClick.AddListener(
            delegate ()
            {
                Application.Quit();
            });
    }
}
