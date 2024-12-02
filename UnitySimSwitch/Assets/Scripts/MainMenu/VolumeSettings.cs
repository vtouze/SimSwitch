using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Manages the audio volume settings for master volume, music volume, and SFX volume.
/// Saves and loads volume settings using PlayerPrefs to persist across sessions.
/// </summary>
public class VolumeSettings : MonoBehaviour
{
    #region Fields
    [Header("Audio Mixer")]
    [Tooltip("The AudioMixer used to adjust various audio categories like Master, Music, and SFX.")]
    [SerializeField] private AudioMixer _mixer = null;

    [Header("Volume Sliders")]
    [Tooltip("Slider for adjusting the master volume.")]
    [SerializeField] private Slider _masterVolume = null;

    [Tooltip("Slider for adjusting the music volume.")]
    [SerializeField] private Slider _musicSlider = null;

    [Tooltip("Slider for adjusting the sound effects (SFX) volume.")]
    [SerializeField] private Slider _sfxSlider = null;
    #endregion Fields

    #region Methods
    /// <summary>
    /// Initializes volume settings by loading saved preferences or setting default values.
    /// </summary>
    private void Start()
    {
        // Check if volume settings exist in PlayerPrefs
        if(PlayerPrefs.HasKey("MasterVolume"))
        {
            LoadVolume(); // Load saved volume settings
        }
        else
        {
            SetMasterVolume(); // Set default values
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    /// <summary>
    /// Sets the master volume level based on the slider value and applies it to the AudioMixer.
    /// Also saves the volume setting to PlayerPrefs.
    /// </summary>
    public void SetMasterVolume()
    {
        // Get the volume value from the slider and convert it to a logarithmic scale for audio adjustments
        float volume = _masterVolume.value;
        _mixer.SetFloat("Master", Mathf.Log10(volume) * 20); // Set the master volume in the AudioMixer
        PlayerPrefs.SetFloat("MasterVolume", volume); // Save the master volume setting
    }

    /// <summary>
    /// Sets the music volume level based on the slider value and applies it to the AudioMixer.
    /// Also saves the volume setting to PlayerPrefs.
    /// </summary>
    public void SetMusicVolume()
    {
        float volume = _musicSlider.value;
        _mixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    /// <summary>
    /// Sets the SFX (sound effects) volume level based on the slider value and applies it to the AudioMixer.
    /// Also saves the volume setting to PlayerPrefs.
    /// </summary>
    public void SetSFXVolume()
    {
        float volume = _sfxSlider.value;
        _mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    /// <summary>
    /// Loads the saved volume settings from PlayerPrefs and updates the sliders and AudioMixer values.
    /// </summary>
    private void LoadVolume()
    {
        // Load saved volume values from PlayerPrefs and apply them to the sliders
        _masterVolume.value = PlayerPrefs.GetFloat("MasterVolume");
        _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        // Apply the loaded values to the AudioMixer
        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
    }
    #endregion Methods
}