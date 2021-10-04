using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Private Instance Variables
    private GameController _gameController;
    private bool _groundedPlayer;
    private Vector3 _playerVelocity;
    private float _playerSpeed= 10f;
    private Rigidbody _rigidbody;

    //Public Variables
    public float PlayerSpeed
    {
        get => _playerSpeed;
        set
        {
            _playerSpeed = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }
    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Initialize()
    {
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     void FixedUpdate()
    {
        if (!_gameController.IsGamePause)
        {
            if(_groundedPlayer && _playerVelocity.y < 0)
            {
                _playerVelocity.y = 0f;
            }

            //Movement
      
             Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _rigidbody.MovePosition(transform.position + move * Time.deltaTime * _playerSpeed);


        }
    }
}
