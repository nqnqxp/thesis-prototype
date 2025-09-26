using UnityEngine;

public class PlayerIdleState : BaseState<PlayerStateType>
{
    private PlayerStateMachine sm;

    public PlayerIdleState(PlayerStateMachine stateMachine) : base(PlayerStateType.Idle)
    {
        sm = stateMachine;
    }

    public override void EnterState()
    {
        // Reset horizontal speed
        sm.currentSpeed = 0f;
        sm.targetMaxSpeed = sm.speed;

        // Set idle animation
        sm.animator.SetFloat("speed", 0f);
    }

    public override void UpdateState()
    {
        Vector2 moveInput = sm.moveInput;

        // Transition to Walking
        if (moveInput.magnitude > 0.1f && !sm.isSprinting)
        {
            sm.TransitionToState(PlayerStateType.Walking);
            return;
        }

        // Transition to Sprinting
        if (moveInput.magnitude > 0.1f && sm.isSprinting)
        {
            sm.TransitionToState(PlayerStateType.Sprinting);
            return;
        }

        // Transition to Jumping
        if (sm.jumpPressed && sm.controller.isGrounded)
        {
            sm.TransitionToState(PlayerStateType.Jumping);
            return;
        }

        // Optional: small gravity to keep grounded
        if (!sm.controller.isGrounded)
        {
            sm.velocity.y += sm.gravity * Time.deltaTime;
            sm.controller.Move(sm.velocity * Time.deltaTime);
        }
    }

    public override void ExitState() { }

    public override PlayerStateType GetNextState() => StateKey;

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}