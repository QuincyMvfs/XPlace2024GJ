using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    [Header("Profile Settings")]
    public string profileName;
    public int starCount;
    public Color profileColor;

    [Header("Progression Info")]
    public bool hasCompletedLevelOne;
    public bool hasCompletedLevelTwo;
    public bool hasCompletedLevelThree;
    public int gainedStarLevelOne;
    public int gainedStarLevelTwo;
    public int gainedStarThree;
    public int gainedStarFour;
    public bool isFromLevel = false;

    [Header("References")]
    [SerializeField] private GameObject pauseMenu;

    private PlayerController playerController;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadProfile();
    }

    public void PauseGame(PlayerController controller)
    {
        playerController = controller;

        if(SceneManager.GetActiveScene().name != "MainMenu")
        {
            pauseMenu.SetActive(true);
        }
    }

    public void UnPauseGame()
    {
        if(playerController != null)

        { playerController.UnpauseController(); }

        pauseMenu.SetActive(false);
    }

    [System.Serializable]
    class SaveData
    {
        public string profileName;
        public Color profileColor;
        public int starCount;
    }

    public void SaveProfile()
    {
        SaveData data = new SaveData();
        data.profileName = profileName;
        data.profileColor = profileColor;
        data.starCount = starCount;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadProfile()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            profileName = data.profileName;
            profileColor = data.profileColor;
            starCount = data.starCount;
        }
    }
}
