using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CardDrugAndReplace : MonoBehaviour
{
    public bool UsePointerDisplacement = true;
    public Transform visualCard;//Ϊ�����𿿽���ͷ

    private bool dragging = false;
    private float zDisplacement;
    private Vector3 pointerDisPlacement = Vector3.zero;

    private Vector3 initialPosition;
    private bool overlapOtherCard;
    private Vector3 overlapCardPosition;
    private Collider overlapCard;
    private TradeInManager tradeInManager;
    public Transform[] slotsInHand;

    private Transform currentParent ;
    private Transform overlapCardParent;

    private bool overlapTradeInArea;

    // ��ʶ�Ƿ�����ִ�ж���
    private static bool isAnimating = false;

    private void OnMouseDown()
    {
        //Debug.Log("���д˿�");

        // ����Ƿ�����ִ�ж���������ִ�ж���ʱ���������
        if (isAnimating) return;

        visualCard.transform.Translate(0, 0.4f, -0.2f);

        dragging = true;
        zDisplacement = transform.position.z - Camera.main.transform.position.z;
        if (UsePointerDisplacement)
            pointerDisPlacement = transform.position - MouseInWorldCoords();
        else
            pointerDisPlacement = Vector3.zero;

        //��¼���Ƴ�ʼλ��
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (dragging)
        {
            var mousePos = MouseInWorldCoords();
            transform.position = new Vector3(mousePos.x + pointerDisPlacement.x, mousePos.y + pointerDisPlacement.y, transform.position.z);
        }
    }

    private void OnMouseUp()
    {
        //Debug.Log("�ͷŴ˿�");

        if (!dragging) return;

        if (dragging)
            dragging = false;

        tradeInManager?.glowFrame(false);
        visualCard.transform.Translate(0, -0.4f, 0.2f);

        if (overlapOtherCard)
        {
            isAnimating = true;

            ReplaceCardsByParent();

            /*
            // ʹ�� DOTween ����ǰ���ƺ��ص������ƶ����˴˵�λ��
            Sequence swapSequence = DOTween.Sequence();
            swapSequence.Append(transform.DOMove(overlapCardPosition, 0.2f));
            swapSequence.Join(overlapCard.transform.DOMove(initialPosition, 0.2f));

            // �ڶ�����ɺ󣬻ָ�����״̬
            swapSequence.OnComplete(() =>
            {
                // ������Ϻ�ָ�״̬
                isAnimating = false;
                overlapOtherCard = false;
                overlapCard = null;
            });*/
        }
        else if(overlapTradeInArea)
        {
            //TODO: �ü��ƹ����滻destroy
            GameManager.Instance.handVisualManger.RemoveCard(transform.gameObject);
        }
        else            
        {
            isAnimating = true;

            Sequence backSequence2 = DOTween.Sequence();
            backSequence2.Append(transform.DOMove(initialPosition, 0.2f));

            backSequence2.OnComplete(() =>
            {
                isAnimating = false;
                overlapOtherCard = false;
                overlapCard = null;
            });
        }
    }

    void ReplaceCardsByParent()
    {
        //slotsInHand = GameManager.Instance.handVisualManger.slots;
        currentParent = transform.parent;
        overlapCardParent = overlapCard.transform.parent;
        if (overlapCardParent.childCount <= 1)
        {
            transform?.SetParent(overlapCardParent);

            transform.localPosition = new Vector3(0, 0, 0);

        }

        if (currentParent.childCount == 0)
        {
            overlapCard?.transform.SetParent(currentParent);

            overlapCard.transform.localPosition = new Vector3(0, 0, 0);

        }

        // ������Ϻ�ָ�״̬
        isAnimating = false;
        overlapOtherCard = false;
        overlapCard = null;

    }

    public void notDrugCard(bool c)
    {
        isAnimating = c; 
    }

    /*IEnumerator ReplaceCardsByParent()
    {
        //slotsInHand = GameManager.Instance.handVisualManger.slots;
        currentParent = transform.parent;
        overlapCardParent = overlapCard.transform.parent;
        if(overlapCardParent.childCount <= 1)
        {
            transform?.SetParent(overlapCardParent);
            yield return new WaitForSeconds(0.01f);
            transform.localPosition = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
        
        if (currentParent.childCount == 0)
        {
            overlapCard?.transform.SetParent(currentParent);
            yield return new WaitForSeconds(0.01f);
            overlapCard.transform.localPosition = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
        
        yield return new WaitForSeconds(0.01f);
        // ������Ϻ�ָ�״̬
        isAnimating = false;
        overlapOtherCard = false;
        overlapCard = null;
        yield return null;
    }*/


    private Vector3 MouseInWorldCoords() 
    {
        Vector3 screenMousePos = Input.mousePosition;
        screenMousePos.z = zDisplacement;
        return Camera.main.ScreenToWorldPoint(screenMousePos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WorriorCard"))
        {
            overlapOtherCard = true;
            overlapCardPosition = other.transform.position;
            overlapCard = other;
        }
        if (other.CompareTag("TradeInArea"))
        {
            overlapTradeInArea = true;
            tradeInManager = other.GetComponent<TradeInManager>();
            tradeInManager.glowFrame(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ��������ÿ����ص�ʱ������ص�״̬
        if (other == overlapCard)
        {
            overlapOtherCard = false;
            overlapCard = null;
        }
        if (other.CompareTag("TradeInArea"))
        {
            overlapTradeInArea = false;
            tradeInManager.glowFrame(false);
            tradeInManager = null;
        }
    }
}
