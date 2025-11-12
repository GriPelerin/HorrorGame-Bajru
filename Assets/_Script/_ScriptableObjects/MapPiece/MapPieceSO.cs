using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map Piece", menuName = "Map Piece")]
public class MapPieceSO : ScriptableObject
{
    [field: SerializeField] public Sprite MapPieceSprite { get; set; }
    public int codeNumber { get; set; }
    [field: SerializeField] public Vector2 codeTextLocation { get; set; }

    public void SetNumber(int number)
    {
        codeNumber = number;
    }
}
