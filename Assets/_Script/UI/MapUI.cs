using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    [SerializeField] private Image mapImage;

    private bool _isMapActive;

    private void Start()
    {
        _isMapActive = false;
    }
    private void OnEnable()
    {
        GameEvents.Instance.OnMapPieceInteracted += ShowMap;
    }
    private void OnDisable()
    {
        GameEvents.Instance.OnMapPieceInteracted -= ShowMap;
    }

    private void ShowMap(MapPieceSO mapData)
    {
        mapImage.sprite = mapData.MapPieceSprite;
        _isMapActive = !_isMapActive;

        if(_isMapActive)
        {
            mapImage.gameObject.SetActive(true);
        }
        else
        {
            mapImage.gameObject.SetActive(false);
        }
    }
}
