using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EndOfLevel : MonoBehaviour
{
    public UnityEvent LevelFinished;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<PlayerMesh>(out PlayerMesh playerMesh))
        {
            if (playerMesh.PlayerGameObject.TryGetComponent<ScoreController>(out ScoreController scoreController))
            {
                scoreController.EnableEndScreen();
                LevelFinished.Invoke();

                FinalScoreUIHandler finalScoreUIHandler = FindObjectOfType<FinalScoreUIHandler>();
                finalScoreUIHandler.gameObject.SetActive(true);
            }
        }

        GameManager.Instance.isFromLevel = true;

        if(SceneManager.GetActiveScene().name == "Level_1_America")
        {
            GameManager.Instance.hasCompletedLevelOne = true;
        }
        else if(SceneManager.GetActiveScene().name == "Level_2_Asia")
        {
            GameManager.Instance.hasCompletedLevelTwo = true;
        }
        else if(SceneManager.GetActiveScene().name == "Level_3_MiddleEast")
        {
            GameManager.Instance.hasCompletedLevelThree = true;
        }

    }
}
