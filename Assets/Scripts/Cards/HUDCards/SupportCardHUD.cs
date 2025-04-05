using UnityEngine;
using UnityEngine.EventSystems;

public class SupportCardHUD : BaseCardHUD
{

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (blocked) return;
        if (!_cardOnGame)
            CombatActions.OnDropSupportCard?.Invoke(this);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        switch (GameManager.Instance.GetGAME_STATE())
        {
            case GAME_STATE.SELECTION_STATE:
                Player.Instance.AddMainCard(_baseCardSO);
                GameManager.Instance.LoadScene(SCENES.GAME);
                break;
            case GAME_STATE.COMBAT_STATE:
                if (_selected)
                {
                    _selected = false;
                    _cardImg.color = Color.white;
                    transform.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                else
                {
                    _selected = true;
                    _cardImg.color = Color.gray;
                    transform.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                }
                //CombatActions.OnCardOGSelected?.Invoke(this);
                break;
            case GAME_STATE.REWARD_STATE:
                Player.Instance.AddCardToDeck(_baseCardSO);
                RewardManagerActions.OnRewardCollected?.Invoke();
                break;
            case GAME_STATE.NULL:
                break;
        }
    }
}
