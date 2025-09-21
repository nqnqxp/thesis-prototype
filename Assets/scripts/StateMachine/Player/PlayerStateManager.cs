/*
using UnityEngine;

public class PlayerStateManager : StateManager<PlayerState>
{
    public Animator Animator { get; private set; }
    public Rigidbody Rb { get; private set; }

    void Awake()
    {
        Animator = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody>();

        States = new Dictionary<PlayerState, BaseState<PlayerState>>()
        {
            { PlayerState.Idle, new PlayerIdleState(this) },
            { PlayerState.Run, new PlayerRunState(this) },
            { PlayerState.Jump, new PlayerJumpState(this) }
        };

        CurrentState = States[PlayerState.Idle];
    }
}

*/