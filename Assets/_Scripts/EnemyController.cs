using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    // Private Instance Variables
    private GameController _gameController;
    private Vector3 _playerVelocity;
    private float _enemySpeed = 1f;
    private Rigidbody _rigidbody;
    private string _enemyType;
    private float _health;
    private float _maxHealth;
    private int _enemyDmg;
    private RigidbodyConstraints _rigidbodyConstraints;
    private AudioSource _audioSource;

    //Public Variables
    public GameObject Target;
    public NavMeshAgent Agent;
    public GameObject HealthBar;
    public Slider HealthbarSlider;

    public float Speed
    {
        get => _enemySpeed;
        set
        {
            _enemySpeed = value;
        }
    }

    public float Health { 
        get => _health;
        set
        {
            _health = value;
            //make sure health can't go over max health
            if (_health > MaxHealth)
            {
                _health = MaxHealth;
            }
            if(_health < MaxHealth)
            {
                HealthBar.SetActive(true);
            }
            if (_health < 0)
            {
                _audioSource.volume = 100;
                _audioSource.Play();
                Object.Destroy(this.gameObject);
            }
            HealthbarSlider.value = _health;
        }
    }
    public string EnemyType { get => _enemyType; set => _enemyType = value; }
    public int EnemyDmg { get => _enemyDmg; set => _enemyDmg = value; }
    public float MaxHealth { get => _maxHealth; 
        set
        {
            _maxHealth = value;
            Health = _maxHealth;
            HealthbarSlider.maxValue = _maxHealth;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        Agent.speed = Speed;
        SetDestination(Target);
        _rigidbodyConstraints = _rigidbody.constraints;
    }
    void Initialize()
    {
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
        Agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (_gameController.IsGamePause || _gameController.IsGameOver)
        {
            Agent.isStopped=true;
            Agent.ResetPath();
            _rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
            _rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;            
        }
        else
        {
            Agent.SetDestination(Target.transform.position);
            //remove all constraits and reset
            _rigidbody.constraints = _rigidbodyConstraints;

            //resume persute
            Agent.isStopped=false;

            if (Agent.hasPath)
            {
                Agent.destination = Target.transform.position;
            }
        }        

    }
    /// <summary>
    /// Finds a new target to attack.
    /// Will be the closest target
    /// </summary>
    /// <param name="Targets">List of targets</param>
    public void NewTarget(List<GameObject> Targets)
    {
        float distanceToClosestTarget = Mathf.Infinity;
        GameObject closestTarget = null;
        foreach(GameObject target in Targets)
        {
            float distanceToTarget = (target.transform.position - this.transform.position).sqrMagnitude;
            if(distanceToTarget < distanceToClosestTarget)
            {
                distanceToClosestTarget = distanceToTarget;
                closestTarget = target;
            }
        }
        SetDestination(closestTarget);
    }
    //// PRIVATE METHODS
    /// <summary>
    /// Sets Enemy Navmeshagent Destination
    /// </summary>
    /// <param name="Target">Game Object to set destination</param>
    private void SetDestination(GameObject Target)
    {
        Agent.SetDestination(Target.transform.position);
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // Calculate Angle Between the collision point and the player
            Vector3 away = collision.contacts[0].point - this.gameObject.transform.position;
            // We then get the opposite (-Vector3) and normalize it
            //away = -away.normalized;

            collision.gameObject.GetComponent<Rigidbody>().AddForce(away * 1000.0f);
        }
        if(collision.gameObject.tag == "Target")
        {
            collision.gameObject.GetComponent<TargetController>().TakeDamage(EnemyDmg);
        }
        
    }
    /// <summary>
    /// Called when object is destroyed
    /// </summary>
    private void OnDestroy()
    {
        _gameController.EnemyDied(this.gameObject);
    }
}
