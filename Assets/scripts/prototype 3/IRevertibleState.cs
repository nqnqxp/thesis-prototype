using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IRevertibleState
{
    object CaptureState();

    void RestoreState(object snapshot);
}

[System.Serializable]
public class CombatantStateSnapshot
{
    public Vector3 position;
    public Quaternion rotation;
    public float currentHealth;
}
