using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class FutureSightManager : MonoBehaviour
{
    public float SimulationDuration = 2.0f;

    private List<IRevertibleState> revertibleObjects = new List<IRevertibleState>();
    private Dictionary<IRevertibleState, object> savedStates = new Dictionary<IRevertibleState, object>();

    void Start()
    {
        revertibleObjects.AddRange(FindObjectsByType<Combatant>(FindObjectsSortMode.None));

        if (revertibleObjects.Count == 0)
        {
            Debug.LogError("FUTURE SIGHT MANAGER: No 'Combatant' scripts found in the scene! The ability won't work.");
        }
    }

    //change to use input system in future
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(ActivateFutureSight());
        }
    }

    private IEnumerator ActivateFutureSight()
    {
        Debug.Log("FUTURE SIGHT ACTIVATED: Capturing State...");

        CaptureAllStates();

        SetAllCombatantsSimulationMode(true);

        Debug.Log("SIMULATION RUNNING: The next " + SimulationDuration + " seconds are just a vision.");

        yield return new WaitForSeconds(SimulationDuration);

        Debug.Log("SIMULATION ENDED: Reverting to initial state.");


        RestoreAllStates();

        SetAllCombatantsSimulationMode(false);

        Debug.Log("FUTURE SIGHT COMPLETE: Ready for real combat.");
    }

    private void CaptureAllStates()
    {
        savedStates.Clear();
        foreach (var obj in revertibleObjects)
        {
            savedStates[obj] = obj.CaptureState();
        }
    }

    private void RestoreAllStates()
    {
        foreach (var entry in savedStates)
        {
            entry.Key.RestoreState(entry.Value);
        }
    }

    private void SetAllCombatantsSimulationMode(bool isSimulating)
    {
        foreach (var obj in revertibleObjects)
        {
            if (obj is Combatant combatant)
            {
                combatant.SetSimulationMode(isSimulating);
            }
        }
    }
}