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
    [SerializeField] private Dictionary<int, float> _multiplierMap = new Dictionary<int, float>();
    [SerializeField] private ScoreUIManager _scoreUIManager;

    private int _currentScore = 0;
    private int _currentStreak = 0;
    private float _currentMultiplier = 0;

    public int CurrentScore => _currentScore;
    public int CurrentStreak => _currentStreak;
    public float CurrentMultiplier => _currentMultiplier;

    private void Awake()
    {
        _multiplierMap.Add(0, 1.0f);
        _multiplierMap.Add(1, 1.2f);
        _multiplierMap.Add(2, 1.5f);
        _multiplierMap.Add(3, 2f);
        _multiplierMap.Add(4, 3f);
        _multiplierMap.Add(5, 5f);
        _multiplierMap.Add(6, 8f);
        _multiplierMap.Add(7, 16f);
        _multiplierMap.Add(8, 25f);
        _multiplierMap.Add(9, 30f);
        _multiplierMap.Add(10, 50f);
    }

    public void AddScore()
    {
        if (_multiplierMap.ContainsKey(_currentStreak))
        {
            float value = _multiplierMap[_currentStreak];
            _currentScore += Mathf.RoundToInt(_baseScore * value);
            UpdateScoreValues(value);
        }
        else
        {
            float value = _multiplierMap.Values.Last();
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
