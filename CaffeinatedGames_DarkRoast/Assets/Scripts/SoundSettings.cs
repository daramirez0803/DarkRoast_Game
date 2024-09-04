//Followed tutorial found here: https://youtube.com/shorts/_m6nTQOMFl0?si=9BRrgvqrEj7ub2xp

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] private Slider soundSlider;
    [SerializeField] private AudioMixer masterMixer;

    private void Start()
    {
    }

    public void SetVolume(float _value)
    {
        RefreshSlider(_value);
        if (_value < 1)
        {
            _value = .0001f;
            masterMixer.SetFloat("MasterVolume", -80f);
        } else {
            masterMixer.SetFloat("MasterVolume", Mathf.Log10(_value / 100) * 20f);
        }
        PersistentValues.instance.masterVolumeFloat = _value;

    }

    public void SetVolumeFromSlider()
    {
        SetVolume(soundSlider.value);
    }

    public void RefreshSlider(float _value)
    {
        soundSlider.value = _value;
    }
}