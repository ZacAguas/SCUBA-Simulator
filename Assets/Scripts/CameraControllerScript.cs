using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerScript : MonoBehaviour
{
    [SerializeField] private Vector2 sensitivity; // max speed, negative y component to un-invert looking
    [SerializeField] private Vector2 acceleration; // in degrees/sec
    [SerializeField] private float inputLagInterval; // the interval at which we change the input value, set as low as possible whilst avoiding stuttering
    [SerializeField] private bool enableVerticalLookClamping; // whether or not we should clamp the vertical rotation 
    [SerializeField] private float clampAngle; // how far we can look up if we choose to clamp vertical look
    
    private Vector2 _rotation; // current rotation in degrees
    private Vector2 _velocity; // current rotation velocity in degrees
    private Vector2 _lastInput; // the last non-zero input received
    private float _inputLagTime; // time since last non-zero input
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }

    private void OnEnable() // Used for when we re-enable this script after having disabled camera movement
    {
        // Reset state
        _velocity = Vector2.zero;
        _inputLagTime = 0;
        _lastInput = Vector2.zero;
        
        // Calc current rotation
        Vector3 eulerRotation = transform.localEulerAngles;
        // Euler angles are [0, 360] but we want [-180, 180]
        if (eulerRotation.x >= 180)
            eulerRotation.x -= 360;
        
        if(enableVerticalLookClamping)
            eulerRotation.x = ClampAngle(eulerRotation.x);
        
        // update the rotation
        transform.localEulerAngles = eulerRotation;
        _rotation = new Vector2(eulerRotation.y, eulerRotation.x);
    }

    private Vector2 GetInput()
    {
        _inputLagTime += Time.deltaTime; // add to time since last good input
        
        Vector2 input = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y"));
        
        // if input values are non-zero or we've exceeded the waiting time (ie. mouse is moving or player truly isn't moving mouse)
        if((Mathf.Approximately(0, input.x) && Mathf.Approximately(0, input.y)) == false || _inputLagTime >= inputLagInterval)
        {
            _lastInput = input;
            _inputLagTime = 0; // we've just had a good input so reset timer
        }

        return _lastInput; // returns the last good input
    }

    private float ClampAngle(float angle)
    {
        return Mathf.Clamp(angle, -clampAngle, clampAngle); // restricts look to the clamp angle up & down
    }

    private void Update()
    {
        Vector2 desiredVelocity = GetInput() * sensitivity; // how far we want to move from our current rotation this frame
        _velocity = new Vector2(
            Mathf.MoveTowards(_velocity.x, desiredVelocity.x, acceleration.x * Time.deltaTime), 
            Mathf.MoveTowards(_velocity.y, desiredVelocity.y, acceleration.y * Time.deltaTime));
        
        _rotation += _velocity * Time.deltaTime;
        if (enableVerticalLookClamping)
            _rotation.y = ClampAngle(_rotation.y);

        transform.localEulerAngles = new Vector3(_rotation.y, _rotation.x, 0);
    }
}
