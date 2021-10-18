using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Private Instance Variables
    private GameController _gameController;
    private bool _groundedPlayer;
    private Vector3 _playerVelocity;
    private float _playerSpeed= 5.0f;
    private Rigidbody _rigidbody;
    private bool _sprint;
    private CameraDirection _cameraDirection = CameraDirection.x;
    private float _cameraHeight = 20f;
    private float _cameraDistance = 7f;
    private float _gravity = 14.0f;
    private float _maxVelocityChange = 10.0f;
    private AudioSource _audioSource;

    //Mouse cursor Camera offset effect
    private Vector2 _playerPosOnScreen;
    private Vector2 _cursorPosition;
    private Vector2 _offsetVector;
    private GameObject _targetObject;
    //Plane that represents imaginary floor that will be used to calculate Aim target position
    private Plane _surfacePlane = new Plane();

    //Public Variables
    public Camera PlayerCamera;
    public GameObject targetIndicatorPrefab;
    public GameObject LaserPrefab;
    public Transform WeaponNozel;
    public enum CameraDirection { x, z }
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
        _rigidbody.useGravity = false;
        _rigidbody.freezeRotation = true;

        this.transform.position = _gameController.PlayerRespawnLocation.transform.position;
        Sprint = false;

        //Instantiate aim target prefab
        if (targetIndicatorPrefab)
        {
            _targetObject = Instantiate(targetIndicatorPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        }
    }
    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Initialize()
    {
        PlayerCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
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
            if (Input.GetButtonDown("Fire"))
            {
                _audioSource.Play();
                GameObject laser = Instantiate(
                    LaserPrefab, 
                    WeaponNozel.position, 
                    this.transform.rotation);
                laser.GetComponent<Rigidbody>().AddForce(transform.forward * 8000f);
            }
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

            Movement();
        }
    }
    //Private methods
    /// <summary>
    /// Since it was so much to move character,rotate and other camera stuff i decided to make a method about it
    /// </summary>
    private void Movement()
    {
        //Setup camera offset
        Vector3 cameraOffset = Vector3.zero;
        if (_cameraDirection == CameraDirection.x)
        {
            cameraOffset = new Vector3(_cameraDistance, _cameraHeight, 0);
        }
        else if (_cameraDirection == CameraDirection.z)
        {
            cameraOffset = new Vector3(0, _cameraHeight, _cameraDistance);
        }

        //if (grounded)
        //{
            Vector3 targetVelocity = Vector3.zero;
            // Calculate how fast we should be moving
            if (_cameraDirection == CameraDirection.x)
            {
                targetVelocity = new Vector3(Input.GetAxis("Vertical") * (_cameraDistance >= 0 ? -1 : 1), 0, Input.GetAxis("Horizontal") * (_cameraDistance >= 0 ? 1 : -1));
            }
            else if (_cameraDirection == CameraDirection.z)
            {
                targetVelocity = new Vector3(Input.GetAxis("Horizontal") * (_cameraDistance >= 0 ? -1 : 1), 0, Input.GetAxis("Vertical") * (_cameraDistance >= 0 ? -1 : 1));
            }
            targetVelocity *= PlayerSpeed;

            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = _rigidbody.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -_maxVelocityChange, _maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -_maxVelocityChange, _maxVelocityChange);
            velocityChange.y = 0;
            _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

            // Jump
            //if (canJump && Input.GetButton("Jump"))
            //{
            //    r.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
            //}
        //}

        // We apply gravity manually for more tuning control
        _rigidbody.AddForce(new Vector3(0, -_gravity * _rigidbody.mass, 0));

        //grounded = false;

        //Mouse cursor offset effect
        _playerPosOnScreen = PlayerCamera.WorldToViewportPoint(transform.position);
        _cursorPosition = PlayerCamera.ScreenToViewportPoint(Input.mousePosition);
        _offsetVector = _cursorPosition - _playerPosOnScreen;

        //Camera follow
        PlayerCamera.transform.position = Vector3.Lerp(PlayerCamera.transform.position, transform.position + cameraOffset, Time.deltaTime * 7.4f);
        PlayerCamera.transform.LookAt(transform.position + new Vector3(-_offsetVector.y * 2, 0, _offsetVector.x * 2));

        //Aim target position and rotation
        _targetObject.transform.position = GetAimTargetPos();
        _targetObject.transform.LookAt(new Vector3(transform.position.x, _targetObject.transform.position.y, transform.position.z));

        //Player rotation
        transform.LookAt(new Vector3(_targetObject.transform.position.x, transform.position.y, _targetObject.transform.position.z));
    }
    Vector3 GetAimTargetPos()
    {
        //Update surface plane
        _surfacePlane.SetNormalAndPosition(Vector3.up, transform.position);

        //Create a ray from the Mouse click position
        Ray ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);

        //Initialise the enter variable
        float enter = 0.0f;

        if (_surfacePlane.Raycast(ray, out enter))
        {
            //Get the point that is clicked
            Vector3 hitPoint = ray.GetPoint(enter);

            //Move your cube GameObject to the point where you clicked
            return hitPoint;
        }

        //No raycast hit, hide the aim target by moving it far away
        return new Vector3(-5000, -5000, -5000);
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
