using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeckManager : Singleton<DeckManager>
{
    public event Action<CardAsset> DealingCard;

    public CardAsset[] deck;

    protected override void Awake()
    {
        base.Awake();
        GameManager.Instance.DealDefaultCards += DealCard;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        ShuffleArray(deck);
    }
    void ShuffleArray<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            // 生成一个随机索引 j，范围是 0 到 i（包括 i）
            int j = UnityEngine.Random.Range(0, i + 1);

            // 交换 array[i] 和 array[j] 的值
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
    public void DealCard()
    {
        if (deck.Length < 1)
        {
            Debug.Log("牌库被抽空了");
            return;
        }
        DealingCard?.Invoke(deck[0]);
        List<CardAsset> cards = new List<CardAsset>(deck);
        cards.Remove(deck[0]);
        deck = cards.ToArray();
    }

}
