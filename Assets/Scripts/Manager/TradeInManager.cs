using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeInManager : Singleton<TradeInManager>
{
    public GameObject glowImage;
    public CardAsset[] InPretradecardAsset;

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
}
