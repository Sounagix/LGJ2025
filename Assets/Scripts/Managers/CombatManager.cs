using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class CombatActions
{
    public static Action<BaseCardHUD> OnDropDragedCard;
} 


public class CombatManager : MonoBehaviour
{
    [SerializeField]
    private Button _backButton;

    private void OnEnable()
    {
        CombatActions.OnDropDragedCard += OnDropDragedCard;
    }

    private void OnDisable()
    {
        CombatActions.OnDropDragedCard -= OnDropDragedCard;
    }

    private void Awake()
    {
        _backButton.onClick.AddListener(() => GameManager.Instance.LoadScene(SCENES.GAME));
    }



    private void OnDropDragedCard(BaseCardHUD hUD)
    {
        //Vector2;
    }
}
