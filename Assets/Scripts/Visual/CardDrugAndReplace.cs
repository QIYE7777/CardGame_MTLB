using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CardDrugAndReplace : MonoBehaviour
{
    public bool UsePointerDisplacement = true;
    public Transform visualCard;//为了拿起靠近镜头

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
    private HoverPreview hoverPreview;

    private GateManager gateManager;
    public EnemyManager enemyManager;
    public bool overlapTradeInArea;
    public bool overlapGateArea;
    public bool overlapEnemyArea;

    // 标识是否正在执行动画
    private static bool isAnimating = false;

    public static event Action<bool> cannotHover;

    private void Start()
    {
        hoverPreview = gameObject.GetComponent<HoverPreview>();
    }
    private void OnMouseDown()
    {
        //Debug.Log(isAnimating);

        // 检查是否正在执行动画，正在执行动画时不允许操作
        if (isAnimating) return;

        //visualCard.transform.Translate(0, 0.4f, -0.2f);

        cannotHover?.Invoke(false);

        dragging = true;
        zDisplacement = transform.position.z - Camera.main.transform.position.z;
        if (UsePointerDisplacement)
            pointerDisPlacement = transform.position - MouseInWorldCoords();
        else
            pointerDisPlacement = Vector3.zero;

        //记录卡牌初始位置
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
        //Debug.Log("释放此卡");

        if (!dragging) return;

        if (dragging)
            dragging = false;

        cannotHover?.Invoke(true);

        tradeInManager?.glowFrame(false);
        gateManager?.glowFrame(false);
        enemyManager?.glowFrame(false);

        //visualCard.transform.Translate(0, -0.4f, 0.2f);

        if (overlapOtherCard)
        {
            isAnimating = true;

            ReplaceCardsByParent();

        }
        else if(overlapTradeInArea|| overlapGateArea )
        {
            //TODO: 用减牌功能替换destroy
            GameManager.Instance.handVisualManger.RemoveCard(transform.gameObject);
        }
        else if (overlapEnemyArea)
        {
            if (enemyManager.isDead) return;
            GameManager.Instance.handVisualManger.RemoveCard(transform.gameObject);
        }
        /*else if (overlapGateArea)
        {
            GameManager.Instance.handVisualManger.RemoveCard(transform.gameObject);
        }*/
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
        if (!overlapOtherCard) return; ;
        //slotsInHand = GameManager.Instance.handVisualManger.slots;
        currentParent = transform.parent;
        if (overlapCard == null) return;
        overlapCardParent = overlapCard.transform.parent;
        if (overlapCardParent.childCount <= 1)
        {
            transform?.SetParent(overlapCardParent);
            transform.DOLocalMove(new Vector3(0, 0, 0), 0.1f);
            //transform.localPosition = new Vector3(0, 0, 0);

        }

        if (currentParent.childCount == 0)
        {
            overlapCard?.transform.SetParent(currentParent);
            overlapCard.transform.DOLocalMove(new Vector3(0, 0, 0), 0.1f);
            //overlapCard.transform.localPosition = new Vector3(0, 0, 0);

        }

        // 交换完毕后恢复状态
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
        // 交换完毕后恢复状态
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
        if (other.CompareTag("GateArea"))
        {
            overlapGateArea = true;
            gateManager = other.GetComponent<GateManager>();
            gateManager.glowFrame(true);
        }
        if (other.CompareTag("EnemyArea"))
        {
            overlapEnemyArea = true;
            enemyManager = other.GetComponent<EnemyManager>();
            enemyManager.glowFrame(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 当不再与该卡牌重叠时，清除重叠状态
        if (other == overlapCard)
        {
            overlapOtherCard = false;
            overlapCard = null;
        }
        if (other.CompareTag("TradeInArea"))
        {
            overlapTradeInArea = false;
            tradeInManager?.glowFrame(false);
            //overlapCard = null;
        }
        if (other.CompareTag("GateArea"))
        {
            overlapGateArea = false;
            gateManager?.glowFrame(false);
            //overlapCard = null;
        }
        if (other.CompareTag("EnemyArea"))
        {
            overlapEnemyArea = false;
            enemyManager?.glowFrame(false);
            //overlapCard = null;
        }
    }
}
