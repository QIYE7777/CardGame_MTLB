using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TradeInManager : Singleton<TradeInManager>
{
    public GameObject glowImage;
    public CardAsset[] InPretradecardAsset;
    public Text ATKPoint;
    public float variationSpeed;
    public int timesForRolling = 1;
    public GameObject arrowsUI;
    public GameObject ATKPointUI;
    public Collider myCollider;

    [HideInInspector]
    public int cardsNumberInTrade;
    float decrementToZeroSpeed;

    public event Action<CardAsset,bool> addCardFromTrade;
    public event Action<int> rollDices;

    private float temporaryATKPoint;
    private bool canZero;
    private bool canAdd;
    private float targetATKPoint;
    private int timesHasRolled;


    private void Update()
    {
        CheckATKPoint();
        if (InPretradecardAsset == null)
        {
            cardsNumberInTrade = 0;
        }
        else
        cardsNumberInTrade = InPretradecardAsset.Length;
        //RefreshATKPoint();
        //ATKPointZeroing();
        //ATKPointAdd(); 
    }
    void CheckATKPoint()
    {
        temporaryATKPoint = Mathf.MoveTowards(temporaryATKPoint, targetATKPoint, variationSpeed * Time.deltaTime);
        RefreshATKPoint();
    }
    public void glowFrame(bool x)
    {
        glowImage.SetActive(x);
    }

    public void AddCardInPretrade(CardAsset c)
    {
        List<CardAsset> InPretradecardAssets = new List<CardAsset>(InPretradecardAsset);
        InPretradecardAssets.Add(c);
        InPretradecardAsset = InPretradecardAssets.ToArray();


        //addATKPoint
        //temporaryATKPoint += c.ATK;
        targetATKPoint = targetATKPoint + c.ATK;
        canAdd = true;
    }

    public void giveAllCardsBack()
    {
        if (InPretradecardAsset == null) return;
        StartCoroutine(giveAllCardsBackIE());
        targetATKPoint = 0;
        canZero = true;
    }

    IEnumerator giveAllCardsBackIE()
    {
        foreach (CardAsset i in InPretradecardAsset)
        {
            addCardFromTrade?.Invoke(i,true);
            yield return new WaitForSeconds(0.5f);
            List<CardAsset> InPretradecardAssets = new List<CardAsset>(InPretradecardAsset);
            InPretradecardAssets.Remove(i);
            InPretradecardAsset = InPretradecardAssets.ToArray();
            yield return new WaitForSeconds(0.05f);
        }
    }

    void RefreshATKPoint()
    {
        ATKPoint.text = Mathf.RoundToInt(temporaryATKPoint).ToString();
    }

    public void RollDiceEmitter()
    {
        if (timesHasRolled >= 1) 
        {
            Debug.Log("你只能投" + timesForRolling + "次骰子");
            return;
        }
        if (temporaryATKPoint < 5)
        {
            Debug.Log("点数不足以roll a dice");
            return;
        }
        //清空PretradecardAsset中的所有东西
        InPretradecardAsset = new CardAsset[0];

        myCollider.enabled = false;
        var i = MathF.Min(5, (float)Math.Floor(temporaryATKPoint / 5));
        rollDices?.Invoke((int)i);
        timesHasRolled++;
        arrowsUI.SetActive(true);
        ATKPointUI.SetActive(false);
    }

    #region 老式修改点数的方法
    
    void ATKPointZeroing()//
    {
        if (!canZero) return;
        //if (temporaryATKPoint >= 1)
        //{
           // temporaryATKPoint--;
            // 使用 MoveTowards 让 currentValue 以恒定速度递减到 endValue
            temporaryATKPoint = Mathf.MoveTowards(temporaryATKPoint, 0, decrementToZeroSpeed * Time.deltaTime);

            RefreshATKPoint();
        if (Mathf.Approximately(temporaryATKPoint,0))
        {
            canZero = false;    // 当 temporaryATKPoint 增加到接近 targetATKPoint 时，停止增加
        }
        //}
    }

    void ATKPointAdd()
    {
        if (!canAdd) return;
        /*if (temporaryATKPoint < targetATKPoint )
        {
            temporaryATKPoint++;
            RefreshATKPoint();
            return;
        }*/
        // 使用 MoveTowards 以恒定速度将 temporaryATKPoint 增加到 targetATKPoint
        temporaryATKPoint = Mathf.MoveTowards(temporaryATKPoint, targetATKPoint, variationSpeed * Time.deltaTime);
        RefreshATKPoint();
        if (Mathf.Approximately(temporaryATKPoint, targetATKPoint))
        {
            canAdd = false;    // 当 temporaryATKPoint 增加到接近 targetATKPoint 时，停止增加
        }
    }
    #endregion
}
