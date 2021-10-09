using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    private int _health;
    private GameController _gameController;
    private bool _calledForhelp;

    public bool Attacked;
    public int Health { get => _health; set => _health = value; }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        Health = _gameController.TargetHealths;
    }

    void Initialize()
    {
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0 && !_gameController.IsGamePause)
        {
            Object.Destroy(this.gameObject);
        }
        if (Attacked && !_calledForhelp)
        {
            _isBeingAttacked();
        }
    }
    private void _isBeingAttacked()
    {
        _gameController.Help(this.gameObject);
        _calledForhelp = true;
    }
    public void TakeDamage(int amount)
    {
        Health -= amount;
    }

}
