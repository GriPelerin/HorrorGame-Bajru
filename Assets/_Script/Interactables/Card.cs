using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bajru.Inventory
{
    public class Card : MonoBehaviour, IInteractable
    {
        [field: SerializeField] public ItemSO ItemScriptableObject { get; set; }
        [field: SerializeField] public CardAbilitySO CardAbility { get; set; }
        public string InteractableName { get { return $"Pick up the card"; } set => throw new System.NotImplementedException(); }

        public void Use(CardAbilityType cardAbility, float duration)
        {
            if (CardAbility != null)
            {
                CardAbility.Activate(cardAbility, duration);
            }
        }
        public void Interact()
        {
            GameEvents.Instance.TriggerCardPickedUp(this);
        }
    }
}
