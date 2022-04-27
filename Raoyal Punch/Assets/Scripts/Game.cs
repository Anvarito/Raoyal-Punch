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
    [SerializeField] private GameObject RestartButton;

    private Vector3 _playerStartPosition;
    private Quaternion _playerStartRotation;
    private Vector3 _enemyStartPosition;
    private Quaternion _enemyStartRotation;
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
        _playerStartPosition = Player.transform.position;
        _playerStartRotation = Player.transform.rotation;
        _enemyStartPosition = Enemy.transform.position;
        _enemyStartRotation = Enemy.transform.rotation;

        RestartGame();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GameStop(Fighter defeatedFighter)
    {
        gameState = EGameState.StopGame;

        if (defeatedFighter is Enemy)
        {
            Player.FighterWin();
        }

        if (defeatedFighter is Player)
        {
            Enemy.FighterWin();
        }

        HpEnemy.SetBarVisible(false);
        HpPlayer.SetBarVisible(false);
        RestartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        RestartButton.gameObject.SetActive(false);
        Player.transform.position = _playerStartPosition;
        Player.transform.rotation = _playerStartRotation;
        Enemy.transform.position = _enemyStartPosition;
        Enemy.transform.rotation = _enemyStartRotation;

        Player.ResetGame();
        Enemy.ResetGame();

        HpEnemy.SetBarVisible(true);
        HpPlayer.SetBarVisible(true);

        gameState = EGameState.inFight;
    }
}
