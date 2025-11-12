using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

namespace Bajru.Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        [Header("Cards in Hand")]
        [SerializeField] GameObject blessingCard;
        [SerializeField] GameObject curseCard;
        [SerializeField] GameObject gambleCard;
        [SerializeField] GameObject stallCard;
        [SerializeField] GameObject revealCard;

        private Vector2[] _originalSlotPos; //Inventory slot position
        private PlayerUI _playerUI;

        private Dictionary<ItemType, GameObject> _itemSetActive = new Dictionary<ItemType, GameObject>();
        private List<Card> _inventoryList = new List<Card>(5);
        private int _selectedItem = -1;


        private void Awake()
        {
            _playerUI = GetComponent<PlayerUI>();
        }
        private void Start()
        {
            SetSlotPositions();
            _itemSetActive.Add(ItemType.BlessingCard, blessingCard);
            _itemSetActive.Add(ItemType.CurseCard, curseCard);
            _itemSetActive.Add(ItemType.GambleCard, gambleCard);
            _itemSetActive.Add(ItemType.StallCard, stallCard);
            _itemSetActive.Add(ItemType.RevealCard, revealCard);
            _selectedItem = -1;
        }
        private void OnEnable()
        {
            if (GameEvents.Instance != null)
            {
                GameEvents.Instance.OnCardPickedUp += AddCardToInventory;
            }
        }

        private void OnDisable()
        {
            if (GameEvents.Instance != null)
            {
                GameEvents.Instance.OnCardPickedUp -= AddCardToInventory;
            }
            DOTween.KillAll();
        }

        private void Update()
        {
            InventoryUI();
            SelectAndUseInput();
        }

        private void NewItemSelected()
        {
            foreach (var cardObject in _itemSetActive.Values)
            {
                cardObject.SetActive(false);
            }

            if (_selectedItem >= 0 && _selectedItem < _inventoryList.Count)
            {
                Card selectedCard = _inventoryList[_selectedItem];
                if (_itemSetActive.ContainsKey(selectedCard.ItemScriptableObject.itemType))
                {
                    _itemSetActive[selectedCard.ItemScriptableObject.itemType].SetActive(true);
                }
            }
        }

        private void AddCardToInventory(Card card)
        {
            if (IsInventoryFull()) return;

            if (card != null)
            {
                AudioManager.Instance.PlaySound(SoundList.CardSound);
                _inventoryList.Add(card); 
                Destroy(card.gameObject);
            }
        }

        private void UseCard(int index)
        {
            if (index >= 0 && index < _inventoryList.Count)
            {
                Card selectedCard = _inventoryList[index];
                selectedCard.Use(selectedCard.CardAbility.abilityType, selectedCard.CardAbility.duration);

                if (_itemSetActive.ContainsKey(selectedCard.ItemScriptableObject.itemType))
                {
                    _itemSetActive[selectedCard.ItemScriptableObject.itemType].SetActive(false);
                    AudioManager.Instance.PlaySound(SoundList.CardAbilitySound);
                }
                _inventoryList.RemoveAt(index);

                _selectedItem = _inventoryList.Count > 0 ? Mathf.Clamp(_selectedItem, 0, _inventoryList.Count - 1) : -1;
                NewItemSelected();
            }
        }

        private bool IsInventoryFull()
        {
            if(_inventoryList.Count > 4)
            {
                return true;
            }
            return false;
        }
        private void SelectAndUseInput()
        {
            if (GameManager.Instance.CurrentGameState != GameStates.GameActive) return;

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ToggleSelection(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ToggleSelection(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ToggleSelection(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ToggleSelection(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                ToggleSelection(4);
            }

            if (Input.GetKeyDown(KeyCode.F) && _selectedItem >= 0 && _inventoryList.Count > _selectedItem && !GameEvents.Instance.IsCardOnCooldown)
            {
                UseCard(_selectedItem);
            }
        }
        private void ToggleSelection(int index)
        {
            if (_selectedItem == index) // same button pressed again
            {
                _selectedItem = -1; // deselect
                NewItemSelected(); // update visuals (turns all off)
            }
            else if (index < _inventoryList.Count)
            {
                _selectedItem = index;
                NewItemSelected();
            }
        }

        private void SetSlotPositions()
        {
            _originalSlotPos = new Vector2[_playerUI.InventorySlots.Length];
            for (int i = 0; i < _playerUI.InventorySlots.Length; i++)
            {
                _originalSlotPos[i] = _playerUI.InventorySlots[i].anchoredPosition;
            }
        }
        private void InventoryUI()
        {
            for (int i = 0; i < 5; i++)
            {
                if (i < _inventoryList.Count)
                {
                    _playerUI.InventorySlotImage[i].sprite = _inventoryList[i].ItemScriptableObject.itemSprite;
                }
                else
                {
                    _playerUI.InventorySlotImage[i].sprite = _playerUI.EmptySlotSprite;
                }
            }

            int a = 0;
            foreach (Image image in _playerUI.InventoryBackgroundImage)
            {
                RectTransform slotTransform = _playerUI.InventorySlots[a];
                if (a == _selectedItem)
                {
                    image.color = new Color32(145, 255, 126, 255);
                    slotTransform.DOKill();
                    slotTransform.DOAnchorPos(_originalSlotPos[a] + new Vector2(0, 30f), 0.25f).SetEase(Ease.OutQuad);
                }
                else
                {
                    image.color = new Color32(219, 219, 219, 255);
                    slotTransform.DOKill();
                    slotTransform.DOAnchorPos(_originalSlotPos[a], 0.25f).SetEase(Ease.OutQuad);
                }
                a++;
            }
        }


    }
}
