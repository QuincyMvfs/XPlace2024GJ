using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySettingsManager : MonoBehaviour
{
    private void Awake()
    {
        // Sets max fps to screen 
        RefreshRate refreshRate = Screen.currentResolution.refreshRateRatio;
        int refreshRateInt = Mathf.RoundToInt((float)refreshRate.numerator / refreshRate.denominator);
        Application.targetFrameRate = refreshRateInt;
    }
}
