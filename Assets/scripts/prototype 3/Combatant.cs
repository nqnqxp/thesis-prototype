using UnityEngine;

public class Combatant : MonoBehaviour, IRevertibleState
{
    public float Health = 100f;
    public bool IsSimulatingFuture { get; private set; } = false;

    public object CaptureState()
    {
        return new CombatantStateSnapshot
        {
            position = transform.position,
            rotation = transform.rotation,
            currentHealth = Health
        };
    }

    public void RestoreState(object snapshot)
    {
        if (snapshot is CombatantStateSnapshot state)
        {
            transform.position = state.position;
            transform.rotation = state.rotation;

            Health = state.currentHealth;

            Debug.Log($"Restored state for {gameObject.name}. Health is now {Health}");
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (IsSimulatingFuture)
        {
            
            Debug.Log($"SIMULATION: {gameObject.name} would have taken {damageAmount} damage.");
        }
        else
        {

            Health -= damageAmount;
            if (Health <= 0) Die();
        }
    }

    public void SetSimulationMode(bool isSimulating)
    {
        IsSimulatingFuture = isSimulating;
    }

    private void Die()
    {
        Debug.Log("dead");
    }
}