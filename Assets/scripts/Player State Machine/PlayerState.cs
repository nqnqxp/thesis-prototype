using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateMachine stateMachine;

    public PlayerState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    // This method is called once when the state is entered.
    public abstract void EnterState();

    // This method is called every frame to update the state's logic.
    public abstract void UpdateState();

    // This method is called once when the state is exited.
    public abstract void ExitState();
}
