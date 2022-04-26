using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputController))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{


    private InputController _inputController;
    private CharacterController _characterController;
    private Animator _animator;

    [SerializeField] private HitPointBar HP_bar;
    [SerializeField] private Enemy EnemyTarget;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float AnimSmoothTime;
    [SerializeField] private float AttackDistance;
    private float _distance;
    private Vector2 _animationSmoothBlend;
    private Vector2 _currentVelocity;

    private int _moveXAnimID;
    private int _moveZAnimID;
    private int _fightAnimID;

    private float _hitPower = 10;
    private void Awake()
    {
        _inputController = GetComponent<InputController>();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        _moveXAnimID = Animator.StringToHash("MoveX");
        _moveZAnimID = Animator.StringToHash("MoveZ");
        _fightAnimID = Animator.StringToHash("fight");
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        AttakAnimation();
    }

    private void Movement()
    {
        _animationSmoothBlend = Vector2.SmoothDamp(_animationSmoothBlend, _inputController.move, ref _currentVelocity, AnimSmoothTime);
        Vector3 moveDirection = new Vector3(_animationSmoothBlend.x, 0, _animationSmoothBlend.y);

        var t = transform.TransformDirection(moveDirection);
        _characterController.Move(t * moveSpeed * Time.deltaTime);

        Vector3 rotationTarget = (EnemyTarget.transform.position - transform.position).normalized;
        rotationTarget = new Vector3(rotationTarget.x, 0, rotationTarget.z);
        transform.rotation = Quaternion.LookRotation(rotationTarget);

        _animator.SetFloat(_moveXAnimID, _animationSmoothBlend.x);
        _animator.SetFloat(_moveZAnimID, _animationSmoothBlend.y);
    }
    public void AttakAnimation()
    {
         _distance = Vector3.Distance(transform.position, EnemyTarget.transform.position);
        _animator.SetBool(_fightAnimID, _distance < AttackDistance);
    }

    public void Attack()
    {
        if(_distance > AttackDistance)
        {
            return;
        }

        EnemyTarget.TakeHit(_hitPower);
    }
}
