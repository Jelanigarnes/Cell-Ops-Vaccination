using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectVaccineController : MonoBehaviour
{
    private GameManager _gameManager;
    private string _ability;

    // PUBLIC INSTANCE VARIABLES
    public AudioSource CharacterSelectSound;
    public Button Continue;

    public string Ability { get => _ability; set => _ability = value; }

    //[Header("Buttons")]
    //public Button Pfizer;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        Cursor.visible = true;
        CharacterSelectSound.volume = _gameManager.GameSettings.MusicVolume;
        if (!_gameManager.AbilitySelected)
        {
            Continue.interactable = false;
        }
    }
    /// <summary>
    /// Initialize
    /// mainly used to find game objects
    /// </summary>
    void Initialize()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Continue = GameObject.Find("BtnContinue").GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Conintue()
    {
        _gameManager.AbilityChoice = Ability;
        _gameManager.AbilitySelected = true;
        SceneManager.UnloadSceneAsync("Vaccine Select");
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
    /// <param name="ability">Players Ability. WARNING Case sensitive</param>
    public void AbilitySelect(string ability)
    {
        Ability = ability;
        Continue.interactable = true;
    }
}
