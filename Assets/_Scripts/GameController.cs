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
    private float _spawnDelay = 1.0f;

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

    // Start is called before the first frame update
    void Start()
    {
       Initialize();
       BringDownMenu();
       this.IsGamePause = false;
       this.IsGameOver = false;
        PlayerAbility = _gameManager.AbilityChoice;
        _createEnemies(1, 0, 0);
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
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
        PlayerRespawnpoint = GameObject.FindGameObjectWithTag("Respawn").GetComponent<Transform>();
        SpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        Targets = GameObject.FindGameObjectsWithTag("Target");
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

    //IEnumerators
    /// <summary>
    /// Method to spawn enemies on map at spawn points
    /// </summary>
    /// <param name="_amountN">Amount of Normal type enemies to spawn</param>
    /// <param name="_amountF">Amount of Fast type enemies to spawn</param>
    /// <param name="_amountB">Amount of Big type enemies to spawn</param>
    private IEnumerator _createEnemies(int _amountN, int _amountF, int _amountB)
    {
        //Wait 1 second before spawning another enemy
        WaitForSeconds Wait = new WaitForSeconds(_spawnDelay);


        GameObject newgameobject = Instantiate(EnemyPrefab, SpawnPoints[Random.Range(0, SpawnPoints.Length)].GetComponent<Transform>());
        //newgameobject.GetComponent<EnemyController>().agent.enabled = true;
        newgameobject.GetComponent<EnemyController>().EnemyType = "Normal";
        newgameobject.GetComponent<EnemyController>().Speed = 2.0f;
        newgameobject.GetComponent<EnemyController>().HealthPoints = 20;
        newgameobject.GetComponent<EnemyController>().Target = Targets[0];
        Debug.Log("Enemy Created");

        yield return Wait;
    }
}
