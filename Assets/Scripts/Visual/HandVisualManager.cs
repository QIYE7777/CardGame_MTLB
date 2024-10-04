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

    public CardAsset lastRemoveCardAsset;

    private int cardNumber;
    private Transform[] slots;
    private float slotMoveDistanceX;
    private int cardNumberInHand;
    private int instantiateIndex;

    private void Start()
    {
        slots = sameDistanceChildren.children;
        slotMoveDistanceX = sameDistanceChildren.dist.x;
        cardNumber = cardAsset.Length;
        cardNumberInHand = 0;

        DeckManager.Instance.DealingCard += AddCard;
    }
    private void Update()
    {
        cardNumberInHand = cardAsset.Length;

        ReplenishGap();
    }
    public void AddCard(CardAsset c)
    {
        if (cardNumberInHand >= 10)
        {
            Debug.Log("手牌满10张了");
            return;
        }

        List<CardAsset> cardAssets = new List<CardAsset>(cardAsset);
        cardAssets.Add(c);
        cardAsset = cardAssets.ToArray();

        AddCardVisual();
    }

    public void AddCardVisual()
    {
        int currentCardIndex = cardNumberInHand;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].childCount == 0)
            {
                instantiateIndex = i;
                break;
            }
        }

        GameObject newCard = Instantiate(warriorCard, slots[instantiateIndex]);
        OneCardManager NewCardOneCardManager = newCard.GetComponent<OneCardManager>();
        NewCardOneCardManager.ChangeCardAsset(cardAsset[currentCardIndex]);

        MoveToMiddle();
    }

    void MoveToMiddle()
    {
        if (cardNumberInHand > 0)
        {
            float moveDistanceX = -slotMoveDistanceX / 2;
            Slot.transform.DOMoveX(Slot.transform.position.x + moveDistanceX, 0.5f);
        }
    }

    public void RemoveCard(GameObject r)
    {
        //移除的牌会被存到TradeIn中
        lastRemoveCardAsset = r.GetComponent<OneCardManager>().cardAsset;
        TradeInManager.Instance.AddCardInPretrade(lastRemoveCardAsset);

        Slot.transform.DOMoveX(Slot.transform.position.x + slotMoveDistanceX / 2, 0.5f);

        Destroy(r);

        //ReplenishGap();
    }

    void ReplenishGap()
    {
        StartCoroutine(ReplenishGapIE());
    }


    IEnumerator ReplenishGapIE()
    {
        for (int i = 1; i < slots.Length; i++)
        {
            if (slots[i].childCount >= 1 && slots[i - 1].childCount < 1)
            {
                var c = slots[i].GetChild(0);
                c.SetParent(slots[i - 1]);

                yield return new WaitForSeconds(0.1f);

                c.localPosition = new Vector3(0, 0, 0);

                yield return new WaitForSeconds(0.05f);
            }
        }
        yield return null;
    }
    
    
    
    
    /* 这个方法会改变 更改过位置的手牌的顺序
        void ReplenishGap()
    {
        // 临时列表，存放所有的子物体（卡牌）
        List<Transform> cardsInHand = new List<Transform>();

        // 遍历所有槽位，收集所有子物体（卡牌）
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].childCount > 0)  // 如果当前槽位有子物体
            {
                Transform card = slots[i].GetChild(0);  // 获取当前槽位的第一个子物体（卡牌）
                cardsInHand.Add(card);  // 将子物体加入到临时列表中
            }
        }

        // 将所有子物体依次设置到前面的槽位中
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < cardsInHand.Count)  // 如果当前槽位的索引小于卡牌数量
            {
                // 将卡牌移动到当前槽位中
                Transform card = cardsInHand[i];
                card.SetParent(slots[i]);  // 设置卡牌为当前槽位的子物体
                card.localPosition = Vector3.zero;  // 重置卡牌的本地位置到槽位的中心
            }
        }

        Debug.Log("ReplenishGap 执行完毕，卡牌已重新排列。");
    }*/

}
