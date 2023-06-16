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
    [SerializeField] private float swimSpeed;
    
    // world
    private const float waterDensity = 1030; // in kg/m^3
    
    // body
    [SerializeField] private float bodyMass; // in kg
    private float bodyVolume; // in cubic meters
    // BCD
    [SerializeField] private float maxSurfaceBCDVolume;
    private float currentBCDVolume;



    private float yMouseRotation;
    private float xMouseRotation;
    
    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        depthManager = GetComponent<DepthManager>();
        rb = GetComponent<Rigidbody>();


        rb.mass = bodyMass;
        bodyVolume = CalculateBodyVolume();
    }

    private void Start()
    {
    }

    private void Update()
    {
        MouseMovement();
    }

    private void FixedUpdate()
    {
        SwimMovement();
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

    private void Buoyancy()
    {
        float gravity = Mathf.Abs(Physics.gravity.y);
        float totalVolume = currentBCDVolume + bodyVolume;
        float depth = depthManager.Depth;
        float buoyantForce = waterDensity * totalVolume * gravity;
        rb.AddForce(Vector3.up * buoyantForce);
    }

    private void SwimMovement()
    {
        Vector3 swimInput = inputManager.GetSwimInput();
        rb.AddRelativeForce(swimInput * swimSpeed); // relative force applies the force according to local rotation

    }

    private float CalculateBodyVolume()
    {
        // note: this is an estimate
        return bodyMass / 1000f; // in cubic meters
    }
}
