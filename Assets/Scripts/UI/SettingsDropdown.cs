using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BlueRiver.UI
{
    public class SettingsDropdown : MonoBehaviour
    {
        public void SelectedDropDownMenu(int value)
        {
            switch (value)
            {
                case 0:
                    GameManager.Instance.SetLocalization("Korean");
                    break;
                case 1:
                    GameManager.Instance.SetLocalization("English");
                    break;
            }

        }

    }
}