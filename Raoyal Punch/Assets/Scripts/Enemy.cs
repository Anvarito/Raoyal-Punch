using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(ShokWaveVisual))]
public class Enemy : Fighter
{

    private CapsuleCollider _collider;

    private Vector3 _currentLookVector;
    private Vector3 _currentVelocity;


    private bool _isSuperPunch = false;
    private int _superPunch5;

    [Header("Boss special")]
    [SerializeField] private float SuperPunchDistance = 10;
    [SerializeField] private float ShokWavePower = 20;
    private float _timeTosuperPunch = 0;
    [SerializeField] private float _durationToSuperPunch = 12;
    private ShokWaveVisual _shokWaveVisual;

    private Player _player;

    public bool PLAYERNEAR = false;
    protected override void Start()
    {
        base.Start();

        _player = Opponent as Player;
        _currentLookVector = transform.forward;
        _collider = GetComponent<CapsuleCollider>();
        _shokWaveVisual = GetComponent<ShokWaveVisual>();
    }

    protected override void AssignAnimationToHash()
    {
        base.AssignAnimationToHash();
        _superPunch5 = Animator.StringToHash("super5");
    }

    public override void ResetGame()
    {
        GetComponent<Collider>().enabled = true;
        base.ResetGame();
    }

    protected override void Update()
    {
        if (Game.GetGameState() == EGameState.inFight)
        {
            if (!_isSuperPunch)
            {
                base.Update();

                if (GetDistantToOpponent() < SuperPunchDistance)
                {
                    PLAYERNEAR = true;
                    _timeTosuperPunch += Time.deltaTime;
                    if (_timeTosuperPunch > _durationToSuperPunch)
                    {
                        _timeTosuperPunch = 0;
                        _isSuperPunch = true;
                        _animator.SetTrigger(_superPunch5);
                        _shokWaveVisual.Launch();
                    }
                }
                else
                {
                    PLAYERNEAR = false;
                    _timeTosuperPunch = 0;
                }
            }
        }
    }

    protected override void EnableRagdoll()
    {
        GetComponent<Collider>().enabled = false;
        base.EnableRagdoll();
    }

    //Called in event from animation
    public void SuperPunchAnimPause()
    {
        StartCoroutine(Pause(1.5f));
    }

    private IEnumerator Pause(float pauseTime)
    {
        _animator.speed = 0;
        yield return new WaitForSeconds(pauseTime);
        _animator.speed = 1;
    }
    //Called in event from animation
    public void CooldownIsOver()
    {
        _isSuperPunch = false;
        _timeTosuperPunch = 0;
    }

    //Called in event from animation
    public void SupePunchShockWave()
    {
        _shokWaveVisual.CircleHide();

        if (GetDistantToOpponent() < SuperPunchDistance)
        {
            _player.TakeShockWave(ShokWavePower);
        }
    }

    protected override void Movement()
    {
        Vector3 rotationTarget = (Opponent.transform.position - transform.position).normalized;
        rotationTarget = new Vector3(rotationTarget.x, 0, rotationTarget.z);
        _currentLookVector = Vector3.SmoothDamp(_currentLookVector, rotationTarget, ref _currentVelocity, AnimSmoothTime);

        transform.rotation = Quaternion.LookRotation(_currentLookVector);
    }
}
