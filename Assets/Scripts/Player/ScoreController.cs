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
    [SerializeField] public List<float> _multiplierMap = new List<float>();
    [SerializeField] private ScoreUIManager _scoreUIManager;
    [SerializeField] private GameObject _endScreen;

    [HideInInspector] public UnityEvent OnComboBrokenEvent;

    private int _currentScore = 0;
    private int _currentStreak = 0;
    private float _currentMultiplier = 0;

    private float _highestStreak;
    private float _highestMultiplier;

    public int CurrentScore => _currentScore;
    public int CurrentStreak => _currentStreak;
    public float CurrentMultiplier => _currentMultiplier;

    private void Start()
    {
        UpdateScoreValues(0);
        BreakCombo();
    }
    public void AddScore()
    {
        float value = 0;
        if (_currentStreak <= _multiplierMap.Count - 1)
        {
            value = _multiplierMap[_currentStreak];
            _currentScore += Mathf.RoundToInt(_baseScore * value);
            UpdateScoreValues(value);
        }
        else
        {
            value = _multiplierMap[_multiplierMap.Count - 1];
            _currentScore += Mathf.RoundToInt(_baseScore * value);
            UpdateScoreValues(value);
        }
    }

    public void AddToStreak(int value)
    {
        _currentStreak += value;
        AddScore();
    }

    public void AddCoinScore(int score)
    {
        _currentScore += score;
        _scoreUIManager.UpdateScore(_currentScore);
    }

    private void UpdateScoreValues(float value)
    {
        _currentStreak++;
        _currentMultiplier = value;

        if (_currentStreak > _highestStreak) _highestStreak = _currentStreak;
        if (_currentMultiplier > _highestMultiplier) _highestMultiplier = _currentMultiplier;

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

        OnComboBrokenEvent.Invoke();
    }

    public void EnableEndScreen()
    {
        _endScreen.SetActive(true);
    }

    public float GetHighestStreak()
    {
        return _highestStreak;
    }

    public float GetHighestMultiplier()
    {
        return _highestMultiplier;
    }
}
