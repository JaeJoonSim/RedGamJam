using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlueRiver.UI
{
    public class PopupPause : UI_Popup
    {
        [SerializeField]
        private Button backButton;

        [SerializeField]
        private Button settingsButton;

        [SerializeField]
        private Button restartButton;

        [SerializeField]
        private Button mainMenuButton;

        private void Start()
        {
            backButton.onClick.AddListener(OnClickBackButton);
            settingsButton.onClick.AddListener(OnClickSettingsButton);
            restartButton.onClick.AddListener(OnClickRestartButton);
            mainMenuButton.onClick.AddListener(OnClickMainMenuButton);
        }

        private void OnClickBackButton()
        {
            ClosePopup();
        }

        private void OnClickSettingsButton()
        {

        }

        private void OnClickRestartButton()
        {

        }

        private void OnClickMainMenuButton()
        {

        }
    }
}