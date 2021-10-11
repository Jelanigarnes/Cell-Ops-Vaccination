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
        SetDestination(Target);
        _rigidbodyConstraints = _rigidbody.constraints;
    }
    void Initialize()
    {
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
        Agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();   }
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

            if (_healthPoints < 0)
            {
                Object.Destroy(this.gameObject);
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
        Debug.Log("Enemy Died");
        _gameController.EnemyDied(this.gameObject);
    }
}
