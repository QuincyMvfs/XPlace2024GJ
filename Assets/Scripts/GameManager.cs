using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    [Header("Profile Settings")]
    public string profileName;
    public int starCount;
    public Color profileColor;

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
            Debug.Log("File Exists: " + path);
        }
    }
}
