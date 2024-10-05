using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    public float duration;
    public Text[] diceText;
    public Text totalDiceText;
    public int diceValue;
    public GameObject dicesUI;
    public GameObject totalDicesUI;
    public GameObject collectButton;

    private bool isRolling = false;
    private int totalDiceValue;

    private void Start()
    {
        TradeInManager.Instance.rollDices += RollDice;
    }

    public void RollDice(int i)
    {
        if (isRolling) return;

        collectButton.SetActive(false);

        totalDiceValue = 0;
        for (int c = 0; c < i; c++)
        {
            StartCoroutine(RollDiceIE(c));
        }
        StartCoroutine(ShowTotalDiceValueIE());
    }
    IEnumerator RollDiceIE(int c)
    {
        isRolling = true;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            diceValue = Random.Range(1, 7);
            diceText[c].text = diceValue.ToString();
            elapsedTime += 0.2f;
            yield return new WaitForSeconds(0.1f);
        }
        diceValue = Random.Range(1, 7);
        diceText[c].text = diceValue.ToString();
        totalDiceValue += diceValue;
        isRolling = false;
    }
    IEnumerator ShowTotalDiceValueIE()
    {
        yield return new WaitForSeconds(1.8f);
        ShowTotalDiceValue();
    }

    void ShowTotalDiceValue()
    {
        totalDiceText.text = totalDiceValue.ToString();
        dicesUI.SetActive(false);
        totalDicesUI.SetActive(true);
        collectButton.SetActive(true);
    }

    public void CollectCards() 
    {
        collectButton.SetActive(false);
        for (int i =0;i< totalDiceValue; i++)
        {
            DeckManager.Instance.DealCard();
        }
    }
}
