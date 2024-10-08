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

    public void CheckPlayerDead()
    {
        int allCards = handVisualManger.cardNumberInHand + EnemyManager.Instance.InPreBattleGateAsset.Length;
        if (allCards <= 0)
        {
            StartCoroutine(CheckPlayerDeadIE());
        }
    }
    IEnumerator CheckPlayerDeadIE()
    {
        Cursor.lockState = CursorLockMode.Locked; // 锁定鼠标位置
        Cursor.visible = false;                   // 隐藏鼠标光标
        Debug.Log("GameOver");
        yield return new WaitForSeconds(1.5f);
        Retreat();
        Cursor.lockState = CursorLockMode.None; // 解除鼠标锁定
        Cursor.visible = true;                  // 显示鼠标光标
    }
    public void Retreat()
    {
        RoomSwitcher.Instance.BackToUI();
        Destroy(dontDestroy);
    }
}
