using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Card Asset")]
public class Card : ScriptableObject
{
    [SerializeField] private Sprite cardSprite;
    [SerializeField] private int cardWeight;
    
    public Sprite CardSprite => cardSprite;
    public int CardWeight => cardWeight;
}