using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    // Private Instance Variables
    private GameController _gameController;
    private Vector3 _playerVelocity;
    private float _enemySpeed = 1f;
    private Rigidbody _rigidbody;
    private string _enemyType;
    private int _healthPoints;
    private int _enemyDmg;
    private RigidbodyConstraints _rigidbodyConstraints;

    //Public Variables
    public GameObject Target;
    public NavMeshAgent Agent;

    public float Speed
    {
        get => _enemySpeed;
        set
        {
            _enemySpeed = value;
        }
    }

    public int HealthPoints { get => _healthPoints; set => _healthPoints = value; }
    public string EnemyType { get => _enemyType; set => _enemyType = value; }
    public int EnemyDmg { get => _enemyDmg; set => _enemyDmg = value; }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        Agent.speed = Speed;
        Agent.SetDestination(Target.transform.position);
        _rigidbodyConstraints = _rigidbody.constraints;
    }
    void Initialize()
    {
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
        Agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if (_gameController.IsGamePause)
        {
            Agent.isStopped=true;
            _rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
            _rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
        }
        else
        {
            //remove all constraits and reset
            _rigidbody.constraints = _rigidbodyConstraints;

            //resume persute
            Agent.isStopped=false;            
        }
    }
}
