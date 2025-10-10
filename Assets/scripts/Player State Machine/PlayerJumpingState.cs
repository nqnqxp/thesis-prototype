using UnityEngine;

public class PlayerJumpingState : PlayerState
{
    private PlayerController playerController;

    public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        playerController = stateMachine.playerController;
    }

    public override void EnterState()
    {
        playerController.animator.SetBool("isGrounded", false);
        Debug.Log("Entering Jumping State");
        playerController.velocity.y = Mathf.Sqrt(playerController.jumpHeight * -2f * playerController.gravity);
        playerController.animator.SetTrigger("jumped");

    }

    public override void UpdateState()
    {
        Vector3 moveVector = playerController.moveDirAtJump * playerController.currentSpeed;
        moveVector.y = playerController.velocity.y;
        playerController.velocity.y += playerController.gravity * Time.deltaTime;
        playerController.controller.Move(moveVector * Time.deltaTime);

        if (playerController.isAimingLeft)
        {
            if (!playerController.isAimingRight)
            {
                playerController.aimCam.Priority = PlayerController.activePriority;
                playerController.tpcCam.Priority = PlayerController.inactivePriority;
            }

            if (playerController.leftFireInput)
            {
                Debug.Log("just shooting left");
            }
        }

        if (playerController.isAimingRight)
        {
            if (!playerController.isAimingLeft)
            {
                playerController.aimCam.Priority = PlayerController.activePriority;
                playerController.tpcCam.Priority = PlayerController.inactivePriority;
            }

            if (playerController.rightFireInput)
            {
                Debug.Log("just shooting right");
            }
        }


        if (!playerController.isAimingLeft && !playerController.isAimingRight)
        {
            playerController.aimCam.Priority = PlayerController.inactivePriority;
            playerController.tpcCam.Priority = PlayerController.activePriority;
        }

        if (playerController.controller.isGrounded && playerController.velocity.y < 0)
        {
            playerController.animator.SetBool("isGrounded", true);
            playerController.velocity.y = -2f;

            if (playerController.moveInput.magnitude < 0.1f)
            {
                stateMachine.ChangeState(new PlayerIdleState(stateMachine));
            }
            else
            {
                stateMachine.ChangeState(new PlayerRunningState(stateMachine));
            }
        }


    }

    public override void ExitState()
    {
        Debug.Log("Exiting Jumping State");

        playerController.jumpInput = false;
    }
}
