using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private string[] _levelNames;

    [SerializeField] private GameObject LevelOneButton;
    [SerializeField] private GameObject LevelTwoButton;
    [SerializeField] private GameObject LevelThreeButton;
    [SerializeField] private GameObject LevelFourButton;

    [SerializeField] private GameObject[] _levelOneStars;
    [SerializeField] private GameObject[] _levelTwoStars;
    [SerializeField] private GameObject[] _levelThreeStars;
    [SerializeField] private GameObject[] _levelFourStars;


    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.hasCompletedLevelOne == true)
        {
            LevelTwoButton.GetComponent<Button>().interactable = true;
            StarUpdating(GameManager.Instance.gainedStarLevelOne, _levelOneStars);
        }
        else
        {
            LevelTwoButton.GetComponent<Button>().interactable = false;
        }

        if (GameManager.Instance.hasCompletedLevelTwo == true)
        {
            LevelThreeButton.GetComponent<Button>().interactable = true;
            StarUpdating(GameManager.Instance.gainedStarLevelTwo, _levelTwoStars);
        }
        else
        {
            LevelThreeButton.GetComponent<Button>().interactable = false;
        }

        if (GameManager.Instance.hasCompletedLevelThree == true)
        {
            LevelFourButton.GetComponent<Button>().interactable = true;
            StarUpdating(GameManager.Instance.gainedStarLevelThree, _levelThreeStars);
        }
        else
        {
            LevelFourButton.GetComponent<Button>().interactable = false;
        }

        if (GameManager.Instance.hasCompletedLevelFour == true)
        {
            StarUpdating(GameManager.Instance.gainedStarLevelFour, _levelFourStars);
        }

        //temp -- we need to read the player profile and set the stars accordingly
        foreach (var star in _levelFourStars)
        { star.SetActive(false); }

        foreach (var star in _levelTwoStars)
        { star.SetActive(false); }

        foreach (var star in _levelThreeStars)
        { star.SetActive(false); }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StarUpdating(int starFromLevel, GameObject[] level)
    {
        if(starFromLevel == 1)
        {
            level[starFromLevel-1].SetActive(true);
        }
        else if(starFromLevel == 2)
        {
            level[starFromLevel-2].SetActive(true);
            level[starFromLevel-1].SetActive(true);
        }
        else if (starFromLevel == 3)
        {
            level[starFromLevel-3].SetActive(true);
            level[starFromLevel-2].SetActive(true);
            level[starFromLevel-1].SetActive(true);
        }
    }

    public void StartLevelOne()
    {
        SceneManager.LoadScene(_levelNames[0]);
    }

    public void StartLevelTwo()
    {
        SceneManager.LoadScene(_levelNames[1]);
    }
    public void StartLevelThree()
    {
        SceneManager.LoadScene(_levelNames[2]);
    }
    public void StartLevelFour()
    {
        SceneManager.LoadScene(_levelNames[3]);
    }

}
