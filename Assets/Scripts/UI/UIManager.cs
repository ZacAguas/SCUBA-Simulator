using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    
    [SerializeField] private Slider bcdSlider;

    private void FixedUpdate()
    {
        bcdSlider.value = playerController.GetNormalisedCurrentBCDVolume();
    }
}
