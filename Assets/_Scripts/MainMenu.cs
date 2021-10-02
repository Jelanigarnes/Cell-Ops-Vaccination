using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private GameManager _gameManager;
    private Text _version;

    // PUBLIC INSTANCE VARIABLES
    public AudioSource MainMenuSound;
    public GameObject Settings;

    [Header("Buttons")]
    public Button StartButton;
    public Button ExitButton;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        Initialize();
        _version.text = "Version: " + Application.version;
        Cursor.visible = true;
        MainMenuSound.volume = _gameManager.GameSettings.MusicVolume;
    }
    /// <summary>
    /// Initialize
    /// mainly used to find game objects
    /// </summary>
    void Initialize()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _version = GameObject.Find("TxtVersion").GetComponent<Text>();
        Settings = GameObject.Find("Settings");
    }
    // Update is called once per frame
    void Update()
    {
        Settings.transform.Rotate(0, 0, Time.deltaTime * 15);
    }
    // PUBLIC METHODS
    /// <summary>
    /// Start Game Scene
    /// </summary>
    public void Start_Game()
    {
        if (_gameManager.AbilitySelected)
        {
            MainMenuSound.Stop();
            SceneManager.LoadScene("Game");
        }
        else
        {
            CharacterSelect();
        }
    }
    /// <summary>
    /// Loads Settings Scene
    /// </summary>
    public void ClickSettings()
    {
        SceneManager.LoadScene("Settings");
    }
    /// <summary>
    /// Leads Credits Scene
    /// </summary>
    public void ClickCredits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void CharacterSelect()
    {
        SceneManager.LoadScene("Vaccine Select");
    }
    /// <summary>
    /// Calls Close Game method in GameManager
    /// </summary>
    public void Close_Game()
    {
        _gameManager.Close_Game();
    }

}
