using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardAbilityType 
{ 
  Blessing, //Increase player movement speed
  Curse,    //Decrease player size
  Stall,    //Decrease monster movement speed
  Gamble,
  Reveal    //Reveal monster location on the minimap
}

[CreateAssetMenu(fileName = "NewCardAbility", menuName = "CardAbilities/New Ability")]
public class CardAbilitySO : ScriptableObject
{
    public CardAbilityType abilityType;
    public float duration;

    public void Activate(CardAbilityType abilityType, float duration)
    {
        GameEvents.Instance.TriggerCardAbility(abilityType, duration);
    }
}
