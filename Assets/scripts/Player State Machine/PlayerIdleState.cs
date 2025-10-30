using GLTFast.Schema;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
//using UnityEngine.InputSystem;

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
        playerController.animator.SetBool("isSprinting", false);

    }

    public override void UpdateState()
    {
        //always get current offset and dutch
        Vector3 currentOffset = playerController.aimCamOffset.Offset;
        float currentDutch = playerController.aimCam.Lens.Dutch;

        //Debug.Log("Current State Hash: " + playerController.animator.GetCurrentAnimatorStateInfo(1).fullPathHash);

        if (playerController.isAimingLeft)
        {
            if (!playerController.isAimingRight)
            {

                playerController.animator.SetBool("isAiming",true);
                playerController.aimCam.Priority = PlayerController.activePriority;
                playerController.tpcCam.Priority = PlayerController.inactivePriority;

                /*
                if (playerController.animator.GetCurrentAnimatorStateInfo(1).IsName("Left.drawLgun"))
                {
                    Debug.Log("drawing left gun");
                    playerController.rigLayer.gameObject.SetActive(true);
                    playerController.LeftHandIK.gameObject.SetActive(true);
                    playerController.LeftGun.gameObject.SetActive(true);
                }
                */

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
                playerController.animator.SetTrigger("shootL");
            }
        }

        if (playerController.isAimingRight)
        {
            if (!playerController.isAimingLeft)
            {
                playerController.animator.SetBool("isAiming", true);
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
            //playerController.animator.SetBool("isDrawingLG", false);
            //playerController.animator.SetBool("isDrawingRG", false);
            //playerController.rigLayer.gameObject.SetActive(false);
            //playerController.LeftHandIK.gameObject.SetActive(false);
            //playerController.LeftGun.gameObject.SetActive(false);
            //playerController.RightHandIK.gameObject.SetActive(false);
            //playerController.RightGun.gameObject.SetActive(false);
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
