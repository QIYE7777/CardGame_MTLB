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
            // ����һ��������� j����Χ�� 0 �� i������ i��
            int j = UnityEngine.Random.Range(0, i + 1);

            // ���� array[i] �� array[j] ��ֵ
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
    public void DealCard()
    {
        if (deck.Length < 1)
        {
            Debug.Log("�ƿⱻ�����");
            return;
        }
        DealingCard?.Invoke(deck[0]);
        List<CardAsset> cards = new List<CardAsset>(deck);
        cards.Remove(deck[0]);
        deck = cards.ToArray();
    }

}
