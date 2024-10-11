using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeckManager : Singleton<DeckManager>
{
    public event Action<CardAsset,int> DealingCard;

    public CardAsset[] deck;

    public bool cardInHandFull;

    protected override void Awake()
    {
        base.Awake();

        //DontDestroyOnLoad(this);
    }

    private void Start()
    {
        GameManager.Instance.DealDefaultCards += DealCard;
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
    public void DealCard(int dealCardCount)
    {
        if (deck.Length < 1)
        {
            Debug.Log("�ƿⱻ�����");
            return;
        }
        DealingCard?.Invoke(deck[0], dealCardCount);
        if (cardInHandFull)
        {
            cardInHandFull = false;
            return; 
        }
        List<CardAsset> cards = new List<CardAsset>(deck);
        cards.Remove(deck[0]);
        deck = cards.ToArray();
    }
}
