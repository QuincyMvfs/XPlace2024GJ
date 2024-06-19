using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [Header ("Settings")]
    [SerializeField] private string gameplaySceneToLoad;

    [Header("References")]
    [SerializeField] private GameObject _menuHandle;
    [SerializeField] private GameObject _levelSelectHandle;
    [SerializeField] private GameObject _settingsMenuHandle;

    [SerializeField] private TMP_InputField _profileNameInputField;
    [SerializeField] private GameObject[] _profileStars;
    [SerializeField] private GameObject _vipText;

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.Instance.isFromLevel == true)
        {
            OpenLevelSelector();
        }

        _profileNameInputField.onEndEdit.AddListener((string profileName) =>
        {
            //call this method when the input field OnEndEdit is called
            NewProfileNameAdded(profileName);
        });

        _profileNameInputField.text = GameManager.Instance.profileName;

        //light up stars on profile card
        for(int i = 0; i < GameManager.Instance.profileStarCount; i++)
        {
            _profileStars[i].SetActive(true);

            if(i == _profileStars.Length - 1)
            {
                _vipText.SetActive(true);
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameplaySceneToLoad);
    }

    public void OpenLevelSelector()
    {
        GameManager.Instance.isFromLevel = false;
        _levelSelectHandle.SetActive(true);
        _menuHandle.SetActive(false);
    }

    public void CloseLevelSelector() 
    {
        _levelSelectHandle?.SetActive(false);
        _menuHandle?.SetActive(true);
    }

    public void OpenSettingsMenu()
    {
        _settingsMenuHandle.SetActive(true);
        _menuHandle.SetActive(false);
    }

    public void CloseSettingsMenu()
    {
        _settingsMenuHandle.SetActive(false);
        _menuHandle.SetActive(true);
    }

    public void QuitGame()
    {
        GameManager.Instance.SaveProfile();

#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

#endif  
    
        Application.Quit();
    }

    //save data
    public void NewProfileNameAdded(string profileName)
    {
        GameManager.Instance.profileName = profileName;
    }
}
