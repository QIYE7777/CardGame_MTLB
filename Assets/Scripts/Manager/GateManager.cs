using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GateManager : Singleton<GateManager>
{
    public GameObject glowImage;
    public CardAsset[] InPreOpenGateAsset;
    public Text finalPointUI;
    public float finalPoint;
    public int cardsNumberInGate;
    private float temporaryFinalPoint;
    public float variationSpeed;
    public CardAsset[] arrayForFlush;
    public Text[] visualInGateList;
    private int visualCount = 0;
    private bool isPressButton;

    //public event Action<CardAsset, bool> addCardFromGate;
    private void Update()
    {
        cardsNumberInGate = InPreOpenGateAsset.Length;
        GameManager.Instance.handVisualManger.cardNumberInGate = cardsNumberInGate;
        CheckATKPoint();
    }
    public void glowFrame(bool x)
    {
        glowImage.SetActive(x);
    }
    public void AddCardInPreGateAsset(CardAsset c)
    {
        List<CardAsset> InPreOpenGateAssets = new List<CardAsset>(InPreOpenGateAsset);
        InPreOpenGateAssets.Add(c);
        InPreOpenGateAsset = InPreOpenGateAssets.ToArray();
        VisualInGate(c);
    }

    public void giveAllCardsBack()
    {
        if (InPreOpenGateAsset == null) return;
        if (isPressButton) return;
        isPressButton = true;

        //把visualInGateList清空
        visualCount = 0;
        StartCoroutine(ClearGateText());

        StartCoroutine(giveAllCardsBackIE());
    }
    IEnumerator ClearGateText()
    {
        foreach (Text t in visualInGateList)
        {
            t.text = "  ";
            yield return new WaitForSeconds(0.6f);
        }
        isPressButton = false;
    }
    IEnumerator giveAllCardsBackIE()
    {
        foreach (CardAsset i in InPreOpenGateAsset)
        {
            GameManager.Instance.handVisualManger.AddCardFromGate(i, true);
            yield return new WaitForSeconds(0.5f);
            List<CardAsset> InPretradecardAssets = new List<CardAsset>(InPreOpenGateAsset);
            InPretradecardAssets.Remove(i);
            InPreOpenGateAsset = InPretradecardAssets.ToArray();
            yield return new WaitForSeconds(0.05f);
        }
        isPressButton = false;
    }
    void RefreshATKPoint()
    {
        finalPointUI.text = Mathf.RoundToInt(temporaryFinalPoint).ToString();
    }
    public void calculatePoint()
    {
        //把visualInGateList清空
        visualCount = 0;
        StartCoroutine(ClearGateText());

        finalPoint = CalculatePointTool.FinalPoint(InPreOpenGateAsset) + finalPoint;
        if (IsFlush() && TradeInManager.Instance.cardsNumberInTrade + GameManager.Instance.handVisualManger.cardNumberInHand == 0)//如果手牌加tradein里的牌为零的话(即向大门提交最后一组牌的时候)，再检查同花
        {
            finalPoint = finalPoint * 2;
        }
        //清空InPreOpenGateAsset数组
        InPreOpenGateAsset = new CardAsset[0];
    }
    void CheckATKPoint()
    {
        //temporaryFinalPoint现在显示的点数
        temporaryFinalPoint = Mathf.MoveTowards(temporaryFinalPoint, finalPoint, variationSpeed * Time.deltaTime);
        RefreshATKPoint();
    }

     private bool IsFlush()
    {
        if (arrayForFlush == null)
            arrayForFlush = InPreOpenGateAsset;

        List<CardAsset> list = new List<CardAsset>(arrayForFlush);
        list.AddRange(InPreOpenGateAsset);
        arrayForFlush = list.ToArray();

        CardAsset.suit temporarySuit  = arrayForFlush[0].Suit;
        int count = 0;
        for (int x = 0; x < arrayForFlush.Length; x++)
        {
            if (arrayForFlush[x].Suit == temporarySuit)
                count++;
        }
        if (count == arrayForFlush.Length)
            return true;
        return false;
    }

    void VisualInGate(CardAsset c)
    {
            visualInGateList[visualCount].text = "   " + c.ATK.ToString() + "   " + c.Suit.ToString();
            visualCount++;
            return;
    }
}
