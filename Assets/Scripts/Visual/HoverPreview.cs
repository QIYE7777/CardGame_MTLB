using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HoverPreview : MonoBehaviour
{
    public bool canHover=true;
    public GameObject cardcanvas;
    public GameObject hoverCard;

    public Transform cardCancasTransfer;

    private bool canKillAll;

    private void Start()
    {
        HandVisualManager.allCardsCannotHover += IfHover;
        CardDrugAndReplace.cannotHover += IfHoverAndRestore;
    }
    private void OnMouseEnter()
    {
        if (canHover )
        {
            StopAllCoroutines();
            hoverCard.SetActive(true);
            cardcanvas.SetActive(false);
            cardCancasTransfer = transform;
            StartCoroutine(OnMouseEnterIE());
        }
    }
    IEnumerator OnMouseEnterIE()
    {
        hoverCard.transform.position = cardCancasTransfer.position;
        hoverCard.transform.localScale = new Vector3(1, 1, 1);
        yield return new WaitForSeconds(0.5f);
        hoverCard.transform.DOMove(new Vector3(0,0,0), 0.5f).SetEase (Ease.InOutQuad);
        hoverCard.transform.DOScale(hoverCard.transform.localScale *2.5f, 0.5f);
        yield return new WaitForSeconds(0.5f);

    }
    private void OnMouseExit()
    {
        if(canKillAll)
            DOTween.KillAll(hoverCard.transform);

        StopAllCoroutines();
        if (cardCancasTransfer == null)return ;

        hoverCard.transform.position = cardCancasTransfer.position;
        hoverCard.transform.localScale = new Vector3(1, 1, 1);
        hoverCard.SetActive(false);
        cardcanvas.SetActive(true);
        //StartCoroutine(OnMouseExitIE());
        if (cardCancasTransfer == null) return;
        /*
        // 立即将 hoverCard 返回原位置和原大小，并关闭
        hoverCard.transform.DOMove(cardCancasTransfer.position, 0.5f).SetEase(Ease.InOutQuad);
        hoverCard.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
        {
            hoverCard.SetActive(false);
            cardcanvas.SetActive(true);  // 恢复原始卡牌显示
        });*/
    }

    IEnumerator OnMouseExitIE()
    {
        yield return new WaitForSeconds(0.1f);
    }
    public void IfHover(bool x)
    {
        canHover = x;
        canKillAll = x;
    }

    public void IfHoverAndRestore(bool x)
    {
        canHover = x;
        if (!x && hoverCard!=null)
        {
            DOTween.KillAll();
            StopAllCoroutines();
            hoverCard.transform.position = cardCancasTransfer.position;
            hoverCard.transform.localScale = new Vector3(1, 1, 1);
            hoverCard.SetActive(false);
            cardcanvas.SetActive(true);
        }
    }
}
