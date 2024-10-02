using UnityEngine;
using UnityEngine.UI;

public class OneCardManager : MonoBehaviour
{
    public CardAsset cardAsset;

    [Header("Text")]
    public Text description;
    public Text ATK;
    [Header("Image")]
    public Image graphic;
    public Image suit;
    [Header("Other")]
    public RectTransform descriptionComponment;

    private void Awake()
    {
        if (cardAsset != null)
        {
            ReadFromCardAsset();
            RemoveDescription();
        }
    }

    void ReadFromCardAsset()
    {
        description.text = cardAsset.Description;
        ATK.text = cardAsset.ATK.ToString();
        graphic.sprite = cardAsset.CardImage;
        suit.sprite = cardAsset.SuitIcon;
    }

    void RemoveDescription()
    {
        if (cardAsset.ATK > 3)
            descriptionComponment.gameObject.SetActive(false);
    }

    public void ChangeCardAsset(CardAsset c) 
    {
        cardAsset = c;
        ReadFromCardAsset();
        RemoveDescription();
    }
}
