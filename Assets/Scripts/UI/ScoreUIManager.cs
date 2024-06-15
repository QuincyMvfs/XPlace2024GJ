using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUIManager : MonoBehaviour
{
    [SerializeField] private ScoreController _scoreController;

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _comboText;
    [SerializeField] private TextMeshProUGUI _streakText;

    private string _startScoreString;
    private string _startComboString;
    private string _startStreakString;

    private void Awake()
    {
        _startScoreString = _scoreText.text;
        _startComboString = _comboText.text;
        _startStreakString = _streakText.text;
    }

    public void UpdateScore(int value)
    {
        _scoreText.text = _startScoreString + value.ToString();
    }

    public void UpdateStreak(int value)
    {
        _streakText.text = _startStreakString + value.ToString();
    }

    public void UpdateCombo(float value)
    {
        _comboText.text = _startComboString + value.ToString();
    }
}