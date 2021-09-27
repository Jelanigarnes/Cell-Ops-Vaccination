using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets._Scripts
{
    public class SettingsManager : MonoBehaviour
    {

        public Toggle FullScreenToggle;
        public Dropdown ResolutionDropdown;
        public Dropdown TextureQualityDropdown;
        public Dropdown AntialisasimDropdown;
        public Dropdown vSyncDropdown;
        public Slider VolumeSlider;
        public AudioSource audiosource;
        public Resolution[] Resolutions;
        public GameSettings GameSettings;
        private GameManager _gameManager;

        void Start()
        {
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            GameSettings = new GameSettings();

            FullScreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
            ResolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
            TextureQualityDropdown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
            AntialisasimDropdown.onValueChanged.AddListener(delegate { OnAntialiasingChange(); });
            vSyncDropdown.onValueChanged.AddListener(delegate { OnVSyncChange(); });
            VolumeSlider.onValueChanged.AddListener(delegate { OnVolumeChange(); });

            Resolutions = Screen.resolutions;
            foreach (Resolution resolution in Resolutions)
            {
                ResolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
            }
            LoadSettings();
        }
        public void OnFullscreenToggle()
        {
            GameSettings.Fullscreen = Screen.fullScreen = FullScreenToggle.isOn;
        }
        public void OnResolutionChange()
        {
            Screen.SetResolution(Resolutions[ResolutionDropdown.value].width, Resolutions[ResolutionDropdown.value].height, Screen.fullScreen);
            GameSettings.ResolutionIndex = ResolutionDropdown.value;
        }
        public void OnTextureQualityChange()
        {
            QualitySettings.masterTextureLimit = GameSettings.TextureQuality = TextureQualityDropdown.value;
        }
        public void OnAntialiasingChange()
        {
            QualitySettings.antiAliasing = GameSettings.Antialiasing = (int)Mathf.Pow(2f, AntialisasimDropdown.value);
        }
        public void OnVSyncChange()
        {
            QualitySettings.vSyncCount = GameSettings.VSync = vSyncDropdown.value;
        }
        public void OnVolumeChange()
        {
            audiosource.volume = GameSettings.MusicVolume = VolumeSlider.value;
        }
        public void SaveSettings()
        {
            string jsonData = JsonUtility.ToJson(GameSettings, true);
            File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
            _gameManager.ChangedSettings();
        }
        public void LoadSettings()
        {
            GameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));
            VolumeSlider.value = GameSettings.MusicVolume;
            AntialisasimDropdown.value = GameSettings.Antialiasing;
            vSyncDropdown.value = GameSettings.VSync;
            TextureQualityDropdown.value = GameSettings.TextureQuality;
            ResolutionDropdown.value = GameSettings.ResolutionIndex;
            FullScreenToggle.isOn = GameSettings.Fullscreen;

            ResolutionDropdown.RefreshShownValue();
        }
        public void Back()
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}