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

    #region ����ʼ����
    //����ʼ����
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
        Cursor.lockState = CursorLockMode.Locked; // �������λ��
        Cursor.visible = false;                   // ���������
        Debug.Log("GameOver");
        yield return new WaitForSeconds(1.5f);
        Retreat();
        Cursor.lockState = CursorLockMode.None; // ����������
        Cursor.visible = true;                  // ��ʾ�����
    }
    public void Retreat()
    {
        RoomSwitcher.Instance.BackToUI();
        Destroy(dontDestroy);
    }
}
