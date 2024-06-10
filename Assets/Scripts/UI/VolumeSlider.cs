using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Slider slider;

    enum SourceType {Music,
                     Volume}

    [SerializeField] SourceType sourseName;

    private void Awake()
    {
        
        slider = GetComponent<Slider>();



        switch (sourseName)
        {
            case SourceType.Music:
                slider.SetValueWithoutNotify(PlayerPrefs.GetFloat("musicVolume", 1) * slider.maxValue);
                slider.onValueChanged.AddListener((v) =>
                {
                    SoundManager.instance.ChangeMusicVolume(slider.value / slider.maxValue);
                });
                break;
            case SourceType.Volume:
                slider.SetValueWithoutNotify(PlayerPrefs.GetFloat("soundVolume", 1) * slider.maxValue);
                slider.onValueChanged.AddListener((v) =>
                {
                    SoundManager.instance.ChangeSoundVolume(slider.value / slider.maxValue);
                });
                break;

        }
    }
}
