using UnityEngine;


public class CardAsset : ScriptableObject
{
    public enum suit { Club, Diamond, Hearts, Spade }; 

    [Header("General info")]
    [TextArea(2,3)]
    public string Description;
    public Sprite CardImage;
    public int ATK;

    public Sprite SuitIcon;
    public suit Suit;
}
