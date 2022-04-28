using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Curtain")]
    [SerializeField] private Image Curtain;
    private float _durationCurtain = 1;
    private float _timerCurtain;
    [SerializeField] private AnimationCurve Evaluate;
    private bool _isLaunchCurtain = false;

    [Header("Cameras")]
    [SerializeField] private CameraMotion CamerasMotion;
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

        if (CamerasMotion != null)
            _durationCurtain = CamerasMotion.GetBlendSeconds();

        RestartGame();
    }

    void Update()
    {
        if (_isLaunchCurtain)
        {
            _timerCurtain += Time.deltaTime;
            _timerCurtain = Mathf.Clamp(_timerCurtain, 0, (_durationCurtain * 2));
            var time = _timerCurtain / (_durationCurtain * 2);
            var value = Evaluate.Evaluate(time);
            Curtain.color = new Color(0, 0, 0, value);

            _isLaunchCurtain = time < 1;
        }
    }

    private void GameStop(Fighter defeatedFighter)
    {
        gameState = EGameState.StopGame;

        if (defeatedFighter is Enemy)
        {
            Player.FighterWin();
            CamerasMotion.EnableFaceCamera(true);
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
        _isLaunchCurtain = true;
        _timerCurtain = 0;
        StartCoroutine(CameraAwait());
    }

    private IEnumerator CameraAwait()
    {
        if (CamerasMotion != null)
            CamerasMotion.EnableFaceCamera(false);
        yield return new WaitForSeconds(_durationCurtain);

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
