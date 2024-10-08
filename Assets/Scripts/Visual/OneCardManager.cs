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
    public GameObject glowImage;

    private static bool card1ShouldGlow =false;
    private void Awake()
    {
        if (cardAsset != null)
        {
            ReadFromCardAsset();
            RemoveDescription();
        }

        RoomSwitcher.Instance.OpenandCloseGlowImageEvent += OpenAndCloseGlowImageFor234;
        RoomManager.Instance.OpenGlowImageFor1 += OpenGlowImageFor1;
        RoomManager.Instance.CloseGlowImageFor1 += CloseGlowImageFor1;
    }
    private void Start()
    {
        if (cardAsset.ATK == 3) glowImage.SetActive(true);
        if(card1ShouldGlow && cardAsset.ATK == 1) glowImage.SetActive(true);
    }

    public void OpenAndCloseGlowImageFor234()//进入大门结算关卡的时候
    {
        if (glowImage == null) return;
        if(cardAsset.ATK == 2)
        {
            glowImage.SetActive(true);
        }
        if (cardAsset.ATK == 4)
        {
            glowImage.SetActive(true);
        }
        if (cardAsset.ATK == 3 )
        { 
            glowImage.SetActive(false);
        }

    }
    public void OpenGlowImageFor1()//进入招募环节的时候
    {
        if (glowImage == null) return;
        if (cardAsset.ATK == 1)
        {
            glowImage.SetActive(true);
            card1ShouldGlow = true;
        }
    }
    public void CloseGlowImageFor1()//离开招募环节的时候
    {
        if (glowImage == null) return;
        if (cardAsset.ATK == 1)
        {
            glowImage.SetActive(false);
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
