using System;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class ThirdPersonShooter : MonoBehaviour
{
    [SerializeField] private CinemachineCamera aimCam;

    private PlayerControls controls;

    void Start()
    {
        controls = new PlayerControls();
        controls.Enable();
    }

    private void Update()
    {
        
    }
    
}
