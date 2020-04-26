using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderDefaultSetter : MonoBehaviour {
    public enum SliderValueList
    {
        bgmVolume,
        sfxVolume
    }

    public SliderValueList value;
    private Slider slider;

	// Use this for initialization
	void Start () {
        slider = GetComponent<Slider>();
        if (value == SliderValueList.bgmVolume)
            slider.value = Settings.current_settings.music_volume;
        if (value == SliderValueList.sfxVolume)
            slider.value = Settings.current_settings.sfx_volume;
    }
}
