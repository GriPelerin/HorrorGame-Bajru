using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStates
{
    GameActive,
    GamePause,
    GameLose,
    GameWon

}
public class GameManager : MonoBehaviour
{
    public PlayerMovement Player { get; private set; }
    public Monster Monster { get; private set; }
    public GameStates CurrentGameState { get; set; }

    public static GameManager Instance;

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
    private void OnEnable()
    {
        GameEvents.Instance.OnGameStatesChanged += SetNewGameState;
        GameEvents.Instance.OnPlayerSpawned += RegisterPlayer;
        GameEvents.Instance.OnMonsterSpawned += RegisterMonster;
    }
    private void OnDisable()
    {
        GameEvents.Instance.OnGameStatesChanged -= SetNewGameState;
        GameEvents.Instance.OnPlayerSpawned -= RegisterPlayer;
        GameEvents.Instance.OnMonsterSpawned -= RegisterMonster;
    }
    private void Start()
    {
        CurrentGameState = GameStates.GameActive;
    }

    public void SetNewGameState(GameStates states)
    {
        CurrentGameState = states;
    }
    public void RegisterPlayer(PlayerMovement player)
    {
        Player = player;
    }
    public void RegisterMonster(Monster monster)
    {
        Monster = monster;
    }
}
