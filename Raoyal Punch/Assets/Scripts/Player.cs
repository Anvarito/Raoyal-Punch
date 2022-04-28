using System.Collections;
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
    private bool _isReadyToGetUp = false;

    private float _timerGetUpCurrent = 0;

    private Vector3[] bonesStartAnmPos = new Vector3[11];
    private Quaternion[] bonesStartAnmRot = new Quaternion[11];
    protected override void Awake()
    {
        base.Awake();
        _inputController = GetComponent<InputController>();
        _characterController = GetComponent<CharacterController>();
    }

    public override void ResetGame()
    {
        base.ResetGame();
        _characterController.enabled = true;
        _animator.SetLayerWeight(1, 1);
        _animator.SetLayerWeight(2, 1);
        _animator.SetLayerWeight(3, 0);
        _animator.SetBool(_winAnimID, false);
        _animator.SetBool(_fightAnimID, false);
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
        if (_isReadyToGetUp)
        {
            PlayerGetUp();
        }
        else
        {
            base.Update();
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
            _isReadyToGetUp = false;
            _animator.enabled = true;

            _animator.SetFloat(_moveXAnimID, 0);
            _animator.SetFloat(_moveZAnimID, 0);
            _isfighterDown = false;

            _characterController.enabled = true;
        }
    }

    public void TakeShockWave(float hitPoint)
    {
        TakeHit(hitPoint);
        _characterController.enabled = false;
        if (_isfighterDown)
            return;
        EnableRagdoll();
        HeadRigidbody.AddForce((HeadRigidbody.transform.position - Opponent.transform.position).normalized * 100, ForceMode.Impulse);
        StartCoroutine(ShokWaveCooldown());
    }

    protected override void EnableRagdoll()
    {
        _characterController.enabled = false;
        base.EnableRagdoll();
    }

    //Get Up
    private IEnumerator ShokWaveCooldown()
    {
        yield return new WaitForSeconds(2);
        _timerGetUpCurrent = 0;
        transform.position = new Vector3(Hips.position.x, 0, Hips.position.z);
        Hips.transform.parent = Armature.transform;
        _isReadyToGetUp = true;

        for (int i = 0; i < bonesCapturePos.Length; i++)
        {
            bonesStartAnmPos[i] = rigidbodies[i].transform.localPosition;
            bonesStartAnmRot[i] = rigidbodies[i].transform.localRotation;
            rigidbodies[i].isKinematic = true;
            rigidbodies[i].useGravity = false;
            colliders[i].enabled = false;
        }
    }
    public override void FighterWin()
    {
        base.FighterWin();
        _animator.SetLayerWeight(1, 0);
        _animator.SetLayerWeight(2, 0);
        _animator.SetLayerWeight(3, 1);
        _animator.SetBool(_winAnimID, true);
    }

}
