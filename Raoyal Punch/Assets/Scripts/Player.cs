using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputController))]
[RequireComponent(typeof(CharacterController))]
public class Player : Fighter
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float DuractionGetUp = 1.5f;

    private InputController _inputController;
    private CharacterController _characterController;

    private Vector2 _animationSmoothBlend;
    private Vector2 _currentVelocity;

    private int _moveXAnimID;
    private int _moveZAnimID;
    private int _winAnimID;


   // private bool _isShock = false;
    private bool _isGetUp = false;

    private float _timerGetUpCurrent = 0;

    private Vector3[] bonesStartAnmPos = new Vector3[11];
    private Quaternion[] bonesStartAnmRot = new Quaternion[11];
    protected override void Awake()
    {
        base.Awake();
        _inputController = GetComponent<InputController>();
        _characterController = GetComponent<CharacterController>();
    }


    protected override void AssignAnimationToHash()
    {
        base.AssignAnimationToHash();
        _moveXAnimID = Animator.StringToHash("MoveX");
        _moveZAnimID = Animator.StringToHash("MoveZ");
        _winAnimID = Animator.StringToHash("win");
    }
    protected override void Movement()
    {
        _animationSmoothBlend = Vector2.SmoothDamp(_animationSmoothBlend, _inputController.move, ref _currentVelocity, AnimSmoothTime);
        Vector3 moveDirection = new Vector3(_animationSmoothBlend.x, 0, _animationSmoothBlend.y);

        Vector3 dir = transform.TransformDirection(moveDirection);
        _characterController.Move(dir * moveSpeed * Time.deltaTime);

        Vector3 rotationTarget = (Opponent.transform.position - transform.position).normalized;
        rotationTarget = new Vector3(rotationTarget.x, 0, rotationTarget.z);
        transform.rotation = Quaternion.LookRotation(rotationTarget);

        _animator.SetFloat(_moveXAnimID, _animationSmoothBlend.x);
        _animator.SetFloat(_moveZAnimID, _animationSmoothBlend.y);
    }

    protected override void Update()
    {
        if (_isGetUp)
        {
            PlayerGetUp();
        }
        else
        {

            if (!_fighterDown)
            {
                base.Update();
            }
            else
            {
                transform.position = new Vector3(Hips.position.x, 0, Hips.position.z);
            }
        }
    }

    private void PlayerGetUp()
    {
        _timerGetUpCurrent += Time.deltaTime;
        float alpha = _timerGetUpCurrent / DuractionGetUp;

        for (int i = 0; i < bonesCapturePos.Length; i++)
        {
            colliders[i].transform.localPosition = Vector3.Lerp(bonesStartAnmPos[i], bonesCapturePos[i], alpha);
            colliders[i].transform.localRotation = Quaternion.Lerp(bonesStartAnmRot[i], bonesCaptureRot[i], alpha);
        }

        if (alpha >= 1)
        {
            _isGetUp = false;
            _animator.enabled = true;

            _animator.SetFloat(_moveXAnimID, 0);
            _animator.SetFloat(_moveZAnimID, 0);
            _fighterDown = false;

            _characterController.enabled = true;
        }
    }

    public void TakeShockWave(float hitPoint)
    {
        TakeHit(hitPoint);
        _characterController.enabled = false;
        if (_fighterDown)
            return;
        EnableRagdoll();
        HeadRigidbody.AddForce((HeadRigidbody.transform.position - Opponent.transform.position).normalized * 100, ForceMode.Impulse);
        StartCoroutine(ShokWaveCooldown());
    }

    //Get Up
    private IEnumerator ShokWaveCooldown()
    {
        yield return new WaitForSeconds(2);
        _timerGetUpCurrent = 0;
        Hips.transform.parent = Armature.transform;
        _isGetUp = true;

        for (int i = 0; i < bonesCapturePos.Length; i++)
        {
            bonesStartAnmPos[i] = rigidbodies[i].transform.localPosition;
            bonesStartAnmRot[i] = rigidbodies[i].transform.localRotation;
            rigidbodies[i].isKinematic = true;
            rigidbodies[i].useGravity = false;
            colliders[i].enabled = false;
        }
    }
    public override void GameFinised()
    {
        _animator.SetLayerWeight(1, 0);
        _animator.SetLayerWeight(2, 0);
        _animator.SetBool(_winAnimID, true);
        _animator.SetBool(_fightAnimID, false);
    }

}
