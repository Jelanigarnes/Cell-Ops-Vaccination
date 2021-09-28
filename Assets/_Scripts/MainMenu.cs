using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private GameObject _gameManagerObject;
    private GameManager _gameManager;
    private Text _version;

    // PUBLIC INSTANCE VARIABLES
    public AudioSource MainMenuSound;
    public GameObject Settings;

    [Header("Buttons")]
    public Button StartButton;
    public Button ExitButton;

    /// <summary>
    /// Use this for initialization
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        Initialize();
        _version.text = "Version: " + Application.version;
        Cursor.visible = true;
        this._gameManagerObject = GameObject.Find("GameManager");
        _gameManager = _gameManagerObject.GetComponent<GameManager>();
        MainMenuSound.volume = _gameManager.GameSettings.MusicVolume;
    }
    void Initialize()
    {
        this._gameManagerObject = GameObject.Find("GameManager");
        _gameManager = _gameManagerObject.GetComponent<GameManager>();
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
        MainMenuSound.Stop();
        SceneManager.LoadScene("Game");
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
    /// <summary>
    /// Calls Close Game method in GameManager
    /// </summary>
    public void Close_Game()
    {
        _gameManager.Close_Game();
    }

}
