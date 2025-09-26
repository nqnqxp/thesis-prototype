using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkingState : BaseState<PlayerStateType>
{
    private PlayerStateMachine sm;

    public PlayerWalkingState(PlayerStateMachine stateMachine) : base(PlayerStateType.Walking)
    {
        sm = stateMachine;
    }

    public override void EnterState()
    {
        sm.targetMaxSpeed = sm.speed;
    }

    public override void UpdateState()
    {
        Vector2 moveInput = sm.moveInput;

        Vector3 forward = sm.cameraTransform.forward;
        Vector3 right   = sm.cameraTransform.right;

        forward.y = 0;
        right.y   = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * moveInput.y + right * moveInput.x;
        moveDir.Normalize();
        

        //YOU WERE LOOKING AROUND HERE BC U HAD TO CHECK SPEED CALCULATION AND 


        // Accelerate / decelerate
        if (moveInput.magnitude > 0.1f)
        {
            sm.currentSpeed = Mathf.MoveTowards(sm.currentSpeed, sm.targetMaxSpeed, sm.acceleration * Time.deltaTime);
        }
        else
        {
            sm.TransitionToState(PlayerStateType.Idle);
            return;
        }

        sm.controller.Move(moveDir * sm.currentSpeed * Time.deltaTime);

        // Rotate toward movement direction
        if (sm.shouldFaceMoveDir && moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            sm.transform.rotation = Quaternion.Slerp(sm.transform.rotation, toRotation, 10f * Time.deltaTime);
        }

        // Animations
        sm.animator.SetFloat("speed", sm.currentSpeed / sm.targetMaxSpeed);
    }

    public override void ExitState() { }
    public override PlayerStateType GetNextState() => StateKey;
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}