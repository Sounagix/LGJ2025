using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum REWARD_TYPE
{
    IRON,
    SILVER,
    GOLD
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
    private BaseCardHUD _rewardHUDPrefab;

    [SerializeField]
    private float _timeToBack;

    [SerializeField]
    private Button _backButton;

    private BaseCardSO _rewardCard;

    private void Awake()
    {
        _backButton.onClick.AddListener(
            delegate ()
            {
                SceneManager.UnloadSceneAsync("Map");
            });
        _backButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        GenerateRewards();
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
        BaseCardHUD currentReward = Instantiate(_rewardHUDPrefab, transform);
        currentReward.transform.localPosition = Vector3.zero;
        currentReward.Initialize(_rewardCard);
        StartCoroutine(BackToMap());
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

    private IEnumerator BackToMap()
    {
        yield return new WaitForSeconds(_timeToBack);
        _backButton.gameObject.SetActive(true);
    }
}
