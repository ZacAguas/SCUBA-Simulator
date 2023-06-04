using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private DepthManager depthManager;

    private void Awake()
    {
        depthManager = GetComponent<DepthManager>();
    }

    private void Start()
    {
    }
}
