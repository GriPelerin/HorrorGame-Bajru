using Bajru.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    [SerializeField]private float _cardCooldownDuration = 5f;
    public bool IsCardOnCooldown { get; set; } = false;
    public static GameEvents Instance { get; private set; }

    public event Action<Card> OnCardPickedUp;
    public event Action<CardAbilityType, float> OnCardAbilityUsed;
    public event Action<MapPieceSO> OnMapPieceInteracted;
    public event Action OnCorrectPassword;
    public event Action<GameStates> OnGameStatesChanged;
    public event Action<PlayerMovement> OnPlayerSpawned;
    public event Action<Monster> OnMonsterSpawned;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //Subscribers : PlayerMovement, Monster
    public void TriggerCardAbility(CardAbilityType abilityType, float duration)
    {
        if(IsCardOnCooldown)
        {
            return;
        }
        IsCardOnCooldown = true;
        OnCardAbilityUsed?.Invoke(abilityType, duration);
        StartCoroutine(CardCooldownTimer());
    }

    //Subscribers : PlayerInventory
    public void TriggerCardPickedUp(Card card)
    {
        OnCardPickedUp?.Invoke(card);
    }
    public void TriggerMapPieceInteracted(MapPieceSO mapPieceData)
    {
        OnMapPieceInteracted?.Invoke(mapPieceData);
    }
    public void TriggerCorrectPassword()
    {
        OnCorrectPassword?.Invoke();
    }
    public void TriggerGameStatesChanged(GameStates state)
    {
        OnGameStatesChanged?.Invoke(state);
    }
    public void TriggerPlayerSpawned(PlayerMovement player)
    {
        OnPlayerSpawned?.Invoke(player);
    }
    public void TriggerMonsterSpawned(Monster monster)
    {
        OnMonsterSpawned?.Invoke(monster);
    }
    private IEnumerator CardCooldownTimer()
    {
        yield return new WaitForSeconds(_cardCooldownDuration);
        IsCardOnCooldown = false;
    }
}
