using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int health;

    private int _currentHealth;

    private void Start()
    {
        _currentHealth = health;
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;

        if(_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameEvents.Instance.TriggerGameStatesChanged(GameStates.GameLose);
    }
}
