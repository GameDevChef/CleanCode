using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReceiver : MonoBehaviour
{
    [SerializeField] private KeyCode shootingButton;
    [SerializeField] private KeyCode runningButton;
    [SerializeField] private KeyCode reloadButton;

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private const string MOUSE_X = "Mouse X";
    private const string MOUSE_Y = "Mouse Y";


    public bool IsShooting { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsReloading { get; private set; }
    public bool MyProperty { get; private set; }
    public float HorizontalInput { get; private set; }
    public float VerticalInput { get; private set; }
    public float MouseInputX { get; private set; }
    public float MouseInputY { get; private set; }

    private void Update()
    {
        ReceiveAxisInput();
        ReceiveButtonsInput();
    }

    private void ReceiveAxisInput()
    {
        HorizontalInput = Input.GetAxis(HORIZONTAL);
        VerticalInput = Input.GetAxis(VERTICAL);
        MouseInputX = Input.GetAxis(MOUSE_X);
        MouseInputY = Input.GetAxis(MOUSE_Y);
    }

    private void ReceiveButtonsInput()
    {
        IsRunning = Input.GetKey(runningButton);
        IsShooting = Input.GetKey(shootingButton);
        IsReloading = Input.GetKeyDown(reloadButton);
    }
}
