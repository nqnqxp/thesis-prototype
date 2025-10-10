using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerState
{
    private PlayerController playerController;
    public float newOffsetX;
    public float newDutch;

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
        //always get current offset and dutch
        Vector3 currentOffset = playerController.aimCamOffset.Offset;
        float currentDutch = playerController.aimCam.Lens.Dutch;

        if (playerController.isAimingLeft)
        {
            if (!playerController.isAimingRight)
            {
                playerController.aimCam.Priority = PlayerController.activePriority;
                playerController.tpcCam.Priority = PlayerController.inactivePriority;

                //if offset is close to target value, just make it the target value to reduce jittering from math lerp (same for dutch below)
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
