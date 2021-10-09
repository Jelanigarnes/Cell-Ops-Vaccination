using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public int _health;
    private GameController _gameController;
    private bool _calledForhelp;
    private int _maxHealth;

    public int MaxHealth { 
        get => _maxHealth;
        set {
            _maxHealth = value;
            if (_health == 0)
            {
                _health = _maxHealth;
            }
        } 
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        _health = MaxHealth;
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
            if (_health <= 0)
            {
                Object.Destroy(this.gameObject);
            }
            if (_health < MaxHealth && !_calledForhelp)
            {
                _isBeingAttacked();
            }
        }
    }
    /// <summary>
    /// Reduce health by amount.
    /// </summary>
    /// <param name="amount"></param>
    public void TakeDamage(int amount)
    {
        _health -= amount;
        Debug.Log("Taking dmg");
    }
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
