using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunningState : PlayerState
{
    private PlayerController playerController;
    Transform cameraTransform;
    public float newOffsetX;
    public float newDutch;

    public PlayerRunningState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        playerController = stateMachine.playerController;
    }

    public override void EnterState()
    {
        Debug.Log("Entering Running State");
    }

    public override void UpdateState()
    {
        //always get current offset and dutch
        Vector3 currentOffset = playerController.aimCamOffset.Offset;
        float currentDutch = playerController.aimCam.Lens.Dutch;

        if (playerController.moveInput.magnitude < 0.1f)
        {
            stateMachine.ChangeState(new PlayerIdleState(stateMachine));
            return;
        }


        if (playerController.isAimingLeft)
        {
            if (!playerController.isAimingRight)
            {
                playerController.animator.SetBool("isAiming", true);
                playerController.aimCam.Priority = PlayerController.activePriority;
                playerController.tpcCam.Priority = PlayerController.inactivePriority;

                /*
                if (playerController.animator.GetCurrentAnimatorStateInfo(1).IsName("Left.drawLgun"))
                {
                    playerController.rigLayer.gameObject.SetActive(true);
                    playerController.LeftHandIK.gameObject.SetActive(true);
                    playerController.LeftGun.gameObject.SetActive(true);
                }
                */

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
                playerController.animator.SetTrigger("shootL");
            }
        }

        if (playerController.isAimingRight)
        {
            if (!playerController.isAimingLeft)
            {
                playerController.animator.SetBool("isDrawingRG", true);
                playerController.aimCam.Priority = PlayerController.activePriority;
                playerController.tpcCam.Priority = PlayerController.inactivePriority;

                /*
                if (playerController.animator.GetCurrentAnimatorStateInfo(2).IsName("Right.drawRgun"))
                {
                    playerController.rigLayer.gameObject.SetActive(true);
                    playerController.RightHandIK.gameObject.SetActive(true);
                    playerController.RightGun.gameObject.SetActive(true);
                }
                */

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
                playerController.animator.SetTrigger("shootR");
            }
        }

        if (playerController.isAimingBoth)
        {
            playerController.animator.SetBool("isAiming", true);
            //playerController.animator.SetBool("isDrawingRG", true);

            /*
            playerController.rigLayer.gameObject.SetActive(true);
            playerController.LeftHandIK.gameObject.SetActive(true);
            playerController.LeftGun.gameObject.SetActive(true);
            playerController.RightHandIK.gameObject.SetActive(true);
            playerController.RightGun.gameObject.SetActive(true);
            */

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
            /*
            playerController.animator.SetBool("isDrawingLG", false);
            playerController.animator.SetBool("isDrawingRG", false);
            playerController.rigLayer.gameObject.SetActive(false);
            playerController.LeftHandIK.gameObject.SetActive(false);
            playerController.LeftGun.gameObject.SetActive(false);
            playerController.RightHandIK.gameObject.SetActive(false);
            playerController.RightGun.gameObject.SetActive(false);
            */
        }

        if (playerController.jumpInput && playerController.controller.isGrounded)
        {
            Vector3 jforward = playerController.tpcCameraTransform.forward;
            Vector3 jright = playerController.tpcCameraTransform.right;

            jforward.y = 0;
            jright.y = 0;


            jforward.Normalize();
            jright.Normalize();

            playerController.moveDirAtJump = (jforward * playerController.moveInput.y + jright * playerController.moveInput.x).normalized;

            stateMachine.ChangeState(new PlayerJumpingState(stateMachine));
        }
        if (playerController.jumpInput && playerController.controller.isGrounded && playerController.isAimingGen)
        {
            Vector3 j2forward = playerController.aimCameraTransform.forward;
            Vector3 j2right = playerController.aimCameraTransform.right;

            j2forward.y = 0;
            j2right.y = 0;

            j2forward.Normalize();
            j2right.Normalize();


            playerController.moveDirAtJump = (j2forward * playerController.moveInput.y + j2right * playerController.moveInput.x).normalized;

            stateMachine.ChangeState(new PlayerJumpingState(stateMachine));
        }

        if (playerController.isAimingGen)
        {
            cameraTransform = playerController.aimCameraTransform;
        }
        else
        {
            cameraTransform = playerController.tpcCameraTransform;
        }

        playerController.forward = cameraTransform.forward;
        playerController.right = cameraTransform.right;

        playerController.forward.y = 0;
        playerController.right.y = 0;

        playerController.forward.Normalize();
        playerController.right.Normalize();

        Vector3 moveDir = playerController.forward * playerController.moveInput.y + playerController.right * playerController.moveInput.x;
        moveDir.Normalize();


        float targetMaxSpeed = playerController.isSprinting ? playerController.speed + 4f : playerController.speed;
        float currentSpeed = Mathf.MoveTowards(playerController.currentSpeed, targetMaxSpeed, playerController.acceleration * Time.deltaTime);
        
        playerController.currentSpeed = currentSpeed;

        playerController.controller.Move(moveDir * currentSpeed * Time.deltaTime);

        if (playerController.isAimingGen)
        {
            Vector3 aimDirection = playerController.aimCameraTransform.forward;
            aimDirection.y = 0;
            aimDirection.Normalize();

            if (aimDirection.sqrMagnitude > 0.001f)
            {
                Quaternion toRotation = Quaternion.LookRotation(aimDirection, Vector3.up);
                playerController.transform.rotation = Quaternion.Slerp(playerController.transform.rotation, toRotation, 10f * Time.deltaTime);
            }
        }
        else if (playerController.shouldFaceMoveDir && moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            playerController.transform.rotation = Quaternion.Slerp(playerController.transform.rotation, toRotation, 10f * Time.deltaTime);
        }

        if (playerController.shouldFaceMoveDir && moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            playerController.transform.rotation = Quaternion.Slerp(playerController.transform.rotation, toRotation, 10f * Time.deltaTime);
        }


        playerController.velocity.y += playerController.gravity * Time.deltaTime;
        playerController.controller.Move(playerController.velocity * Time.deltaTime);


        playerController.animator.SetFloat("speed", (currentSpeed/targetMaxSpeed));
        //Debug.Log(playerController.animator.speed);

        playerController.jumpInput = false;
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Running State");
    }

}
