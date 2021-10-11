using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Private Instance Variables
    private float _time=0.0f;
    private bool _isGameOver;
    private bool _isGamePause;
    private GameManager _gameManager;
    private GameObject _player;
    //[SerializeField]
    private string _playerAbility;
    private Transform _playerRespawnpoint;
    private float _spawnDelay = 2.0f;
    private int _numberOfEnemiesToSpawn;
    private Transform _playerRespawnLocation;

    // PUBLIC INSTANCE VARIABLES

    public GameObject Camera;

    [Header("Menu")]
    public Text TimeLable;
    public Text MenuTitle;
    public Button BackToMainMenuButton;
    public Button ResumeButton;

    [Header("Player Objects")]
    public GameObject PlayerPrefab;
    public GameObject HelpArrow;

    [Tooltip("The amount of enemies in the game.")]
    [Header("Enemies")]
    public GameObject EnemyPrefab;
    public List<GameObject> Enemies;

    [Tooltip("The amount of Targets in the game.")]
    [Header("Targets")]
    public List<GameObject> Targets;

    [Tooltip("The amount of enemy spawnpoints in the game.")]
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
                SceneManager.LoadScene("Menu");
                //Invoke("BackToMainScreen", 5);
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
    public Transform PlayerRespawnLocation { get => _playerRespawnLocation; set => _playerRespawnLocation = value; }
    public GameObject Player { get => _player; set => _player = value; }

    // Start is called before the first frame update
    void Start()
    {        
        Initialize();
        BringDownMenu();
        this.IsGamePause = true;
        this.IsGameOver = false;
        PlayerAbility = _gameManager.AbilityChoice;
        Cursor.visible = true;
        TimeLable.text = "00:00:00";
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsGamePause)
        {
            _updateTimer();
        }        
        if (Input.GetKey(KeyCode.Escape))
        {
            IsGamePause = true;
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
        Targets.AddRange(GameObject.FindGameObjectsWithTag("Target"));
        PlayerRespawnLocation = GameObject.FindGameObjectWithTag("Respawn").transform;
    }
    // Public METHODS*******************************
    /// <summary>
    /// Carrys user back to main menu scene
    /// </summary>
    public void BackToMainScreen()
    {
        SceneManager.LoadScene("Menu");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    /// <summary>
    /// Resumes game
    /// </summary>
    public void Resume()
    {
        IsGamePause = false;
        BringDownMenu();
    }
    /// <summary>
    /// Brings down menu UI
    /// </summary>
    public void BringDownMenu()
    {
        MenuTitle.gameObject.SetActive(false);
        BackToMainMenuButton.gameObject.SetActive(false);
        ResumeButton.gameObject.SetActive(false);
    }
    /// <summary>
    /// Is called by Targets when they are being attacked
    /// </summary>
    public void Help(GameObject target)
    {
        GameObject helpArrow = Instantiate(HelpArrow, _player.transform);
        helpArrow.GetComponent<TargetPointer>().HelpTarget = target;
        //helpArrow.transform.SetParent(Player.transform);
    }
    /// <summary>
    /// Refresh Target list and tells all enemies to find new Target
    /// </summary>
    /// <param name="target">Target that died</param>
    public void TargetDied(GameObject target)
    {
        Targets.Remove(target);
        if (Targets.Count == 0)
        {
            IsGameOver = true;
        }
        foreach(GameObject enemy in Enemies)
        {
            enemy.GetComponent<EnemyController>().NewTarget(Targets);
        }
    }
    public void EnemyDied(GameObject enemy)
    {
        Enemies.Remove(enemy);
    }
    /// <summary>
    /// Gets and display the time
    /// </summary>
    private void _updateTimer()
    {
        _time += Time.deltaTime;
        float minutes = Mathf.Floor(_time / 60);
        float seconds = Mathf.RoundToInt(_time % 60);
        float splitseconds = Mathf.RoundToInt(_time * 1000);
        splitseconds = (splitseconds % 1000);

        TimeLable.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, splitseconds); 
    }
    /// <summary>
    /// Start the game
    /// </summary>
    public void StartGame()
    {
        Object.Destroy(Camera);
        Player = Instantiate(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        //Sets level details based on current level. TO add later
        StartCoroutine(_createEnemies(5, 3, 1));
        foreach(GameObject target in Targets)
        {
            target.GetComponent<TargetController>().MaxHealth = 100000;
        }

        ////
      
        //Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject.Find("BtnStart").SetActive(false);
        _isGamePause = false;
    }
    // Private METHODS*******************************
    private void _bringUpMenu()
    {
        MenuTitle.gameObject.SetActive(true);
        BackToMainMenuButton.gameObject.SetActive(true);
        ResumeButton.gameObject.SetActive(true);
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
        while (_totalEnemies > 0 || (_amountN == 0 && _amountF == 0 && _amountB == 0))
        {
            while (IsGamePause)
            {
                yield return null;
            }
            if (_amountN > 0)
            {
                GameObject NormalEnemy = Instantiate(EnemyPrefab, SpawnPoints[Random.Range(0, SpawnPoints.Length)].GetComponent<Transform>().position, Quaternion.identity);
                NormalEnemy.GetComponent<EnemyController>().EnemyType = "Normal";
                NormalEnemy.GetComponent<EnemyController>().Speed = 2.0f;
                NormalEnemy.GetComponent<EnemyController>().HealthPoints = 20;
                NormalEnemy.GetComponent<EnemyController>().EnemyDmg = 1;
                NormalEnemy.GetComponent<EnemyController>().Target = Targets[Random.Range(0, Targets.Count)];
                Enemies.Add(NormalEnemy);

                _amountN = _amountN - 1;
                _totalEnemies = _totalEnemies - 1;
            }
            else if (_amountF > 0)
            {
                    GameObject FastEnemy = Instantiate(EnemyPrefab, SpawnPoints[Random.Range(0, SpawnPoints.Length)].GetComponent<Transform>().position, Quaternion.identity);
                    FastEnemy.GetComponent<EnemyController>().EnemyType = "Fast";
                    FastEnemy.GetComponent<EnemyController>().Speed = 4.0f;
                    FastEnemy.GetComponent<EnemyController>().HealthPoints = 10;
                    FastEnemy.GetComponent<EnemyController>().EnemyDmg = 1;
                    FastEnemy.GetComponent<EnemyController>().Target = Targets[Random.Range(0, Targets.Count)];
                Enemies.Add(FastEnemy);

                _amountF = _amountF - 1;
                    _totalEnemies = _totalEnemies - 1;
            }
            else if (_amountB > 0)
            {
                    GameObject BigEnemy = Instantiate(EnemyPrefab, SpawnPoints[Random.Range(0, SpawnPoints.Length)].GetComponent<Transform>().position, Quaternion.identity);
                    BigEnemy.GetComponent<EnemyController>().EnemyType = "Big";
                    BigEnemy.GetComponent<EnemyController>().Speed = 1.0f;
                    BigEnemy.GetComponent<EnemyController>().HealthPoints = 40;
                    BigEnemy.GetComponent<EnemyController>().EnemyDmg = 1;
                    BigEnemy.GetComponent<EnemyController>().Target = Targets[Random.Range(0, Targets.Count)];
                Enemies.Add(BigEnemy);

                _amountB = _amountB - 1;
                    _totalEnemies = _totalEnemies - 1;
            }
            //Wait 1 second before spawning another enemy
            WaitForSeconds Wait = new WaitForSeconds(_spawnDelay);
            yield return Wait;
        }
    }
}
