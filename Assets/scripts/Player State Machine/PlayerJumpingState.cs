using UnityEngine;

public class PlayerJumpingState : PlayerState
{
    private PlayerController playerController;
    public float newOffsetX;
    public float newDutch;

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
        //always get current offset and dutch
        Vector3 currentOffset = playerController.aimCamOffset.Offset;
        float currentDutch = playerController.aimCam.Lens.Dutch;

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

                if (Mathf.Abs(currentOffset.x - playerController.aimLCamOffset.x) < 0.001f)
                {
                    playerController.aimCamOffset.Offset = playerController.aimLCamOffset;
                }
                else
                {
                    newOffsetX = Mathf.Lerp(currentOffset.x, playerController.aimLCamOffset.x, Time.deltaTime * playerController.transitionSpeed);
                }
                playerController.aimCamOffset.Offset = new Vector3(newOffsetX, currentOffset.y, currentOffset.z);

                if (Mathf.Abs(currentDutch - PlayerController.aimLCamDutch) < 0.001f)
                {
                    playerController.aimCam.Lens.Dutch = PlayerController.aimLCamDutch;
                }
                else
                {
                    newDutch = Mathf.Lerp(currentDutch, PlayerController.aimLCamDutch, Time.deltaTime * playerController.transitionSpeed);
                }

                playerController.aimCam.Lens.Dutch = newDutch;
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

                if (Mathf.Abs(currentOffset.x - playerController.aimRCamOffset.x) < 0.001f)
                {
                    playerController.aimCamOffset.Offset = playerController.aimRCamOffset;
                }
                else
                {
                    newOffsetX = Mathf.Lerp(currentOffset.x, playerController.aimRCamOffset.x, Time.deltaTime * playerController.transitionSpeed);
                }
                playerController.aimCamOffset.Offset = new Vector3(newOffsetX, currentOffset.y, currentOffset.z);

                if (Mathf.Abs(currentDutch - PlayerController.aimRCamDutch) < 0.001f)
                {
                    playerController.aimCam.Lens.Dutch = PlayerController.aimRCamDutch;
                }
                else
                {
                    newDutch = Mathf.Lerp(currentDutch, PlayerController.aimRCamDutch, Time.deltaTime * playerController.transitionSpeed);
                }

                playerController.aimCam.Lens.Dutch = newDutch;
            }

            if (playerController.rightFireInput)
            {
                Debug.Log("just shooting right");
            }
        }

        if (playerController.isAimingBoth)
        {
            if (Mathf.Abs(currentOffset.x - playerController.aimBCamOffset.x) < 0.001f)
            {
                playerController.aimCamOffset.Offset = playerController.aimBCamOffset;
            }
            else
            {
                newOffsetX = Mathf.Lerp(currentOffset.x, playerController.aimBCamOffset.x, Time.deltaTime * playerController.transitionSpeed);
            }
            playerController.aimCamOffset.Offset = new Vector3(newOffsetX, currentOffset.y, currentOffset.z);

            if (Mathf.Abs(currentDutch - PlayerController.aimBCamDutch) < 0.001f)
            {
                playerController.aimCam.Lens.Dutch = PlayerController.aimBCamDutch;
            }
            else
            {
                newDutch = Mathf.Lerp(currentDutch, PlayerController.aimBCamDutch, Time.deltaTime * playerController.transitionSpeed);
            }

            playerController.aimCam.Lens.Dutch = newDutch;
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
