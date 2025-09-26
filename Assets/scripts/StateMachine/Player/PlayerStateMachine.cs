using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerStateMachine : StateMachine<PlayerStateType>
{
    [Header("References")]
    public Transform cameraTransform;
    public CharacterController controller;
    public Animator animator;

    [Header("Movement Settings")]
    public float speed = 5f;
    public float sprintSpeed = 9f;
    public float acceleration = 5f;
    public float deceleration = 15f;
    public float jumpHeight = 2f;
    public float gravity = -9.8f;
    public bool shouldFaceMoveDir = true;

    // Input values
    public Vector2 moveInput;
    public bool isSprinting;
    public bool jumpPressed;

    // Internal movement state
    [HideInInspector] public Vector3 velocity;
    [HideInInspector] public float currentSpeed;
    [HideInInspector] public float targetMaxSpeed;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        States[PlayerStateType.Idle] = new PlayerIdleState(this);
        States[PlayerStateType.Walking] = new PlayerWalkingState(this);
        //States[PlayerStateType.Sprinting] = new PlayerSprintingState(this);
        //States[PlayerStateType.Jumping] = new PlayerJumpingState(this);

        CurrentState = States[PlayerStateType.Idle];
    }

    // Input callbacks
    public void OnMove(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>();
    public void OnSprint(InputAction.CallbackContext ctx) => isSprinting = ctx.ReadValueAsButton();
    public void OnJump(InputAction.CallbackContext ctx) => jumpPressed = ctx.performed;
}