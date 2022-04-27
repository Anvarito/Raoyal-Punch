using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    
    public Vector2 move { get; private set; }

    private PlayerInput inputs;

    public static bool testTrigger = false;
    private void OnEnable()
    {
        inputs.Player.Enable();
    }

    private void OnDisable()
    {
        inputs.Player.Disable();
    }

    private void Awake()
    {
        inputs = new PlayerInput();

        inputs.Player.Move.performed += context => move = context.ReadValue<Vector2>();
        inputs.Player.Move.canceled += context => move = Vector2.zero;


    }

    private void Update()
    {
        testTrigger = inputs.Player.Test.triggered;
    }
}
