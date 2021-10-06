using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    private int _health;
    private GameController _gameController;

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
        if (Health <= 0)
        {
            Object.Destroy(this.gameObject);
        }
    }
    public void TakeDamage(int amount)
    {
        Health -= amount;
    }
}
