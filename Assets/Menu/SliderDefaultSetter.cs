using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderDefaultSetter : MonoBehaviour {
    public enum SliderValueList
    {
        bgmVolume,
        sfxVolume
    }

    public SliderValueList value;
    private UISlider slider;

	// Use this for initialization
	void Start () {
        slider = GetComponent<UISlider>();
        if (value == SliderValueList.bgmVolume)
            slider.sliderValue = Settings.current_settings.music_volume;
        if (value == SliderValueList.sfxVolume)
            slider.sliderValue = Settings.current_settings.sfx_volume;
    }
}
