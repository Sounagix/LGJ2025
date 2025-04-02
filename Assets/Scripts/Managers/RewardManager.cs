using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum REWARD_TYPE
{
    IRON,
    SILVER,
    GOLD,
    SIZE
}

public static class RewardManagerActions
{
    public static System.Action OnRewardCollected;
}

public class RewardManager : MonoBehaviour
{
    [SerializeField]
    private List<BaseCardSO> _ironRewards = new List<BaseCardSO>();

    [SerializeField]
    private List<BaseCardSO> _silverRewards = new List<BaseCardSO>();

    [SerializeField]
    private List<BaseCardSO> _goldRewards = new List<BaseCardSO>();

    [SerializeField]
    private float _ironChance, _silverChance, _goldChance;

    [SerializeField]
    private float _minRewardChance, _maxRewardChance;

    [SerializeField]
    private GameObject _combatHUDPrefab;

    [SerializeField]
    private GameObject _healingHUDPrefab;

    private BaseCardSO _rewardCard;

    private BaseCardHUD _currentRewardHUD;

    private void OnEnable()
    {
        GameManagerActions.OnGameStateChange?.Invoke(GAME_STATE.REWARD_STATE);
        RewardManagerActions.OnRewardCollected += OnRewardCollected;
        GenerateRewards();
    }

    private void OnDisable()
    {
        GameManagerActions.OnGameStateChange?.Invoke(GAME_STATE.MAP_STATE);
        RewardManagerActions.OnRewardCollected -= OnRewardCollected;
    }

    private void GenerateRewards()
    {
        float random = Random.Range(_minRewardChance, _maxRewardChance);
        if (random <= _goldChance)
        {
            GenerateReward(REWARD_TYPE.IRON);
        }
        else if (random <= _silverChance)
        {
            GenerateReward(REWARD_TYPE.SILVER);
        }
        else if (random <= _ironChance)
        {
            GenerateReward(REWARD_TYPE.GOLD);
        }

        ShowReward();
    }

    private void ShowReward()
    {
        GameObject prefab = null;
        switch (_rewardCard.cARD)
        {
            case CARD_HUD_TYPE.COMBAT:
                prefab = _combatHUDPrefab;
                break;
            case CARD_HUD_TYPE.HEALING:
                prefab = _healingHUDPrefab;
                break;
            case CARD_HUD_TYPE.NULL:
                break;
        }
        _currentRewardHUD = Instantiate(prefab, transform).GetComponent<BaseCardHUD>();
        _currentRewardHUD.transform.localPosition = Vector3.zero;
        _currentRewardHUD.Initialize(_rewardCard);
    }

    private void GenerateReward(REWARD_TYPE rEWARD_TYPE)
    {
        switch (rEWARD_TYPE)
        {
            case REWARD_TYPE.IRON:
                _rewardCard = _ironRewards[Random.Range(0, _ironRewards.Count)];
                break;
            case REWARD_TYPE.SILVER:
                _rewardCard = _silverRewards[Random.Range(0, _silverRewards.Count)];
                break;
            case REWARD_TYPE.GOLD:
                _rewardCard = _goldRewards[Random.Range(0, _goldRewards.Count)];
                break;
        }
    }

    private void BackToMap()
    {
        Destroy(_currentRewardHUD.gameObject);
        _currentRewardHUD = null;
        gameObject.SetActive(false);
    }

    private void OnRewardCollected()
    {
        BackToMap();
    }
}
