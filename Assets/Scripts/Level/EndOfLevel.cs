using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EndOfLevel : MonoBehaviour
{
    public UnityEvent LevelFinished;
    private int _levelOneStars;
    private int _levelTwoStars;
    private int _levelThreeStars;
    private int _levelFourStars;
    private float _currentScore;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerMesh>(out PlayerMesh playerMesh))
        {
            if (playerMesh.PlayerGameObject.TryGetComponent<ScoreController>(out ScoreController scoreController))
            {
                scoreController.EnableEndScreen();
                LevelFinished.Invoke();

                FinalScoreUIHandler finalScoreUIHandler = FindObjectOfType<FinalScoreUIHandler>();
                finalScoreUIHandler.gameObject.SetActive(true);
                _currentScore = scoreController.CurrentScore;

                if (playerMesh.PlayerGameObject.TryGetComponent<PlayerInput>(out PlayerInput playerInput))
                {
                    playerInput.enabled = false; // Disable the PlayerInput component
                }
            }
        }

        GameManager.Instance.isFromLevel = true;

        if (SceneManager.GetActiveScene().name == "Level_1_America")
        {
            if (_currentScore < 5000)
            {
                _levelOneStars = 1;
            }
            else if (_currentScore < 10000 && _currentScore >= 5000)
            {
                _levelOneStars = 2;
            }
            else if (_currentScore >= 10000)
            {
                _levelOneStars = 3;
                if (!GameManager.Instance.profileStarAchieved[1])
                { 
                    GameManager.Instance.profileStarCount++;
                    GameManager.Instance.profileStarAchieved[1] = true; 
                } 

            }

            GameManager.Instance.hasCompletedLevelOne = true;
            GameManager.Instance.gainedStarLevelOne = _levelOneStars;
        }

        if (SceneManager.GetActiveScene().name == "Level_2_Asia")
        {
            if (_currentScore < 10000)
            {
                _levelTwoStars = 1;
            }
            else if (_currentScore < 20000 && _currentScore >= 10000)
            {
                _levelTwoStars = 2;
            }
            else if (_currentScore >= 20000)
            {
                _levelTwoStars = 3;
                if (!GameManager.Instance.profileStarAchieved[2])
                {
                    GameManager.Instance.profileStarCount++;
                    GameManager.Instance.profileStarAchieved[2] = true;
                }
            }

            GameManager.Instance.hasCompletedLevelTwo = true;
            GameManager.Instance.gainedStarLevelTwo = _levelTwoStars;
        }

        if (SceneManager.GetActiveScene().name == "Level_3_MiddleEast")
        {
            if (_currentScore < 13500)
            {
                _levelThreeStars = 1;
            }
            else if (_currentScore < 25000 && _currentScore >= 13500)
            {
                _levelThreeStars = 2;
            }
            else if (_currentScore >= 25000)
            {
                _levelThreeStars = 3;
                if (!GameManager.Instance.profileStarAchieved[3])
                {
                    GameManager.Instance.profileStarCount++;
                    GameManager.Instance.profileStarAchieved[3] = true;
                }
            }

            GameManager.Instance.hasCompletedLevelThree = true;
            GameManager.Instance.gainedStarLevelThree = _levelThreeStars;
        }

        if(SceneManager.GetActiveScene().name == "Level_4_Europe")
        {
            if (!GameManager.Instance.profileStarAchieved[0])
            {
                GameManager.Instance.profileStarCount++;
                GameManager.Instance.profileStarAchieved[0] = true;
            }

            if (_currentScore < 15000)
            {
                _levelFourStars = 1;
            }
            else if (_currentScore < 32000 && _currentScore >= 15000)
            {
                _levelFourStars = 2;
            }
            else if (_currentScore >= 32000)
            {
                _levelFourStars = 3;
                if (!GameManager.Instance.profileStarAchieved[4])
                {
                    GameManager.Instance.profileStarCount++;
                    GameManager.Instance.profileStarAchieved[4] = true;
                }
            }

            GameManager.Instance.hasCompletedLevelFour = true;
            GameManager.Instance.gainedStarLevelFour = _levelFourStars;
        }

        }
    }
 


    
