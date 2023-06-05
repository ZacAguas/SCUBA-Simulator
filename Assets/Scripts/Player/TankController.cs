using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public float CurrentTankPressure {
        get => currentTankPressure;
        private set => Mathf.Max(0, value);
    }
    private float currentTankPressure; // backing field, should have no usages, use property instead

    public enum GasMix
    {
        Air,
        Nitrox32,
        Nitrox36,
        Trimix21_35,
        Trimix18_45,
        Trimix10_70
    }
    [SerializeField] private GasMix selectedGasMix;
    public GasMix GetGasMix() => selectedGasMix;
    public float oxygenPercentage;
    public float nitrogenPercentage;
    public float heliumPercentage;

    [SerializeField] private float maxTankPressure; // max pressure of tank in bar
    [SerializeField] private float lowTankPercentageThreshold;  // point at which air in tank is considered 'low' eg. 0.33 = rule of thirds
    
    [SerializeField] private GameEvent onLowTankPressure; // triggered when tank reaches 'low' threshold
    [SerializeField] private GameEvent onOutOfAir; // triggered when tank reaches 0

    private void Start()
    {
        CurrentTankPressure = maxTankPressure;
        InitialiseGasMix();
    }

    private void InitialiseGasMix()
    {
        switch (selectedGasMix)
        {
            case GasMix.Air:
                oxygenPercentage = 0.21f;
                nitrogenPercentage = 0.79f;
                heliumPercentage = 0f;
                break;
            case GasMix.Nitrox32:
                oxygenPercentage = 0.32f;
                nitrogenPercentage = 0.68f;
                heliumPercentage = 0f;
                break;
            case GasMix.Nitrox36:
                oxygenPercentage = 0.36f;
                nitrogenPercentage = 0.64f;
                heliumPercentage = 0f;
                break;
            case GasMix.Trimix21_35:
                oxygenPercentage = 0.21f;
                nitrogenPercentage = 0.44f;
                heliumPercentage = 0.35f;
                break;
            case GasMix.Trimix18_45:
                oxygenPercentage = 0.18f;
                nitrogenPercentage = 0.37f;
                heliumPercentage = 0.45f;
                break;
            case GasMix.Trimix10_70:
                oxygenPercentage = 0.10f;
                nitrogenPercentage = 0.2f;
                heliumPercentage = 0.7f;
                break;
        }
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
