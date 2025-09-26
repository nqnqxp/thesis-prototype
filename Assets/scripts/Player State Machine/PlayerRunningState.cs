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


        if (playerController.jumpInput && playerController.controller.isGrounded)
        {
            Vector3 jforward = playerController.cameraTransform.forward;
            Vector3 jright = playerController.cameraTransform.right;

            jforward.y = 0;
            jright.y = 0;

            jforward.Normalize();
            jright.Normalize();

            playerController.moveDirAtJump = (jforward * playerController.moveInput.y + jright * playerController.moveInput.x).normalized;

            stateMachine.ChangeState(new PlayerJumpingState(stateMachine));
        }

        Vector3 forward = playerController.cameraTransform.forward;
        Vector3 right = playerController.cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * playerController.moveInput.y + right * playerController.moveInput.x;
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
