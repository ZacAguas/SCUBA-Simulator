using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    private UIDocument doc;
    private ProgressBar pressureGauge;
    private void Start()
    {
        doc = GetComponent<UIDocument>();
        // pressureGauge.highValue
    }
}
