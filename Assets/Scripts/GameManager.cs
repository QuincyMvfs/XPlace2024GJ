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
    public int profileStarCount;
    public Color profileColor;

    [Header("Progression Info")]
    public bool hasCompletedLevelOne;
    public bool hasCompletedLevelTwo;
    public bool hasCompletedLevelThree;
    public bool hasCompletedLevelFour;
    public int gainedStarLevelOne;
    public int gainedStarLevelTwo;
    public int gainedStarLevelThree;
    public int gainedStarLevelFour;
    public bool isFromLevel = false;
    public bool[] profileStarAchieved = new bool [5];

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

        Time.timeScale = 0;
    }

    public void UnPauseGame()
    {
        if(playerController != null)

        //{ playerController.UnpauseController(); }

        playerController.IsPaused = false;
        pauseMenu.SetActive(false);

        Time.timeScale = 1;
    }

    [System.Serializable]
    class SaveData
    {
        [Header ("Profile")]
        public string profileName;
        public Color profileColor;
        public int profileStarCount;

        [Header("Progression Info")]
        public bool hasCompletedLevelOne;
        public bool hasCompletedLevelTwo;
        public bool hasCompletedLevelThree;
        public bool hasCompletedLevelFour;
        public int gainedStarLevelOne;
        public int gainedStarLevelTwo;
        public int gainedStarLevelThree;
        public int gainedStarLevelFour;
    }

    public void SaveProfile()
    {
        SaveData data = new SaveData();
        data.profileName = profileName;
        data.profileColor = profileColor;
        data.profileStarCount = profileStarCount;
        data.hasCompletedLevelOne = hasCompletedLevelOne;
        data.hasCompletedLevelTwo = hasCompletedLevelTwo;
        data.hasCompletedLevelThree = hasCompletedLevelThree;
        data.hasCompletedLevelFour = hasCompletedLevelFour;
        data.gainedStarLevelOne = gainedStarLevelOne;
        data.gainedStarLevelTwo = gainedStarLevelTwo;
        data.gainedStarLevelThree = gainedStarLevelThree;
        data.gainedStarLevelFour = gainedStarLevelFour;

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
            profileStarCount = data.profileStarCount;
            hasCompletedLevelOne = data.hasCompletedLevelOne;
            hasCompletedLevelTwo = data.hasCompletedLevelTwo;
            hasCompletedLevelThree = data.hasCompletedLevelThree;
            hasCompletedLevelFour = data.hasCompletedLevelFour;
            gainedStarLevelOne = data.gainedStarLevelOne;
            gainedStarLevelTwo = data.gainedStarLevelTwo;
            gainedStarLevelThree = data.gainedStarLevelThree;
            gainedStarLevelFour = data.gainedStarLevelFour;
        }
    }

    public void ResetProfile()
    {
        profileName = null;
        profileStarCount = 0;
        hasCompletedLevelOne = false;
        hasCompletedLevelTwo = false;
        hasCompletedLevelThree = false;
        hasCompletedLevelFour = false;
        gainedStarLevelOne = 0;
        gainedStarLevelTwo = 0;
        gainedStarLevelThree = 0;
        gainedStarLevelFour = 0;
        isFromLevel = false;
        profileStarAchieved = new bool[4];

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
