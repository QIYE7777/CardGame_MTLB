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
    public OneCardManager hoverCardManage;

    private static bool card1ShouldGlow =false;
    private void Awake()
    {
        RoomSwitcher.Instance.OpenandCloseGlowImageEvent += OpenAndCloseGlowImageFor234;
        RoomManager.Instance.OpenGlowImageFor1 += OpenGlowImageFor1;
        RoomManager.Instance.CloseGlowImageFor1 += CloseGlowImageFor1;
        GameManager.ClearLastGameData += ClearLastGameData;
    }
    private void Start()
    {
        if (hoverCardManage != null)
        {
            hoverCardManage.cardAsset = cardAsset;
            Debug.Log(cardAsset);
        }

        if (cardAsset != null)
        {
            ReadFromCardAsset();
            RemoveDescription();
        }

        if (glowImage == null) return;
        if (cardAsset.ATK == 3) glowImage.SetActive(true);
        if(card1ShouldGlow && cardAsset.ATK == 1) glowImage.SetActive(true);
    }

    public void OpenAndCloseGlowImageFor234()//������Ž���ؿ���ʱ��
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
    public void OpenGlowImageFor1()//������ļ���ڵ�ʱ��
    {
        if (glowImage == null) return;
        if (cardAsset.ATK == 1)
        {
            glowImage.SetActive(true);
            card1ShouldGlow = true;
        }
    }
    public void CloseGlowImageFor1()//�뿪��ļ���ڵ�ʱ��
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
    public void ClearLastGameData()//����no.1��glowimage
    {
        card1ShouldGlow = false;
    }
}
