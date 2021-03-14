using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform rifleTransformParent;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float minXRotation;
    [SerializeField] private float maxXRotation;

    private InputReciever inputReciever;
    private Rigidbody rigodbody;
    private float currentRotationY;
    private float currentRotationX;

    private void Awake()
    {
        inputReciever = GetComponent<InputReciever>();
        rigodbody = GetComponent<Rigidbody>();
        currentRotationY = transform.eulerAngles.y;
        currentRotationX = transform.eulerAngles.x;
    }
    private void FixedUpdate()
    {
        Vector3 worldMoveVector = CalculateWorldMoveVector();
        SetRigidbodyVelocity(worldMoveVector);
    }

    private void Update()
    {
        RotatePlayerObject();
    }

    private void SetRigidbodyVelocity(Vector3 worldMoveVector)
    {
        float moveSpeed = inputReciever.IsRunning ? runSpeed : walkSpeed;
        rigodbody.velocity = worldMoveVector.normalized * moveSpeed;
    }

    private Vector3 CalculateWorldMoveVector()
    {
        var moveVector = new Vector3(inputReciever.HorizontalInput, 0f, inputReciever.VerticalInput);
        var worldMoveVector = transform.TransformDirection(moveVector);
        worldMoveVector = new Vector3(worldMoveVector.x, 0f, worldMoveVector.z);
        return worldMoveVector;
    }

    private void RotatePlayerObject()
    {
        float yaw = inputReciever.MouseInputX * Time.deltaTime * rotationSpeed * mouseSensitivity;
        float pitch = inputReciever.MouseInputY * Time.deltaTime * rotationSpeed * mouseSensitivity;
        currentRotationY += yaw;
        currentRotationX -= pitch;
        currentRotationX = Mathf.Clamp(currentRotationX, minXRotation, maxXRotation);
        rifleTransformParent.localRotation = Quaternion.Euler(currentRotationX, 0, 0);
        transform.localRotation = Quaternion.Euler(0, currentRotationY, 0);
    }
}
