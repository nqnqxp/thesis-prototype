using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

// This class represents the Running state of the player.
public class PlayerRunningState : PlayerState
{
    private PlayerController playerController;

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

        if (playerController.moveInput.magnitude < 0.1f)
        {
            stateMachine.ChangeState(new PlayerIdleState(stateMachine));
            return;
        }

        if (playerController.isAimingLeft || playerController.isAimingRight)
        {
            Debug.Log("Running and Aiming");
            //return;
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

            playerController.forward = playerController.aimCameraTransform.forward;
            playerController.right = playerController.aimCameraTransform.right;

        }
        else
        {
            playerController.forward = playerController.tpcCameraTransform.forward;
            playerController.right = playerController.tpcCameraTransform.right;
        }

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

        if (playerController.shouldFaceMoveDir && moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            playerController.transform.rotation = Quaternion.Slerp(playerController.transform.rotation, toRotation, 10f * Time.deltaTime);
        }


        playerController.velocity.y += playerController.gravity * Time.deltaTime;
        playerController.controller.Move(playerController.velocity * Time.deltaTime);


        playerController.animator.SetFloat("speed", currentSpeed / targetMaxSpeed);

        playerController.jumpInput = false;
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Running State");
    }
}
