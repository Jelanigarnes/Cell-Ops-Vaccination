using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private string _abilityChoice;
    private bool _abilitySelected;
    private float _score;

    public static GameManager instance = null;

    public GameSettings GameSettings;
    public string AbilityChoice
    {
        get
        {
            return _abilityChoice;
        }
        set
        {
            _abilityChoice = value;
        }
    }
    public bool AbilitySelected
    {
        get
        {
            return _abilitySelected;
        }
        set
        {
            _abilitySelected = value;
        }
    }

    public float Score 
    { 
        get => _score; 
        set => _score = value; 
    }

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Checks if game settings file exist
        if (System.IO.File.Exists(Application.persistentDataPath + "/gamesettings.json"))
        {
            //Loads game Settings
            GameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));
        }
        else
        {
            //Create File
            Debug.Log("Creating file");
            GameSettings = new GameSettings();
            GameSettings.MusicVolume = 50f;
            string jsonData = JsonUtility.ToJson(GameSettings, true);
            File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// For When player wins game
    /// </summary>
    public void WinGame()
    {

    }
    /// <summary>
    /// Closes Game
    /// </summary>
    public void Close_Game()
    {
        Application.Quit();
    }
    /// <summary>
    /// Grabs game settings file from location
    /// </summary>
    public void ChangedSettings()
    {
        GameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));
    }
    /// <summary>
    /// Loads player settings
    /// </summary>
    void LoadSettings()
    {
        QualitySettings.antiAliasing = GameSettings.Antialiasing;
        QualitySettings.vSyncCount = GameSettings.VSync;
        QualitySettings.masterTextureLimit = GameSettings.TextureQuality;
        Screen.fullScreen = GameSettings.Fullscreen;
    }

}
