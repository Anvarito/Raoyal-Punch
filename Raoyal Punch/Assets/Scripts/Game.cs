using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameState
{
    inFight,
    StopGame,
}

public class Game : MonoBehaviour
{
    private static EGameState gameState = EGameState.StopGame;
    [SerializeField] private Player Player;
    [SerializeField] private Enemy Enemy;
    [SerializeField] private HitPointBar HpEnemy;
    [SerializeField] private HitPointBar HpPlayer;
    public static EGameState GetGameState()
    {
        return gameState;
    }

    private void Awake()
    {
        gameState = EGameState.inFight;
        Fighter.OnFighterDefeat += GameStop;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GameStop(Fighter defeatedFighter)
    {
        gameState = EGameState.StopGame;
       
        if(defeatedFighter is Enemy)
        {
            Player.GameFinised();
        }

        if(defeatedFighter is Player)
        {
            Enemy.GameFinised();
        }

        HpEnemy.SetBarVisible(false);
        HpPlayer.SetBarVisible(false);
    }
}
