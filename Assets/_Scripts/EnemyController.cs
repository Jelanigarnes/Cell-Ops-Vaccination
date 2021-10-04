using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // Private Instance Variables
    private GameController _gameController;
    private Vector3 _playerVelocity;
    private float _enemySpeed = 10f;
    private Rigidbody _rigidbody;

    //Public Variables
    public GameObject Target;
    public NavMeshAgent agent;

    public float Speed
    {
        get => _enemySpeed;
        set
        {
            _enemySpeed = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        agent.speed = Speed;
        agent.SetDestination(Target.transform.position);
    }
    void Initialize()
    {
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
        agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {        
    }
}
