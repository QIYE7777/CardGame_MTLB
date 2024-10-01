using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandVisualManager : MonoBehaviour
{
    public CardAsset[] cardAsset;
    public GameObject warriorCard;
    public GameObject Slot;
    public SameDistanceChildren sameDistanceChildren;

    private int cardNumber;
    private Transform[] slots;
    private float slotMoveDistanceX;
    private int cardNumberInHand;

    private void Awake()
    {
        DeckManager.Instance.DealingCard += AddCard;
    }
    private void Start()
    {
        slots = sameDistanceChildren.children;
        slotMoveDistanceX = sameDistanceChildren.dist.x;
        cardNumber = cardAsset.Length;
        cardNumberInHand = 0;

        /* 发初始手牌的老方法
        if (cardAsset.Length == 0)
            Debug.Log("没有初始手牌");
        else
            StartCoroutine(AddCardVisualWithBreak());
        */
    }

    /*
    IEnumerator AddCardVisualWithBreak()
    {
        for (int i = 0; i < cardNumber; i++)
        {
            AddCardVisual();
            yield return new WaitForSeconds(0.5f);
        }
    }
    */
    public void AddCard(CardAsset c)
    {
        List<CardAsset> cardAssets = new List<CardAsset>(cardAsset);
        cardAssets.Add(c);
        cardAsset = cardAssets.ToArray();

        AddCardVisual();
    }

    public void AddCardVisual()
    {
        if(cardNumberInHand < 10)
        {
            int currentCardIndex = cardNumberInHand;
            GameObject newCard = Instantiate(warriorCard, slots[currentCardIndex]);
            OneCardManager NewCardOneCardManager = newCard.GetComponent<OneCardManager>();
            NewCardOneCardManager.ChangeCardAsset(cardAsset[currentCardIndex]);

            cardNumberInHand++;

            MoveToMiddle();
        }
        else
        {
            Debug.Log("手牌满10张了");
            return;
        }
            
    }
    
    void MoveToMiddle()
    {
        if(cardNumberInHand > 1)
        {
            float moveDistanceX = -slotMoveDistanceX / 2;
            Slot.transform.DOMoveX(Slot.transform.position.x + moveDistanceX, 0.5f);
        }
    }
}
