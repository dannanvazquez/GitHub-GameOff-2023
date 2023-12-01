using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSliderController : MonoBehaviour
{
    public AudioMixer audioMixer; // Drag your Audio Mixer here in the Inspector

    // Define parameter names
    private const string MasterVolumeParam = "MasterVolume";
    private const string SFXVolumeParam = "SFXVolume";
    private const string VoicesVolumeParam = "VoicesVolume";
    private const string MusicVolumeParam = "MusicVolume";
    private const string AmbientVolumeParam = "AmbientVolume";

    public void SetMasterVolume()
    {
        SetParameter(MasterVolumeParam);
    }

    public void SetSFXVolume()
    {
        SetParameter(SFXVolumeParam);
    }

    public void SetVoicesVolume()
    {
        SetParameter(VoicesVolumeParam);
    }

    public void SetMusicVolume()
    {
        SetParameter(MusicVolumeParam);
    }

    public void SetAmbientVolume()
    {
        SetParameter(AmbientVolumeParam);
    }

    private void SetParameter(string parameterName)
    {
        // Assuming the parameter is a float
        float sliderValue = GetComponent<Slider>().value;

        // Set the parameter in the Audio Mixer
        audioMixer.SetFloat(parameterName, sliderValue);
    }
}