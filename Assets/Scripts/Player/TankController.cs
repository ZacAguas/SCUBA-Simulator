using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public float CurrentTankPressure {
        get => currentTankPressure;
        private set => Mathf.Max(0, currentTankPressure);
    }
    private float currentTankPressure; // backing field, should have no usages, use property instead
    
    [SerializeField] private float maxTankPressure; // max pressure of tank in bar
    [SerializeField] private float lowTankPercentageThreshold;  // point at which air in tank is considered 'low' eg. 0.33 = rule of thirds
    [SerializeField] private GameEvent onLowTankPressure; // triggered when tank reaches 'low' threshold
    [SerializeField] private GameEvent onOutOfAir; // triggered when tank reaches 0

    private void Start()
    {
        CurrentTankPressure = maxTankPressure;
    }

    private void CheckTankPressure()
    {
        if (CurrentTankPressure <= maxTankPressure * lowTankPercentageThreshold) // we have 'low' pressure
            onLowTankPressure.Invoke(); // raise low pressure event

        if (CurrentTankPressure == 0)
            onOutOfAir.Invoke();

    }
    private void FixedUpdate()
    {
        CheckTankPressure();
    }
    
}
