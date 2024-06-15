using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class ScoreController : MonoBehaviour
{
    [Header("Score Variables")]
    [SerializeField] private float _baseScore = 25;
    [SerializeField] private float _baseMultiplier = 1.0f;
    [SerializeField] public List<float> _multiplierMap = new List<float>();
    [SerializeField] private ScoreUIManager _scoreUIManager;

    private int _currentScore = 0;
    private int _currentStreak = 0;
    private float _currentMultiplier = 0;

    public int CurrentScore => _currentScore;
    public int CurrentStreak => _currentStreak;
    public float CurrentMultiplier => _currentMultiplier;

    public void AddScore()
    {
        if (_currentStreak <= _multiplierMap.Count - 1)
        {
            float value = _multiplierMap[_currentStreak];
            Debug.Log(value);
            _currentScore += Mathf.RoundToInt(_baseScore * value);
            UpdateScoreValues(value);
        }
        else
        {
            float value = _multiplierMap[_multiplierMap.Count - 1];
            _currentScore += Mathf.RoundToInt(_baseScore * value);
            UpdateScoreValues(value);
        }
    }

    private void UpdateScoreValues(float value)
    {
        _currentStreak++;
        _currentMultiplier = value;

        _scoreUIManager.UpdateCombo(_currentMultiplier);
        _scoreUIManager.UpdateScore(_currentScore);
        _scoreUIManager.UpdateStreak(_currentStreak);
    }

    public void BreakCombo()
    {
        _currentMultiplier = 0;
        _currentStreak = 0;

        _scoreUIManager.UpdateCombo(_currentMultiplier);
        _scoreUIManager.UpdateStreak(_currentStreak);
    }
}