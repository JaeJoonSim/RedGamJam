using BlueRiver.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsInitButton : MonoBehaviour
{
    public SettingsVolumeUI[] sliders;
    public Button initButton;

    private void Start()
    {
        initButton.onClick.AddListener(OnClickInit);
    }

    public void OnClickInit()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].volumeSlider.value = 1;
            sliders[i].UpdateVolumeBar(sliders[i].volumeSlider.value);
        }
    }
}
