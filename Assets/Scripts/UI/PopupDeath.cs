using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlueRiver.UI
{
    public class PopupDeath : UI_Popup
    {
        [SerializeField]
        private Button restartButton;

        [SerializeField]
        private Button MainMenuButton;

        private void Start()
        {
            restartButton.onClick.AddListener(() =>
            {
                GameManager.Instance.ChangeScene();
            });

            MainMenuButton.onClick.AddListener(() =>
            {
                GameManager.Instance.ChangeScene("MainMenu");
            });
        }
    }
}