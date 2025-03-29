using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GAME_STATE : int
{

}

public class Player : MonoBehaviour
{

    public IEnumerator MoveToPosition(Vector2 target, float duration, BoardManager boardManager)
    {
        float elapsed = 0f;
        Vector2 start = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector2.Lerp(start, target, t);
            yield return null;
        }

        transform.position = target;
        boardManager.OnPlayerMovementFinished();
    }
}
