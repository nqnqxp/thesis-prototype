using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateMachine stateMachine;

    public PlayerState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }


    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();
}
