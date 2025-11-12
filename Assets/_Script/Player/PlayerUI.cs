using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("INVENTORY")]
    [SerializeField] private RectTransform[] inventorySlots = new RectTransform[5];
    [SerializeField] private Image[] inventorySlotImage = new Image[5];
    [SerializeField] private Image[] inventoryBackgroundImage = new Image[5];
    [SerializeField] private Sprite emptySlotSprite;

    public RectTransform[] InventorySlots => inventorySlots;
    public Image[] InventorySlotImage => inventorySlotImage;
    public Image[] InventoryBackgroundImage => inventoryBackgroundImage;
    public Sprite EmptySlotSprite => emptySlotSprite;

    [Space(20)]
    [Header("MAP PIECE")]
    [SerializeField] private Image mapImage;
    [SerializeField] private TextMeshProUGUI mapCodeText;

    public Image MapImage => mapImage;

    private bool _isMapActive;

    [Space(20)]
    [Header("INTERACT")]
    [SerializeField] private TextMeshProUGUI interactText;
    public TextMeshProUGUI InteractText => interactText;


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
        mapCodeText.text = mapData.codeNumber.ToString();
        mapCodeText.rectTransform.anchoredPosition = mapData.codeTextLocation;
        _isMapActive = !_isMapActive;

        if (_isMapActive)
        {
            mapImage.gameObject.SetActive(true);
        }
        else
        {
            mapImage.gameObject.SetActive(false);
        }
    }
}
