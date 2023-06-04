using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DepthManager : MonoBehaviour
{
    private TankController tankController;
    
    public float Depth { get; private set; }
    
    private float prevDepth;
    private bool playerNarced = false;
    
    [SerializeField] private GameEvent onBecomeNarced; // triggered when player reaches certain depth determined by narcosisStartDepth

    [SerializeField] private Transform seaLevel;
    [SerializeField] private float narcosisAirThreshold; // the depth at which narcosis starts (FOR AIR)
    private float equivalentNarcoticDepth; // estimate depth at which gas mixture produces equivalent narcotic effect to air

    private void Awake()
    {
        tankController = GetComponent<TankController>();
    }

    private void Start()
    {
        CalculateDepth();
        prevDepth = Depth;
        playerNarced = false;
        
        CalculateEquivalentNarcoticDepth(); // careful with script execution order (percentages must be calculated in tank controller before this)
        
        Debug.Log("Start depth: " + Depth);
    }

    private void CalculateDepth()
    {
        Depth = seaLevel.position.y - transform.position.y;
    }

    private void CalculateEquivalentNarcoticDepth()
    {
        equivalentNarcoticDepth = Depth * (tankController.nitrogenPercentage + 2 * tankController.heliumPercentage);
    }

    private void CheckNarcosis()
    {
        if (Depth >= narcosisAirThreshold) // raise narced event if past threshold and not already narced
        {
            if (!playerNarced) // only invoke if not already narced
                onBecomeNarced.Invoke(); // do I need to check if this is null?
            playerNarced = true;
        }
        else
            playerNarced = false;
    }
    private void FixedUpdate() // for physics calculations
    {
        CalculateDepth();
        CheckNarcosis();


        prevDepth = Depth; // keep track of the depth last frame
    }


    // careful updating depth on update or fixed update for physics
}
