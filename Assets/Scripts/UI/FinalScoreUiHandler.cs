using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalScoreUIHandler: MonoBehaviour
{
    [SerializeField] private ScoreController _scoreController;
    [SerializeField] private GameObject _star1;
    [SerializeField] private GameObject _star2;
    [SerializeField] private GameObject _star3;
    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private float _scoreIncrementDelay = 0.01f;
    [SerializeField] private StoreFinalScores _storeFinalScores;
    [SerializeField] private float _scoreIncreamentAmount;

    private void OnEnable()
    {
        if(_scoreController != null)
        {
            float score = _scoreController.CurrentScore;
            StartCoroutine(DisplayScores(score));
        }
    }

    IEnumerator DisplayScores(float score)
    {
        float displayedScore = 0;
        while (displayedScore < score)
        {
            displayedScore++;
            displayedScore += _scoreIncreamentAmount;
            _speedText.text = displayedScore.ToString();

            if (displayedScore >= _storeFinalScores.star1)
            {
                _star1.SetActive(true);
            }
            if (displayedScore >= _storeFinalScores.star2)
            {
                _star2.SetActive(true);
            }
            if (displayedScore >= _storeFinalScores.star3)
            {
                _star3.SetActive(true);
            }
            yield return new WaitForSeconds(_scoreIncrementDelay);
        }
    }

    public void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void OpenLevel(string level)
    {
        SceneManager.LoadScene(level);
    }
}
