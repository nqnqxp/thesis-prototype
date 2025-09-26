using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private PlayerState currentState;
    public PlayerState CurrentState => currentState;

    public PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();

        currentState = new PlayerIdleState(this);
        currentState.EnterState();
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState();
        }
    }
    public void ChangeState(PlayerState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
}
