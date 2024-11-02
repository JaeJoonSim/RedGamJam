using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace BlueRiver.UI
{
    public class SettingsVolumeUI : MonoBehaviour
    {
        public Image[] volumeSegments;
        public Slider volumeSlider;
        public Button gaugeUp;
        public Button gaugeDown;
        public AudioMixer mixer;
        public string mixerName;

        void Start()
        {
            volumeSlider.onValueChanged.AddListener(UpdateVolumeBar);
            volumeSlider.value = 1f;
            UpdateVolumeBar(volumeSlider.value);

            gaugeUp.onClick.AddListener(() => volumeSlider.value += 0.1f);
            gaugeDown.onClick.AddListener(() => volumeSlider.value -= 0.1f);
        }

        public void UpdateVolumeBar(float value)
        {
            int activeSegments = Mathf.RoundToInt(value * volumeSegments.Length);

            for (int i = 0; i < volumeSegments.Length; i++)
            {
                volumeSegments[i].enabled = i < activeSegments;
            }

            mixer.SetFloat(mixerName, value);
        }
    }
}