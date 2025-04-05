using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum REWARD_TYPE
{
    IRON,
    SILVER,
    GOLD,
    BOSS,
    SIZE
}

public static class RewardManagerActions
{
    public static System.Action OnRewardCollected;
}

public class RewardManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _midPanel;

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

    private List<BaseCardSO> _listOfRewardsSO = new();

    private List< BaseCardHUD>_listOfRewardsHUD = new();

    [SerializeField]
    [Min(2)]
    private int _numOfRewardsToChoice;

    private void OnEnable()
    {
        GameManagerActions.OnGameStateChange?.Invoke(GAME_STATE.REWARD_STATE);
        RewardManagerActions.OnRewardCollected += OnRewardCollected;
        for (int i = 0; i < _numOfRewardsToChoice; i++) 
        {
            GenerateRewards();
        }
        foreach (BaseCardSO card in _listOfRewardsSO)
        {
            ShowReward(card);
        }
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
            GenerateReward(REWARD_TYPE.GOLD);
        }
        else if (random <= _silverChance)
        {
            GenerateReward(REWARD_TYPE.SILVER);
        }
        else
        {
            GenerateReward(REWARD_TYPE.IRON);
        }        
    }

    private void ShowReward(BaseCardSO card)
    {
        GameObject prefab = null;
        if (card.cARD.Equals(CARD_HUD_TYPE.COMBAT))
        {
            prefab = _combatHUDPrefab;
        }
        else
        {
            prefab = _healingHUDPrefab;
        }
        BaseCardHUD currentReward = Instantiate(prefab, _midPanel.transform).GetComponent<BaseCardHUD>();
        currentReward.transform.localPosition = Vector3.zero;
        currentReward.Initialize(card);
        _listOfRewardsHUD.Add(currentReward);
    }

    private void GenerateReward(REWARD_TYPE rEWARD_TYPE)
    {
        switch (rEWARD_TYPE)
        {
            case REWARD_TYPE.IRON:
                _listOfRewardsSO.Add(_ironRewards[Random.Range(0, _ironRewards.Count)]);
                break;
            case REWARD_TYPE.SILVER:
                _listOfRewardsSO.Add(_silverRewards[Random.Range(0, _silverRewards.Count)]);
                break;
            case REWARD_TYPE.GOLD:
                _listOfRewardsSO.Add(_goldRewards[Random.Range(0, _goldRewards.Count)]);
                break;
        }
    }

    private void BackToMap()
    {
        Clean();
        gameObject.SetActive(false);
    }

    private void OnRewardCollected()
    {
        BackToMap();
    }

    private void Clean()
    {
        foreach (BaseCardHUD card in _listOfRewardsHUD)
        {
            Destroy(card.gameObject);
        }
        _listOfRewardsSO.Clear();
        _listOfRewardsHUD.Clear();
    }
}
