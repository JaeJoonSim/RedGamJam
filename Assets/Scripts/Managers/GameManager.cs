using BlueRiver.Character;
using BlueRiver.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueRiver
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public PlayerController player;
        public PopupPause popupPause;

        private bool canClickEscape = true;
        
        private void Update()
        {
            OnClickEscape();
        }

        private void OnClickEscape()
        {
            if (PopupManager.GetPopupCount() > 0)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!canClickEscape)
                {
                    if (popupPause == null)
                        PopupManager.ShowPopup<UI_Popup>("Popup Pause");
                    else
                        PopupManager.ClosePopup(popupPause);
                    return;
                }

                if (PopupManager.GetPopupCount() <= 0)
                    PopupManager.ShowPopup<UI_Popup>("Popup Pause");
                else
                    PopupManager.ClosePopup();
            }
        }

        public void SetClickEscape(bool value)
        {
            canClickEscape = value;
        }
    }
}