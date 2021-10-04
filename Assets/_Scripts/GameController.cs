using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Private Instance Variables
    private float _time;
    private bool _isGameOver;
    private bool _isGamePause;
    private GameManager _gameManager;
    private string _playerAbility;

    // PUBLIC INSTANCE VARIABLES

    [Header("Menu")]
    public Text TimeLable;
    public Text MenuTitle;
    public Button BackToMainMenu;
    public Button Resume;

    [Header("Enemies")]
    public List<GameObject> Enemies;

    [Header("Targets")]
    public List<GameObject> Targets;
    public bool IsGameOver
    {
        get
        {
            return this._isGameOver;
        }
        set
        {
            this._isGameOver = value;
            if (_isGameOver)
            {
                IsGamePause = true;
                //GameOverLable.gameObject.SetActive(true);
                //GamePlaySound.Stop();                
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                PlayerPrefs.Save();
                Invoke("BackToMainScreen", 5);
            }
        }
    }
    public bool IsGamePause
    {
        get
        {
            return this._isGamePause;
        }
        set
        {
            this._isGamePause = value;
            if (_isGamePause)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.visible = false;
            }
        }
    }
    public string PlayerAbility
    {
        get
        {
            return this._playerAbility;
        }
        set
        {
            this._playerAbility = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       Initialize();
       BringDownMenu();
       this.IsGamePause = false;
       this.IsGameOver = false;
        PlayerAbility = _gameManager.AbilityChoice;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsGamePause)
        {
            
        }
        if (Input.GetKey(KeyCode.Escape) && !IsGameOver)
        {
            _bringUpMenu();          
        }
    }

    // Use this for initialization
    void Initialize()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    // Private METHODS*******************************
    private void _bringUpMenu()
    {
        IsGamePause = true;
        MenuTitle.gameObject.SetActive(true);
        BackToMainMenu.gameObject.SetActive(true);
        Resume.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    // Public METHODS*******************************


    public void BackToMainScreen()
    {
        SceneManager.LoadScene("Menu");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void BringDownMenu()
    {
        IsGamePause = false;
        MenuTitle.gameObject.SetActive(false);
        BackToMainMenu.gameObject.SetActive(false);
        Resume.gameObject.SetActive(false);
    }
}
