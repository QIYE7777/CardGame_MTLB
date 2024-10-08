using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class HandVisualManager : MonoBehaviour
{
    public CardAsset[] cardAsset;
    public GameObject warriorCard;
    public GameObject Slot;
    public SameDistanceChildren sameDistanceChildren;
    public int handCardsLimination = 10;
    public AudioSource dealSound;

    public CardAsset lastRemoveCardAsset;

    private int cardNumber;
    public Transform[] slots;
    private float slotMoveDistanceX;
    public int cardNumberInHand;
    private int instantiateIndex;
    bool cardsFromTrade = false;
    bool cardsFromGate = false;
    bool cardsFromEnemy = false;
    public EventSystem eventSystem;
    public  CardDrugAndReplace NewCardDrugAndReplace;
    public int canAddCardsInHand;
    public int cardNumberInGate;
    private void Start()
    {
        slots = sameDistanceChildren.children;
        slotMoveDistanceX = sameDistanceChildren.dist.x;
        cardNumber = cardAsset.Length;
        cardNumberInHand = 0;

        DeckManager.Instance.DealingCard += AddCard;
        //TradeInManager.Instance.addCardFromTrade += AddCardFromTrade;
        //GateManager.Instance.addCardFromGate += AddCardFromGate;
        //EnemyManager.Instance.addCardFromPreBattle += AddCardFromEnemyPreBattle;
    }
    private void Update()
    {
        cardNumberInHand = cardAsset.Length;
        /*if (RoomSwitcher.Instance.currentRoomIndex >=0 && cardNumberInHand + TradeInManager.Instance.cardsNumberInTrade + GateManager.Instance.cardsNumberInGate <= 0)
            GameManager.Instance.PlayerDead();*/

        //Debug.Log("可添加的剩余手牌数为" + canAddCardsInHand);
        canAddCardsInHand = handCardsLimination - (cardNumberInHand + cardNumberInGate);

        ReplenishGap();
    }

    void MouseClickIgnored()//TODO:想想禁用鼠标最好的方法
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void MouseClickTurnOn()
    {
        Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
    }
    public void AddCard(CardAsset c)
    {
        if (cardNumberInHand + TradeInManager.Instance.cardsNumberInTrade + GateManager.Instance.cardsNumberInGate   < handCardsLimination || cardsFromTrade ||cardsFromGate)
        {
            dealSound.Play();

            //MouseClickIgnored();
            List<CardAsset> cardAssets = new List<CardAsset>(cardAsset);
            cardAssets.Add(c);
            cardAsset = cardAssets.ToArray();

            AddCardVisual();
            return;
        }
        Debug.Log("手牌满10张了");
        DeckManager.Instance.cardInHandFull = true;
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
        var NewCardOneCardManager = newCard.GetComponent<OneCardManager>();
        NewCardOneCardManager.ChangeCardAsset(cardAsset[currentCardIndex]);
        NewCardDrugAndReplace = newCard.GetComponent<CardDrugAndReplace>();
        NewCardDrugAndReplace.notDrugCard(true);

        MoveToMiddle();
    }

    void MoveToMiddle()
    {
        if (cardNumberInHand > 0)
        {
            float moveDistanceX = -slotMoveDistanceX / 2;
            //Slot.transform.DOMoveX(Slot.transform.position.x + moveDistanceX, 0.5f);

            Sequence swapSequence = DOTween.Sequence();
            swapSequence.Append(Slot.transform.DOMoveX(Slot.transform.position.x + moveDistanceX, 0.5f));
            // 在动画完成后，恢复交互状态
            swapSequence.OnComplete(() =>
            {
                // 鼠标点击输入开启
                //MouseClickTurnOn();
                NewCardDrugAndReplace.notDrugCard(false);
            });
        }
        else NewCardDrugAndReplace.notDrugCard(false);
    }

    public void RemoveCard(GameObject r)
    {
        lastRemoveCardAsset = r.GetComponent<OneCardManager>().cardAsset;
        
        if(r.GetComponent<CardDrugAndReplace>().overlapTradeInArea)
            TradeInManager.Instance.AddCardInPretrade(lastRemoveCardAsset);

        if (r.GetComponent<CardDrugAndReplace>().overlapGateArea)
            GateManager.Instance.AddCardInPreGateAsset(lastRemoveCardAsset);

        if (r.GetComponent<CardDrugAndReplace>().overlapEnemyArea)
            EnemyManager.Instance.AddCardInPreBattle(lastRemoveCardAsset);

        if (cardNumberInHand > 1)
        {
            Slot.transform.DOMoveX(Slot.transform.position.x + slotMoveDistanceX / 2, 0.5f);
        }

        List<CardAsset> cardAssets = new List<CardAsset>(cardAsset);
        cardAssets.Remove(r.GetComponent<OneCardManager>().cardAsset);
        cardAsset = cardAssets.ToArray();

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

    public void AddCardFromTrade(CardAsset c, bool x)
    {
        cardsFromTrade = x;
        AddCard(c);
        cardsFromTrade = false;
    }

    public void AddCardFromGate(CardAsset c, bool x)
    {
        cardsFromGate = x;
        AddCard(c);
        cardsFromGate = false;
        Debug.Log(cardsFromGate);
    }
    public void AddCardFromEnemyPreBattle(CardAsset c, bool x)
    {
        cardsFromEnemy = x;
        AddCard(c);
        cardsFromEnemy = false;
    }

    public void RandomLoseCard()
    {
        if (cardNumberInHand == 0) return;
        int ranfomIndex = Random.Range(0, cardAsset.Length - 1);
        GameObject randomCard = slots[ranfomIndex].GetChild(0).gameObject;

        List<CardAsset> cardAssets = new List<CardAsset>(cardAsset);
        cardAssets.Remove(randomCard.GetComponent<OneCardManager>().cardAsset);
        cardAsset = cardAssets.ToArray();

        randomCard.transform.DOMoveY(0.01f, 1);
        randomCard.GetComponent<CanvasGroup>().DOFade(0, 0.9f);

        StartCoroutine(RandomLoseCardIE(randomCard));
    }
    IEnumerator RandomLoseCardIE(GameObject r)
    {
        yield return new WaitForSeconds(1);
        if (cardNumberInHand > 1)
        {
            Slot.transform.DOMoveX(Slot.transform.position.x + slotMoveDistanceX / 2, 0.5f);
        }
        Destroy(r);
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
