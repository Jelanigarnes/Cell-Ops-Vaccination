using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private GameObject _gameManagerObject;
    private GameManager _gameManager;

    // PUBLIC INSTANCE VARIABLES
    public AudioSource MainMenuSound;

    [Header("Text")]
    public Text VersionLabel;

    [Header("Buttons")]
    public Button StartButton;
    public Button ExitButton;

    /// <summary>
    /// Use this for initialization
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        VersionLabel.text = "Version: " + Application.version;
        Cursor.visible = true;
        this._gameManagerObject = GameObject.Find("GameManager");
        _gameManager = _gameManagerObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
    /// Calls Close Game method in GameManager
    /// </summary>
    public void Close_Game()
    {
        _gameManager.Close_Game();
    }

}
