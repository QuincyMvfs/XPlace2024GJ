using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalScoreUI : MonoBehaviour
{
    [SerializeField] private ScoreController _scoreController;
    [SerializeField] private GameObject _star1;
    [SerializeField] private GameObject _star2;
    [SerializeField] private GameObject _star3;
    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private float _scoreIncrementDelay = 0.01f;
    [SerializeField] private StoreFinalScores _storeFinalScores;


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
            _speedText.text = displayedScore.ToString();

            if (displayedScore >= 0)
            {
                _star1.SetActive(true);
            }
            if (displayedScore >= 10)
            {
                _star2.SetActive(true);
            }
            if (displayedScore >= 50)
            {
                _star3.SetActive(true);
            }
            yield return new WaitForSeconds(_scoreIncrementDelay);
        }
    }
}
