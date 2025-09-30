using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;


public class PlayerController : MonoBehaviour
{
    [SerializeField] public Transform cameraTransform;
    [SerializeField] public float speed = 5f;
    [SerializeField] public float acceleration = 5f;
    [SerializeField] public float deceleration = 15f;

    [SerializeField] public float jumpHeight = 2f;
    [SerializeField] public float gravity = -9.8f;
    [SerializeField] public bool shouldFaceMoveDir = false;

    public float currentSpeed = 0f;
    private float targetMaxSpeed;

    public CharacterController controller;
    public Vector2 moveInput;
    public bool isSprinting;
    public Vector3 velocity;
    public bool jumpInput;
    public Vector3 moveDirAtJump;

    /* Combat State Variables
    public bool isAiming;
    public Vector2 mouseDeltaInput;
    public float leftChargeInput;
    public float rightChargeInput;
    public bool leftFireInput;
    public bool rightFireInput;
     
    // Gun Variables
    public float leftGunCharge = 0f;  // Charged by Forward
    public float rightGunCharge = 0f; // Charged by Backward
    */

    public Animator animator;

    private PlayerStateMachine stateMachine;

    void Start()
    {

        controller = GetComponent<CharacterController>();

        stateMachine = GetComponent<PlayerStateMachine>();
        if (stateMachine == null)
        {
            stateMachine = gameObject.AddComponent<PlayerStateMachine>();
        }

        stateMachine.playerController = this;

        targetMaxSpeed = speed;
    }

    //MOVEMENT (MOVE SPRINT JUMP)
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isSprinting = true;
            animator.SetBool("isSprinting", true);
        }
        else if (context.canceled)
        {
            isSprinting = false;
            animator.SetBool("isSprinting", false);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpInput = true;
        }
    }

    /*COMBAT
    
    public void Aim(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isAiming = true;
        }
        else if (context.canceled)
        {
            isAiming = false;
        }
    }

    public void Look(InputAction.CallbackContext context)
    {
        mouseDeltaInput = context.ReadValue<Vector2>();
    }

    public void leftCharge(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            leftChargeInput = Mathf.Min(0.0f + 0.2f * Time.deltaTime, 1.0f);
        }
        else if (context.canceled)
        {
            leftChargeInput = 0f;
        }

    }

    public void rightCharge(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            rightChargeInput = Mathf.Min(0.0f + 0.2f * Time.deltaTime, 1.0f);
        }
        else if (context.canceled)
        {
            rightChargeInput = 0f;
        }

    }

    public void LeftFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            leftFireInput = true;
        }
    }

    public void RightFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rightFireInput = true;
        }
    }
    */
}