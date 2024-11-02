using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlueRiver.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        public void ChangeScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void OnClickSettings()
        {
            PopupManager.ShowPopup<UI_Popup>("Popup Settings");
        }
    }
}