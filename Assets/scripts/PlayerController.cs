using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float deceleration = 15f;  

    [SerializeField] private float jumpHeight = 0.5f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private bool shouldFaceMoveDir = false;

    private float currentSpeed = 0f;
    private float targetMaxSpeed; 

    private CharacterController controller;
    private Vector2 moveInput;
    private bool isSprinting;
    private Vector3 velocity;

    public Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        targetMaxSpeed = speed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        //Debug.Log($"Move Input: {moveInput}");

    }

    public void Sprint(InputAction.CallbackContext context)
    {
        //Debug.Log($"Sprinting {context.performed}");
        if (context.started)
        {
            Debug.Log("Sprint started");
            animator.SetBool("isSprinting", true);
            isSprinting = true;
        }
        else if (context.canceled)
        {
            Debug.Log("Sprint canceled");
            animator.SetBool("isSprinting", false);
            isSprinting = false;
        }
        //Debug.Log($"Sprint Input: {isSprinting}");
    }

    public void Jump(InputAction.CallbackContext context)
    {
        //Debug.Log($"Jumping {context.performed} - Is Grounded: {controller.isGrounded}");
        if (context.performed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("jumped");
            
        }
    }

    void Update()
    {
        
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * moveInput.y + right * moveInput.x;
        moveDir.Normalize();


        if (moveInput.magnitude > 0.1f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetMaxSpeed, acceleration * Time.deltaTime);
            
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        targetMaxSpeed = isSprinting ? speed + 4f : speed;

        controller.Move(moveDir * currentSpeed * Time.deltaTime);

        //rotate toward movement direction
        if (shouldFaceMoveDir && moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }
        
        //deals with jumping
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


        //move animations
        animator.SetFloat("speed", currentSpeed / targetMaxSpeed);
        
        Debug.Log(currentSpeed);
        
    }
}
