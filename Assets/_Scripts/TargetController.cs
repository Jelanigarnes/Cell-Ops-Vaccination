using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetController : MonoBehaviour
{
    private float _health;
    private GameController _gameController;
    private bool _calledForhelp;
    private float _maxHealth;
    public GameObject HealthBar;
    public Slider HealthbarSlider;

    public float MaxHealth { 
        get => _maxHealth;
        set {
            _maxHealth = value;
            Health = _maxHealth;
            HealthbarSlider.maxValue = _maxHealth;
        } 
    }

    public float Health { 
        get => _health;
        set {
             _health = value;
            //make sure health can't go over max health
            if (_health > MaxHealth)
            {
                _health = MaxHealth;
            }
            HealthbarSlider.value = _health;
        } 
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameController.IsGamePause)
        {
            if (Health <= 0)
            {
                Object.Destroy(this.gameObject);
            }
            if (Health < MaxHealth && !_calledForhelp)
            {
                _isBeingAttacked();
                HealthBar.SetActive(true);
            }
        }
    }
    /// <summary>
    /// Reduce health by amount.
    /// </summary>
    /// <param name="amount">Damage Amount in float</param>
    public void TakeDamage(float amount)
    {
        Health -= amount;
    }
    /////PRIVAT METHODS
    /// <summary>
    /// Calls for help 
    /// </summary>
    private void _isBeingAttacked()
    {
        _gameController.Help(this.gameObject);
        _calledForhelp = true;
    }
    /// <summary>
    /// when object is destroyed it will notify the gamecontroller.
    /// </summary>
    private void OnDestroy()
    {
        _gameController.TargetDied(this.gameObject);
    }
}
