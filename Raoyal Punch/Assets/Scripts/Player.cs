using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputController))]
[RequireComponent(typeof(CharacterController))]
public class Player : Fighter
{
    [SerializeField] private float moveSpeed;

    private InputController _inputController;
    private CharacterController _characterController;
   
    private Vector2 _animationSmoothBlend;
    private Vector2 _currentVelocity;

    private int _moveXAnimID;
    private int _moveZAnimID;

    protected override void Awake()
    {
        base.Awake();
        _inputController = GetComponent<InputController>();
        _characterController = GetComponent<CharacterController>();

        _moveXAnimID = Animator.StringToHash("MoveX");
        _moveZAnimID = Animator.StringToHash("MoveZ");
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Movement();
    }

    private void Movement()
    {
        _animationSmoothBlend = Vector2.SmoothDamp(_animationSmoothBlend, _inputController.move, ref _currentVelocity, AnimSmoothTime);
        Vector3 moveDirection = new Vector3(_animationSmoothBlend.x, 0, _animationSmoothBlend.y);

        var t = transform.TransformDirection(moveDirection);
        _characterController.Move(t * moveSpeed * Time.deltaTime);

        Vector3 rotationTarget = (Opponent.transform.position - transform.position).normalized;
        rotationTarget = new Vector3(rotationTarget.x, 0, rotationTarget.z);
        transform.rotation = Quaternion.LookRotation(rotationTarget);

        _animator.SetFloat(_moveXAnimID, _animationSmoothBlend.x);
        _animator.SetFloat(_moveZAnimID, _animationSmoothBlend.y);
    }
}
