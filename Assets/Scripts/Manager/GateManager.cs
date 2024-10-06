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

    public event Action<CardAsset, bool> addCardFromGate;
    private void Update()
    {
        cardsNumberInGate = InPreOpenGateAsset.Length;
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
    }

    public void giveAllCardsBack()
    {
        if (InPreOpenGateAsset == null) return;
        StartCoroutine(giveAllCardsBackIE());
    }

    IEnumerator giveAllCardsBackIE()
    {
        foreach (CardAsset i in InPreOpenGateAsset)
        {
            addCardFromGate?.Invoke(i, true);
            yield return new WaitForSeconds(0.5f);
            List<CardAsset> InPretradecardAssets = new List<CardAsset>(InPreOpenGateAsset);
            InPretradecardAssets.Remove(i);
            InPreOpenGateAsset = InPretradecardAssets.ToArray();
            yield return new WaitForSeconds(0.05f);
        }
    }
    void RefreshATKPoint()
    {
        finalPointUI.text = Mathf.RoundToInt(temporaryFinalPoint).ToString();
    }
    public void calculatePoint()
    {
        finalPoint = CalculatePointTool.FinalPoint(InPreOpenGateAsset) + finalPoint;

        //清空InPreOpenGateAsset数组
        InPreOpenGateAsset = new CardAsset[0];
    }
    void CheckATKPoint()
    {
        //temporaryFinalPoint现在显示的点数
        temporaryFinalPoint = Mathf.MoveTowards(temporaryFinalPoint, finalPoint, variationSpeed * Time.deltaTime);
        RefreshATKPoint();
    }
}
