using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPointer : MonoBehaviour
{
    GameController _gameController;
    GameObject _helpTarget;

    public GameObject HelpTarget { get => _helpTarget; set => _helpTarget = value; }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void Initialize() {
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }
    

    // Update is called once per frame
    void Update()
    {
        if (_gameController.IsGamePause)
        {

        }
        else
        {
            //transform.RotateAround(_gameController.Player.transform.position, _gameController.transform.up, 1);
            transform.LookAt(HelpTarget.transform);
        }        
    }
}
