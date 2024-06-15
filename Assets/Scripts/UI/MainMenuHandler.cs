using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [Header ("Settings")]
    [SerializeField] private string gameplaySceneToLoad;

    [Header("References")]
    [SerializeField] private GameObject menuHandle;
    [SerializeField] private GameObject levelSelectHandle;
    [SerializeField] private GameObject settingsMenuHandle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameplaySceneToLoad);
    }

    public void OpenLevelSelector()
    {
        levelSelectHandle.SetActive(true);
        menuHandle.SetActive(false);
    }

    public void CloseLevelSelector() 
    {
        levelSelectHandle?.SetActive(false);
        menuHandle?.SetActive(true);
    }

    public void OpenSettingsMenu()
    {
        settingsMenuHandle.SetActive(true);
        menuHandle.SetActive(false);
    }

    public void CloseSettingsMenu()
    {
        settingsMenuHandle.SetActive(false);
        menuHandle.SetActive(true);
    }

    public void QuitGame()
    {

#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

#endif  
    
        Application.Quit();
    }
}
