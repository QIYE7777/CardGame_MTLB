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
    public CardAsset[] arrayExpect2;
    public Text[] visualInGateList;
    private int visualCount = 0;
    private bool isPressButton;

    public List<CardAsset> cardClubList = new List<CardAsset>();
    public List<CardAsset> cardDiamondList = new List<CardAsset>();
    public List<CardAsset> cardHeartsList = new List<CardAsset>();
    public List<CardAsset> cardSpadeList = new List<CardAsset>();
    public List<CardAsset> card4SuitList = new List<CardAsset>();
    public List<CardAsset> card2SuitList = new List<CardAsset>();

    private bool calculateEnd = false;
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
        int addCardCount = 0;
        foreach (CardAsset i in InPreOpenGateAsset)
        {  addCardCount++; }
        foreach (CardAsset i in InPreOpenGateAsset)
        {
            GameManager.Instance.handVisualManger.AddCardFromGate(i, true, addCardCount);
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
    void CheckNO4Effect(CardAsset c)
    {
        //NO.4 仅结算时生效，给你的手牌中与本牌同花色的牌额外+2点战力（不包括本牌）
        if (c.Suit == CardAsset.suit.Club)
            cardClubList.Add(c);
        if (c.Suit == CardAsset.suit.Diamond)
            cardDiamondList.Add(c);
        if (c.Suit == CardAsset.suit.Hearts)
            cardHeartsList.Add(c);
        if (c.Suit == CardAsset.suit.Spade)
            cardSpadeList.Add(c);
        if (c.ATK == 4)
            card4SuitList.Add(c);
        if (c.ATK == 2)
            card2SuitList.Add(c);
    }
    public void calculatePoint()
    {
        if (calculateEnd) return;
        foreach(CardAsset c in InPreOpenGateAsset)
            CheckNO4Effect(c);

        //把visualInGateList清空
        visualCount = 0;
        StartCoroutine(ClearGateText());

        //NO.4
        if(card4SuitList.Count > 0 && TradeInManager.Instance.cardsNumberInTrade + GameManager.Instance.handVisualManger.cardNumberInHand == 0)
        {
            int count = 0;
            foreach(CardAsset c in card4SuitList)
            {
                count++;
                if(card2SuitList.Count == 0)
                {
                    switch (c.Suit)
                    {
                        case CardAsset.suit.Club:
                            finalPoint += (cardClubList.Count - 1) * 2;
                            break;
                        case CardAsset.suit.Diamond:
                            finalPoint += (cardDiamondList.Count - 1) * 2;
                            break;
                        case CardAsset.suit.Hearts:
                            finalPoint += (cardHeartsList.Count - 1) * 2;
                            break;
                        case CardAsset.suit.Spade:
                            finalPoint += (cardSpadeList.Count - 1) * 2;
                            break;
                    }
                }
                else
                {
                    switch (c.Suit)
                    {
                        case CardAsset.suit.Club:
                            finalPoint += (cardClubList.Count - 1) * 2;
                            if (count > 1) return;
                            foreach (CardAsset x in card2SuitList)
                            {
                                if(x.Suit != c.Suit)
                                    finalPoint += 2;
                            } 
                            break;
                        case CardAsset.suit.Diamond:
                            finalPoint += (cardDiamondList.Count - 1) * 2;
                            if (count > 1) return;
                            foreach (CardAsset x in card2SuitList)
                            {
                                if (x.Suit != c.Suit)
                                    finalPoint += 2;
                            }
                            break;
                        case CardAsset.suit.Hearts:
                            finalPoint += (cardHeartsList.Count - 1) * 2;
                            if (count > 1) return;
                            foreach (CardAsset x in card2SuitList)
                            {
                                if (x.Suit != c.Suit)
                                    finalPoint += 2;
                            }
                            break;
                        case CardAsset.suit.Spade:
                            finalPoint += (cardSpadeList.Count - 1) * 2;
                            if (count > 1) return;
                            foreach (CardAsset x in card2SuitList)
                            {
                                if (x.Suit != c.Suit)
                                    finalPoint += 2;
                            }
                            break;
                    }
                }
            }
        }

        finalPoint = CalculatePointTool.FinalPoint(InPreOpenGateAsset) + finalPoint;
        if (IsFlush() && TradeInManager.Instance.cardsNumberInTrade + GameManager.Instance.handVisualManger.cardNumberInHand == 0)//如果手牌加tradein里的牌为零的话(即向大门提交最后一组牌的时候)，再检查同花
        {
            finalPoint = finalPoint * 2;
        }
        
        //清空InPreOpenGateAsset数组
        InPreOpenGateAsset = new CardAsset[0];
        if (TradeInManager.Instance.cardsNumberInTrade + GameManager.Instance.handVisualManger.cardNumberInHand <= 0) calculateEnd = true;
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
        //NO.2 在结算阶段能够被视为任一花色
        List<CardAsset> listExpect2 = new List<CardAsset>();
        for (int x = 0; x < arrayForFlush.Length; x++)
        {
            if (arrayForFlush[x].ATK != 2)
                listExpect2.Add(arrayForFlush[x]);
        }
        arrayExpect2 = listExpect2.ToArray();

        if (arrayExpect2.Length == 0) return true;

        CardAsset.suit temporarySuit = arrayExpect2[0].Suit;
        int count = 0;
        for (int x = 0; x < arrayExpect2.Length; x++)
        {
            if (arrayExpect2[x].Suit == temporarySuit)
                count++;
            if (count == arrayExpect2.Length)
            {
                Debug.Log(count);
                return true;
            }
        }
        return false;
    }

    void VisualInGate(CardAsset c)
    {
            visualInGateList[visualCount].text = "   " + c.ATK.ToString() + "   " + c.Suit.ToString();
            visualCount++;
            return;
    }
}
