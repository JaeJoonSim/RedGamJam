using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
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

            GameManager.Instance.popupPause = this;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (GameManager.Instance != null)
                GameManager.Instance.popupPause = null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (PopupManager._popupStack.Last() == this)
                    ClosePopup();
            }
        }

        private void OnClickBackButton()
        {
            ClosePopup();
        }

        private void OnClickSettingsButton()
        {
            PopupManager.ClosePopup();
            PopupManager.ShowPopup<UI_Popup>("Popup Settings");
        }

        private void OnClickRestartButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnClickMainMenuButton()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}