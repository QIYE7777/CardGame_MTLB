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

    public CardAsset lastRemoveCardAsset;

    private int cardNumber;
    public Transform[] slots;
    private float slotMoveDistanceX;
    private int cardNumberInHand;
    private int instantiateIndex;
    bool cardsFromTrade = false;
    public EventSystem eventSystem;
    private CardDrugAndReplace NewCardDrugAndReplace;

    private void Start()
    {
        slots = sameDistanceChildren.children;
        slotMoveDistanceX = sameDistanceChildren.dist.x;
        cardNumber = cardAsset.Length;
        cardNumberInHand = 0;

        DeckManager.Instance.DealingCard += AddCard;
        TradeInManager.Instance.addCardFromTrade += AddCardFromTrade;
    }
    private void Update()
    {
        cardNumberInHand = cardAsset.Length;

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

        if (cardNumberInHand + TradeInManager.Instance.cardsNumberInTrade < 10|| cardsFromTrade)
        {
            //MouseClickIgnored();
            List<CardAsset> cardAssets = new List<CardAsset>(cardAsset);
            cardAssets.Add(c);
            cardAsset = cardAssets.ToArray();

            AddCardVisual();
            return;
        }
        Debug.Log("������10����");
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
    }

    public void RemoveCard(GameObject r)
    {
        //�Ƴ����ƻᱻ�浽TradeIn��
        lastRemoveCardAsset = r.GetComponent<OneCardManager>().cardAsset;
        TradeInManager.Instance.AddCardInPretrade(lastRemoveCardAsset);

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
