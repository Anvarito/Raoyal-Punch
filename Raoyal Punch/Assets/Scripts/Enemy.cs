using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Fighter
{
    private Vector3 _currentLookVector;
    private Vector3 _currentVelocity;

    private float _timeTosuperPunch = 0;
    private float _durationToSuperPunch = 12;

    private bool _isSuperPunch = false;
    private int _superPunch5;

    [Header("Boss special")]
    [SerializeField] private float SuperPunchDistance = 10;
    [SerializeField] private float ShokWavePower = 20;

    private Player _player;
    protected override void Start()
    {
        base.Start();

        _player = Opponent as Player;
        _currentLookVector = transform.forward;
    }

    protected override void AssignAnimationToHash()
    {
        base.AssignAnimationToHash();
        _superPunch5 = Animator.StringToHash("super5");
    }

    protected override void Update()
    {
        if (Game.GetGameState() == EGameState.inFight )
        {
            if (!_isSuperPunch)
            {
                base.Update();

                if (GetDistantToOpponent() < SuperPunchDistance)
                {
                    _timeTosuperPunch += Time.deltaTime;
                    if (_timeTosuperPunch > _durationToSuperPunch)
                    {
                        _timeTosuperPunch = 0;
                        _isSuperPunch = true;
                        _animator.SetTrigger(_superPunch5);
                    }
                }
                else
                {
                    _timeTosuperPunch = 0;
                }
            }
        }
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
        if (GetDistantToOpponent() < SuperPunchDistance)
        {
            _player.TakeShockWave(ShokWavePower);
        }
    }

    public override void GameFinised()
    {
        _animator.SetBool(_fightAnimID, false);
    }

    protected override void Movement()
    {
        Vector3 rotationTarget = (Opponent.transform.position - transform.position).normalized;
        rotationTarget = new Vector3(rotationTarget.x, 0, rotationTarget.z);
        _currentLookVector = Vector3.SmoothDamp(_currentLookVector, rotationTarget, ref _currentVelocity, AnimSmoothTime);

        transform.rotation = Quaternion.LookRotation(_currentLookVector);
    }
}
