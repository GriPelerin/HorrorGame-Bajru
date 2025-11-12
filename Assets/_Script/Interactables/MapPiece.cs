using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPiece : MonoBehaviour, IInteractable
{
    [SerializeField] private MapPieceSO mapPieceData;
    public string InteractableName { get { return "Map Piece"; } set => throw new System.NotImplementedException(); }

    public void Interact()
    {
        GameEvents.Instance.TriggerMapPieceInteracted(mapPieceData);
        AudioManager.Instance.PlaySound(SoundList.NoteSound);
    }
}
