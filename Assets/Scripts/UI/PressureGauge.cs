using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PressureGauge : MonoBehaviour
{
    [SerializeField] private GameObject needle;
    [SerializeField] private GameObject labelTemplate;

    private Vector3 startingRotation; // the rotation of the needle at the 0 o'clock position
    private float maxPressureAngle; // angle at 100% pressure, depends on startingRotation
    private float zeroPressureAngle; // angle at 0% pressure, depends on startingRotation
    private float totalAngleRange;


    private float maxPressure;
    private float currentPressure;

    private void Awake()
    {
        labelTemplate.SetActive(false);
    }

    private void Start()
    {
        var localEulerAngles = needle.transform.localEulerAngles;
        // for some reason the rotation is off
        startingRotation = new Vector3(localEulerAngles.x, localEulerAngles.y - 90, localEulerAngles.z + 90);
        
        maxPressureAngle = startingRotation.y + 120f;
        zeroPressureAngle = startingRotation.y - 120f;
        totalAngleRange = maxPressureAngle - zeroPressureAngle;
        currentPressure = maxPressure;
        
        // for testing
        maxPressure = 200;
        currentPressure = 200;
        
        CreateLabels();
    }

    private void Update()
    {
        currentPressure -= 30 * Time.deltaTime;
        if (currentPressure < 0)
            currentPressure = maxPressure;
        needle.transform.localEulerAngles = new Vector3(GetPressureRotation(), startingRotation.y, startingRotation.z);
    }

    private void CreateLabels()
    {
        int numLabels = 10;
        for (int i = 0; i <= numLabels; i++)
        {
            GameObject label = Instantiate(labelTemplate, transform);
            float normalisedLabelPressure = (float)i / numLabels;
            var labelAngle = zeroPressureAngle + normalisedLabelPressure * totalAngleRange;

            var angles = label.transform.localEulerAngles;
            label.transform.localEulerAngles = new Vector3(labelAngle, angles.y, angles.z);

            // gross hardcoded value NOTE: LABEL MUST BE CALLED "Label" in template
            label.transform.Find("Label").GetComponent<TextMeshPro>().text =
                Mathf.RoundToInt(normalisedLabelPressure * maxPressure).ToString();
            label.SetActive(true);
        }
        
    }
    
    private float GetPressureRotation()
    {
        float normalisedPressure = currentPressure / maxPressure; // normalise so gives a value between 0 and 1
        return zeroPressureAngle + normalisedPressure * totalAngleRange;
    }

}
