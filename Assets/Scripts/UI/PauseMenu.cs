using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Image[] _buttons;
    [SerializeField] private TextMeshProUGUI[] _texts;
    [SerializeField] private Color _americaColor = Color.cyan;
    [SerializeField] private Color _asiaColor = Color.red;
    [SerializeField] private Color _middleEastColor = Color.red;
    [SerializeField] private Color _europeColor = Color.blue;

    private void Awake()
    {
        GetColor();
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        GetColor();
    }

    private void GetColor()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        switch (currentScene)
        {
            case "Level_1_America":
                SetColor(_americaColor);
                break;
            case "Level_2_Asia":
                SetColor(_asiaColor);
                break;
            case "Level_3_MiddleEast":
                SetColor(_middleEastColor);
                break;
            case "Level_4_Europe":
                SetColor(_europeColor);
                break;

        }
    }

    private void SetColor(Color newColor)
    {
        foreach (Image image in _buttons)
        {
            image.color = newColor;
        }

        foreach (TextMeshProUGUI text in _texts)
        {
            text.color = newColor;
        }
    }

    public void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        this.gameObject.SetActive(false);
    }

    public void OpenLevel(string level)
    {
        SceneManager.LoadScene(level);
        this.gameObject.SetActive(false);
    }
}
