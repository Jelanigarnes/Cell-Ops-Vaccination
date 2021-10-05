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
    //[SerializeField]
    private string _playerAbility;
    private Transform _playerRespawnpoint;
    private float _spawnDelay = 2.0f;
    private int _numberOfEnemiesToSpawn;

    // PUBLIC INSTANCE VARIABLES

    [Header("Menu")]
    public Text TimeLable;
    public Text MenuTitle;
    public Button BackToMainMenu;
    public Button Resume;


    [Tooltip("The amount of enemies in the game.")]
    [Header("Enemies")]
    public GameObject EnemyPrefab;
    public GameObject[] Enemies;

    [Header("Targets")]
    public GameObject[] Targets;

    [Header("Enemy Spawnpoints")]
    public GameObject[] SpawnPoints;
    
    public Transform PlayerRespawnpoint { get => _playerRespawnpoint; set => _playerRespawnpoint = value; }
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

    public int NumberOfEnemiesToSpawn { get => _numberOfEnemiesToSpawn; set => _numberOfEnemiesToSpawn = value; }

    // Start is called before the first frame update
    void Start()
    {        
        Initialize();
        BringDownMenu();
        this.IsGamePause = false;
        this.IsGameOver = false;
        PlayerAbility = _gameManager.AbilityChoice;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsGamePause)
        {
            
        }
        if (Input.GetKey(KeyCode.Escape) && !IsGameOver)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _bringUpMenu();          
        }
    }

    // Use this for initialization
    void Initialize()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        PlayerRespawnpoint = GameObject.FindGameObjectWithTag("Respawn").GetComponent<Transform>();
        SpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        Targets = GameObject.FindGameObjectsWithTag("Target");
    }
    /// <summary>
    /// Start the game
    /// </summary>
    public void StartGame()
    {
        Cursor.visible = false;
        StartCoroutine(_createEnemies(10, 0, 0));
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject.Find("BtnStart").SetActive(false);
    }
    // Private METHODS*******************************
    private void _bringUpMenu()
    {
        IsGamePause = true;
        MenuTitle.gameObject.SetActive(true);
        BackToMainMenu.gameObject.SetActive(true);
        Resume.gameObject.SetActive(true);
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

    //IEnumerators
    /// <summary>
    /// Method to spawn enemies on map at spawn points
    /// </summary>
    /// <param name="_amountN">Amount of Normal type enemies to spawn</param>
    /// <param name="_amountF">Amount of Fast type enemies to spawn</param>
    /// <param name="_amountB">Amount of Big type enemies to spawn</param>
    /// <param name="_totalEnemies">Amount of Enemies in total</param>
    private IEnumerator _createEnemies(int _amountN, int _amountF, int _amountB,int _totalEnemies=40)
    {
        Debug.Log("Spawning enemy");
        while (_totalEnemies > 0)
        {
            if (_amountN > 0)
            {
                Debug.Log("Spawning Normal Enemy.");
                GameObject newgameobject = Instantiate(EnemyPrefab, SpawnPoints[Random.Range(0, SpawnPoints.Length)].GetComponent<Transform>().position, Quaternion.identity);
                newgameobject.GetComponent<EnemyController>().EnemyType = "Normal";
                newgameobject.GetComponent<EnemyController>().Speed = 2.0f;
                newgameobject.GetComponent<EnemyController>().HealthPoints = 20;
                newgameobject.GetComponent<EnemyController>().Target = Targets[Random.Range(0, Targets.Length)];

                _amountN = _amountN - 1;
                _totalEnemies = _totalEnemies - 1;
            }
            else if (_amountF > 0)
            {
                _amountF = _amountF - 1;
                _totalEnemies = _totalEnemies - 1;
            }
            else if (_amountB > 0)
            {
                _amountB = _amountB - 1;
                _totalEnemies = _totalEnemies - 1;
            }
            //Wait 1 second before spawning another enemy
            WaitForSeconds Wait = new WaitForSeconds(_spawnDelay);

            yield return Wait;
        }
    }
}
