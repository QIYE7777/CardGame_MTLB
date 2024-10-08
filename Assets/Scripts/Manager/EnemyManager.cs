using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class EnemyManager : Singleton<EnemyManager>
{
    public EnemyAsset enemyAsset;
    public Text healthUI;
    public GameObject healthUIob;
    public Image enemyImage;
    public Text description;
    public GameObject glowImage;
    public float variationSpeed;
    public Image clawedAttackImage;
    public AudioSource clawedAttackSound;
    public CanvasGroup clawedAttackCanvas;
    public bool isDead;
    public CardAsset[] InPreBattleGateAsset;
    public Text[] visualInPreBattleList;
    public Text[] visualInEnemyPreList;
    private int visualCount = 0;
    public GameObject deadthIcon;
    public GameObject enemyPanal;
    public AudioSource roar;

    private bool isPressButton;
    private float temporaryHealth;
    private float health;
    private int damage = 0;
    private CardDrugAndReplace cardDrugAndReplace;
    //public event Action<CardAsset, bool> addCardFromPreBattle;

    private void Start()
    {
        healthUI.text = enemyAsset.Health.ToString();
        enemyImage.sprite = enemyAsset.CardImage;
        description.text = enemyAsset.Description;
        temporaryHealth = enemyAsset.Health;
        health = enemyAsset.Health;
    }
    private void Update()
    {
        if (isDead) return;
        CheckHealth();
    }
    public void glowFrame(bool x)
    {
        if (isDead) return;
        glowImage.SetActive(x);
    }
    public void LoseHealth()
    {
        if (isDead) return;
        if (InPreBattleGateAsset.Length == 0)
        {
            Debug.Log("没上牌，无法造成伤害");
            return;
        }

        StartCoroutine(ClearGateText());
        foreach (CardAsset c in InPreBattleGateAsset)
        {
            damage += c.ATK;
        }
        InPreBattleGateAsset = new CardAsset[0];
        visualCount = 0;

        //吞食音效和图标
        AttackEffect();
        health = temporaryHealth - damage;

        if (health <= 0)
        {
            //数字消失，黑白图片,伤害清零
            isDead = true;
            healthUIob.SetActive(false);
            deadthIcon.SetActive(true);
            damage = 0;

            RoomManager.Instance.FromBattleToRecruit();
        }
        if (health > 0)
        {
            //伤害清零
            damage = 0;
            //随机吞食玩家一张牌
            StartCoroutine(AttackPlayer());
        }
    }
    IEnumerator AttackPlayer()//吞食玩家一张牌
    {
        //TODO:可以加个动画或者提示啥的
        GameManager.Instance.handVisualManger.NewCardDrugAndReplace.notDrugCard(true);
        yield return new WaitForSeconds(1);
        AtackAnimation();
        GameManager.Instance.handVisualManger.RandomLoseCard();
        yield return new WaitForSeconds(0.01f);
        GameManager.Instance.CheckPlayerDead();
        yield return new WaitForSeconds(1.3f);
        GameManager.Instance.handVisualManger.NewCardDrugAndReplace.notDrugCard(false);
    }
    void AtackAnimation()
    {
        roar.Play();
        enemyPanal.transform.DOShakePosition(1f, 3, 60,5);
    }
    void CheckHealth()
    {
        //temporaryFinalPoint现在显示的点数
        temporaryHealth = Mathf.MoveTowards(temporaryHealth, health, variationSpeed * Time.deltaTime);
        RefreshHealthText();
    }
    void RefreshHealthText()
    {
        healthUI.text = Mathf.RoundToInt(temporaryHealth).ToString();
    }

    void AttackEffect()
    {
        clawedAttackImage.enabled = true;
        clawedAttackSound.Play();
        StartCoroutine(AttackEffectIE());
        
    }
    IEnumerator AttackEffectIE()
    {
        clawedAttackCanvas.DOFade(0, 0.5f);
        clawedAttackImage.rectTransform.DOScale(2, 0.5f);
        yield return new WaitForSeconds(1);
        clawedAttackImage.enabled = false;
        clawedAttackCanvas.alpha = 1;
        clawedAttackImage.rectTransform.localScale = new Vector3(1, 1, 1);
    }

    public void AddCardInPreBattle(CardAsset c)
    {
        List<CardAsset> InPreOpenGateAssets = new List<CardAsset>(InPreBattleGateAsset);
        InPreOpenGateAssets.Add(c);
        InPreBattleGateAsset = InPreOpenGateAssets.ToArray();
        VisualInEnemyPre(c);
    }

    public void giveAllCardsBack()
    {
        if (InPreBattleGateAsset == null) return;
        if (isPressButton) return;
        isPressButton = true;

        //把visualInGateList清空
        visualCount = 0;
        StartCoroutine(ClearGateText());

        StartCoroutine(giveAllCardsBackIE());
    }
    IEnumerator ClearGateText()
    {
        foreach (Text t in visualInEnemyPreList)
        {
            t.text = "  ";
            yield return new WaitForSeconds(0.6f);
        }
        isPressButton = false;
    }
    IEnumerator giveAllCardsBackIE()
    {
        foreach (CardAsset i in InPreBattleGateAsset)
        {
            GameManager.Instance.handVisualManger.AddCardFromEnemyPreBattle(i, true);
            yield return new WaitForSeconds(0.5f);
            List<CardAsset> InPretradecardAssets = new List<CardAsset>(InPreBattleGateAsset);
            InPretradecardAssets.Remove(i);
            InPreBattleGateAsset = InPretradecardAssets.ToArray();
            yield return new WaitForSeconds(0.05f);
        }
        isPressButton = false;
    }
    void VisualInEnemyPre(CardAsset c)
    {
        visualInEnemyPreList[visualCount].text = "   " + c.ATK.ToString() + " " + c.Suit.ToString();
        visualCount++;
        return;
    }
}
