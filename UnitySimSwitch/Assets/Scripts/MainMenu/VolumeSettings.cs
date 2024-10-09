using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class VolumeSettings : MonoBehaviour
{
    #region Fields
    [SerializeField] private AudioMixer _mixer = null;
    [SerializeField] private Slider _masterVolume = null;
    [SerializeField] private Slider _musicSlider = null;
    [SerializeField] private Slider _sfxSlider = null;
    #endregion Fields

    #region Methods

    private void Start()
    {
        if(PlayerPrefs.HasKey("MasterVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMasterVolume();
            SetMusicVolume();
            SetSFXVolume();
        }

    }

    public void SetMasterVolume()
    {
        float volume = _masterVolume.value;
        _mixer.SetFloat("Master", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume()
    {
        float volume = _musicSlider.value;
        _mixer.SetFloat("Music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = _sfxSlider.value;
        _mixer.SetFloat("SFX", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadVolume()
    {
        _masterVolume.value = PlayerPrefs.GetFloat("MasterVolume");
        _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
    }

    #endregion Methods
}