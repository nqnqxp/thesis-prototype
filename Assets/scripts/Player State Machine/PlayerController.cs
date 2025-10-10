using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;


public class PlayerController : MonoBehaviour
{
    [SerializeField] public Transform tpcCameraTransform;
    [SerializeField] public Transform aimCameraTransform;
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


    public Vector3 forward;
    public Vector3 right;

    [SerializeField] public CinemachineCamera tpcCam;
    [SerializeField] public CinemachineCamera aimCam;

    public CinemachineCameraOffset aimCamOffset;

    public const int activePriority = 20;
    public const int inactivePriority = 1;

    public const float aimRCamDutch = -4.35f;
    public const float aimLCamDutch = 4.35f;

    public const float aimBCamDutch = 0f;
    public Vector3 aimBCamOffset = new Vector3(0f, 0f, 0f);

    public Vector3 aimRCamOffset = new Vector3(0.66f, 0f, 0f);
    public Vector3 aimLCamOffset = new Vector3(-0.66f, 0f, 0f);

    //for camera blending
    public float transitionSpeed = 10f;

    // Combat State Variables
    public bool isAimingLeft;
    public bool isAimingRight;
    public bool isAimingGen;
    public bool isAimingBoth;
    
    public bool leftFireInput;
    public bool rightFireInput;

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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        aimCam.GetComponent<CinemachineCameraOffset>();
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

    //COMBAT
    
    public void AimLeft(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            isAimingLeft = true;
            isAimingGen = true;

            if (isAimingRight == true)
            {
                isAimingBoth = true;
            }
        }
        else if (context.canceled)
        {
            isAimingLeft = false;
            isAimingBoth = false;

            if (!isAimingRight)
            {
                isAimingGen = false;
            }
        }
        
    }

    public void AimRight(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isAimingRight = true;
            isAimingGen = true;
            if (isAimingLeft == true)
            {
                isAimingBoth = true;
            }
        }
        else if (context.canceled)
        {
            isAimingRight = false;
            isAimingBoth = false;

            if (!isAimingLeft)
            {
                isAimingGen = false;
            }

        }

        /*
        if (context.started)
        {
            isAimingRight = true;
            isAimingGen = true;
            aimCam.gameObject.SetActive(true);
            tpcCam.gameObject.SetActive(false);
        }
        else if (context.canceled)
        {
            isAimingRight = false;
            if (!isAimingLeft)
            {
                isAimingGen = false;
                aimCam.gameObject.SetActive(false);
                tpcCam.gameObject.SetActive(true);
            }
        }
        */
    }

    
    public void LeftFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            leftFireInput = true;
        }
        else if (context.canceled)
        {
            leftFireInput = false;
        }
    }

    public void RightFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            rightFireInput = true;
        }
        else if (context.canceled)
        {
            rightFireInput = false;
        }
    }

    
    void Update()
    {
        Debug.Log(isAimingGen);
    }
    

}