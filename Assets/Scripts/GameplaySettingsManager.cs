using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySettingsManager : MonoBehaviour
{
    [SerializeField] private QualityLevel _defaultQualityLevel = QualityLevel.Good;

    private void Awake()
    {
        // Sets max fps to screen 
        RefreshRate refreshRate = Screen.currentResolution.refreshRateRatio;
        int refreshRateInt = Mathf.RoundToInt((float)refreshRate.numerator / refreshRate.denominator);
        Application.targetFrameRate = refreshRateInt;

        SetGraphicSettings(_defaultQualityLevel);
    }

    public void SetGraphicSettings(QualityLevel qualityLevel)
    {
        QualitySettings.SetQualityLevel((int)qualityLevel);
    }
}
