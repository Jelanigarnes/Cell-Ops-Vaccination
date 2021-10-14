using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Private Instance Variables
    private GameController _gameController;
    private bool _groundedPlayer;
    private Vector3 _playerVelocity;
    private float _playerSpeed= 3f;
    private Rigidbody _rigidbody;
    private bool _sprint;

    //Public Variables
    public Camera PlayerCamera;
    public float PlayerSpeed
    {
        get => _playerSpeed;
        set
        {
            _playerSpeed = value;
        }
    }

    public bool Sprint { get => _sprint; set => _sprint = value; }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        this.transform.position = _gameController.PlayerRespawnLocation.transform.position;
        Sprint = false;
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
        if (!_gameController.IsGamePause)
        {
            //Activate Ability
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
            {
                Ability(_gameController.PlayerAbility);
            }
            //Activet Sprint
            if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) && !Sprint))
            {
                Sprint = true;
                PlayerSpeed *= 2;
            }
            if ((Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift) && Sprint))
            {
                PlayerSpeed /= 2;
                Sprint = false;
            }
            //Rotate player to mouse
            //Vector2 mousePosition = new Vector2(
            //    Input.GetAxis("Mouse X"),
            //    Input.GetAxis("Mouse Y")
            //);
            //transform.LookAt(mousePosition);
        }
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
    //Private methods
    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
    private void Ability(string _abilityType)
    {
        switch (_abilityType)
        {
            case "Phizer-BioNTech":
                Debug.Log("Phizer-BioNTech Ability.");
                break;
            case "Johnson & Johnson":
                Debug.Log("Johnson & Johnson Ability.");
                break;
            case "Moderna":
                Debug.Log("Moderna");
                break;
            default:
                Debug.Log("No Ability Written yet.");
                break;
        }
    }
    private void OnCollsionEnter(Collision collision)
    {
        Debug.Log("Object collided: " + collision.gameObject.name);
    }
}
