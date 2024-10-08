using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class RoomManager : Singleton<RoomManager>
{
    public GameObject battleArea;
    public GameObject recruitArea;
    public EnemyManager enemyManager;
    public GameObject nextLevelButton;

    public EnemyAsset[] enemyAssets_15;
    public EnemyAsset[] enemyAssets_20;
    public EnemyAsset[] enemyAssets_25;

    public event Action OpenGlowImageFor1;
    public event Action CloseGlowImageFor1;
    private void Start()
    {
        if (GameManager.Instance.handVisualManger.cardNumberInHand <= 0 && RoomSwitcher.Instance.currentRoomIndex == 1)
            GameManager.Instance.DealDefaultCardsWithBreaks();

        ChooseEnemy();
    }
    public void ChooseEnemy()
    {
        if (RoomSwitcher.Instance.currentRoomIndex == 1)
        {
            int randomIndex = UnityEngine.Random.Range(1, enemyAssets_15.Length);
            enemyManager.enemyAsset = enemyAssets_15[randomIndex-1];
            Debug.Log(randomIndex);
        }
        if (RoomSwitcher.Instance.currentRoomIndex == 2)
        {
            int randomIndex = UnityEngine.Random.Range(1, enemyAssets_20.Length);
            enemyManager.enemyAsset = enemyAssets_20[randomIndex-1];
            Debug.Log(randomIndex);
        }
        if (RoomSwitcher.Instance.currentRoomIndex == 3)
        {
            int randomIndex = UnityEngine.Random.Range(1, enemyAssets_25.Length);
            enemyManager.enemyAsset = enemyAssets_25[randomIndex-1];
            Debug.Log(randomIndex);
        }
    }

    public void FromBattleToRecruit()
    {
        StartCoroutine(FromBattleToRecruitIE());
    }
    IEnumerator FromBattleToRecruitIE()
    {
        yield return new WaitForSeconds(1);
        battleArea.transform.DOMoveX(-11.5f, 1);
        recruitArea.transform.DOMoveX(0, 1);
        OpenGlowImageFor1?.Invoke();
    }

    public void FromRecruitToNexrRoom()
    {
        Debug.Log("toNextLevel");
        CloseGlowImageFor1?.Invoke();
        GameObject dontDestroy = GameObject.FindWithTag("DontDestroy");
        var roomSwitcher = dontDestroy.GetComponent<RoomSwitcher>();
        roomSwitcher.SwitchToNextLevel();
    }

    public void GameEnd()
    {
        if(GateManager.Instance.finalPoint < 50)
        {
            Debug.Log("门没开耶，没有收获");
            return;
        }
        if (GateManager.Instance.finalPoint >= 50 && GateManager.Instance.finalPoint <70)
            Debug.Log("门开了，少年。 这次探险的收获是" + GateManager.Instance.finalPoint + "金币");
        if (GateManager.Instance.finalPoint >= 70)
            Debug.Log("门开了，少年。 这次探险的收获是" + GateManager.Instance.finalPoint *2 + "金币");      
    }
}
