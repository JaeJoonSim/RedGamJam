using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlueRiver.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        public void Start()
        {
            SoundManager.Instance.PlaySound("MainMenu");
        }

        public void ChangeScene(string sceneName)
        {
            if(sceneName == "Stage1")
            {
                SoundManager.Instance.PlaySound("MainGameStart");
            }
            SceneManager.LoadScene(sceneName);
        }

        public void OnClickSettings()
        {
            PopupManager.ShowPopup<UI_Popup>("Popup Settings");
        }
    }
}