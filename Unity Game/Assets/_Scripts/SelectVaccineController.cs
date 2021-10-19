using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectVaccineController : MonoBehaviour
{
    private GameManager _gameManager;

    // PUBLIC INSTANCE VARIABLES
    public AudioSource CharacterSelectSound;

    //[Header("Buttons")]
    //public Button Pfizer;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        Cursor.visible = true;
        CharacterSelectSound.volume = _gameManager.GameSettings.MusicVolume;
    }
    /// <summary>
    /// Initialize
    /// mainly used to find game objects
    /// </summary>
    void Initialize()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Goes back to Menu Scene
    /// </summary>
    public void Back()
    {
        SceneManager.UnloadSceneAsync("Vaccine Select");
        //SceneManager.LoadScene("Menu");
    }
    /// <summary>
    /// Selects a character and returns user back to main menu
    /// </summary>
    public void CharacerSelect(string ability)
    {
        _gameManager.AbilityChoice = ability;
        _gameManager.AbilitySelected = true;
        Back();
    }
}
