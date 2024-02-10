using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    [SerializeField] private float walkingSpeed = 5.0f;
    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float gravity = -9.18f;
    [SerializeField] private float jumpForce = 5.0f;

    [SerializeField] private Transform camHolder;
    
    private float verticalRotation = 0;
    private Vector3 smoothVelocity;

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Player movement
        float forwardSpeed = Input.GetAxis("Vertical") * walkingSpeed;
        float sideSpeed = Input.GetAxis("Horizontal") * walkingSpeed;

        Vector3 speed = new Vector3(sideSpeed, 0, forwardSpeed);
        speed = transform.rotation * speed;

        // Jumping
        if (characterController.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                float jumpSpeed = Mathf.Sqrt(2 * jumpForce * Mathf.Abs(Physics.gravity.y));
                smoothVelocity.y = jumpSpeed;
            }
        }

        // Apply gravity
        smoothVelocity.y += gravity * Time.deltaTime;

        // Smoothly apply vertical velocity
        Vector3 smoothedMovement = new Vector3(speed.x, smoothVelocity.y, speed.z);
        characterController.Move(smoothedMovement * Time.deltaTime);

        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation += mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        transform.Rotate(0, mouseX, 0);
        camHolder.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    public void SetCursorState(CursorLockMode mode, bool cursorVisible)
    {
        Cursor.lockState = mode;
        Cursor.visible = cursorVisible;
    }
}
