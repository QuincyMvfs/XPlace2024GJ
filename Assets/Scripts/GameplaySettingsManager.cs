using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplaySettingsManager : MonoBehaviour
{
    private TMP_Dropdown _dropdown;

    private void Awake()
    {
        // Sets max fps to screen 
        RefreshRate refreshRate = Screen.currentResolution.refreshRateRatio;
        int refreshRateInt = Mathf.RoundToInt((float)refreshRate.numerator / refreshRate.denominator);
        Application.targetFrameRate = refreshRateInt;

        _dropdown = GetComponent<TMP_Dropdown>();
        _dropdown.value = QualitySettings.GetQualityLevel()+ 1;
    }

    public void SetGraphicSettings(int qualityLevel)
    {
        QualitySettings.SetQualityLevel(qualityLevel + 1);
    }
}
