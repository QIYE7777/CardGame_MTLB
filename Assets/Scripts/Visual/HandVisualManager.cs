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

        //Debug.Log("����ӵ�ʣ��������Ϊ" + canAddCardsInHand);
        canAddCardsInHand = handCardsLimination - (cardNumberInHand + cardNumberInGate);

        ReplenishGap();
    }

    void MouseClickIgnored()//TODO:������������õķ���
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
        Debug.Log("������10����");
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
            // �ڶ�����ɺ󣬻ָ�����״̬
            swapSequence.OnComplete(() =>
            {
                // ��������뿪��
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
    /* ���������ı� ���Ĺ�λ�õ����Ƶ�˳��
        void ReplenishGap()
    {
        // ��ʱ�б�������е������壨���ƣ�
        List<Transform> cardsInHand = new List<Transform>();

        // �������в�λ���ռ����������壨���ƣ�
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].childCount > 0)  // �����ǰ��λ��������
            {
                Transform card = slots[i].GetChild(0);  // ��ȡ��ǰ��λ�ĵ�һ�������壨���ƣ�
                cardsInHand.Add(card);  // ����������뵽��ʱ�б���
            }
        }

        // �������������������õ�ǰ��Ĳ�λ��
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < cardsInHand.Count)  // �����ǰ��λ������С�ڿ�������
            {
                // �������ƶ�����ǰ��λ��
                Transform card = cardsInHand[i];
                card.SetParent(slots[i]);  // ���ÿ���Ϊ��ǰ��λ��������
                card.localPosition = Vector3.zero;  // ���ÿ��Ƶı���λ�õ���λ������
            }
        }

        Debug.Log("ReplenishGap ִ����ϣ��������������С�");
    }*/

}
