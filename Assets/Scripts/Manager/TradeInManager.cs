using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TradeInManager : Singleton<TradeInManager>
{
    public GameObject glowImage;
    public CardAsset[] InPretradecardAsset;

    public int cardsNumberInTrade;

    public event Action<CardAsset,bool> addCardFromTrade;

    private void Update()
    {
        cardsNumberInTrade = InPretradecardAsset.Length;
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
    }

    public void giveAllCardsBack()
    {
        StartCoroutine(giveAllCardsBackIE());
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
}
