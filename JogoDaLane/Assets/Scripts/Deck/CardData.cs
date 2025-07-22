using UnityEngine;

[CreateAssetMenu(fileName = "NewCardData", menuName = "Card System/Card Data")]
public class CardData : ScriptableObject
{
    public string cardName = "Nova Carta";
    public Sprite cardImage;
    public int cost;
    public GameObject troopPrefab;
}