using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DepthManager : MonoBehaviour
{

    public float Depth { get; private set; }
    
    private float prevDepth;
    private bool playerNarced = false;
    
    [SerializeField] private GameEvent onBecomeNarced; // triggered when player reaches certain depth determined by narcosisStartDepth

    [SerializeField] private Transform seaLevel;
    [SerializeField] private float narcosisThreshold; // the depth at which nitrogen narcosis starts

    private void Start()
    {
        CalculateDepth();
        prevDepth = Depth;
        playerNarced = false;

        Debug.Log("Start depth: " + Depth);
    }

    private void CalculateDepth()
    {
        Depth = seaLevel.position.y - transform.position.y;
    }

    private void CheckNarcosis()
    {
        if (Depth >= narcosisThreshold) // raise narced event if past threshold and not already narced
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
