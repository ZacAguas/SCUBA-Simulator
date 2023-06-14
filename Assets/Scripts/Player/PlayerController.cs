using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DepthManager))]
public class PlayerController : MonoBehaviour
{
    private InputManager inputManager;
    private DepthManager depthManager;
    private Rigidbody rb;


    [SerializeField] private float mouseSensitivity;
    [SerializeField] private bool clampVerticalLook;

    private float yMouseRotation;
    private float xMouseRotation;
    
    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        depthManager = GetComponent<DepthManager>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        MouseMovement();
        
    }

    private void MouseMovement()
    {
        Vector2 mouseInput = inputManager.GetMouseInput();
        yMouseRotation += mouseInput.x * mouseSensitivity * Time.deltaTime;
        xMouseRotation -= mouseInput.y * mouseSensitivity * Time.deltaTime;
        
        if (clampVerticalLook)
            xMouseRotation = Mathf.Clamp(xMouseRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xMouseRotation, yMouseRotation, 0);
    }
}
