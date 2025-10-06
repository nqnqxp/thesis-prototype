using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerState
{
    private PlayerController playerController;

    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        playerController = stateMachine.playerController;
    }

    public override void EnterState()
    {
        Debug.Log("Entering Idle State");
        playerController.animator.SetFloat("speed", 0f);
    }

    public override void UpdateState()
    {

        if (playerController.isAimingLeft || playerController.isAimingRight)
        {
            Debug.Log("Just aiming");
            if (playerController.leftFireInput || playerController.rightFireInput)
            {
                Debug.Log("Just shooting");
            }
        }

        if (playerController.moveInput.magnitude > 0.1f)
        {
            stateMachine.ChangeState(new PlayerRunningState(stateMachine));
        }

        if (playerController.jumpInput && playerController.controller.isGrounded)
        {
            stateMachine.ChangeState(new PlayerJumpingState(stateMachine));
        }

        playerController.velocity.y += playerController.gravity * Time.deltaTime;
        playerController.controller.Move(playerController.velocity * Time.deltaTime);

        // Reset jump input after checking.
        playerController.jumpInput = false;
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Idle State");
    }
}
