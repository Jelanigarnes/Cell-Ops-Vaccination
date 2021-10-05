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

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Enemy Spawned");
        Initialize();
        Invoke("EnabledNavMeshAgent", 0.025f);
    }
    void Initialize()
    {
        //TO remove later
        Target = GameObject.FindGameObjectWithTag("Target");
        //
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
        Agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {        
    }
    //private methods
    private void EnabledNavMeshAgent()
    {
        Agent.enabled = true;
        Agent.speed = Speed;
        Agent.SetDestination(Target.transform.position);
    }
}
