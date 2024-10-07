using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : Singleton<GameManager>
{
    [Header ("Game Gel Settings")]
    public int defaultNumberofCards;

    [Header("Reference")]
    public DeckManager deckManager;
    public HandVisualManager handVisualManger;
    public GameObject dontDestroy;

    public event Action DealDefaultCards;

    public void DealDefaultCardsWithBreaks()
    {
        StartCoroutine(DealDefaultCardsWithBreaksIE());
    }

    #region 发初始手牌
    //发初始手牌
    IEnumerator DealDefaultCardsWithBreaksIE()
    {
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i<defaultNumberofCards; i++)
        {
            DealDefaultCards?.Invoke();
            yield return new WaitForSeconds(0.5f);
        }
    }
    #endregion

    public void PlayerDead()
    {
        Debug.Log("playerDead");
    }
    public void Retreat()
    {
        RoomSwitcher.Instance.BackToUI();
        Destroy(dontDestroy);
    }
}
