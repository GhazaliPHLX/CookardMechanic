using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Cards/CardData")]
public class CardData : ScriptableObject
{
    [Header("Data Kartu")]
    public string CardName;
    public Sprite CardSprite;
    public CardType type;
}

public enum CardType
{
    Coin
}
