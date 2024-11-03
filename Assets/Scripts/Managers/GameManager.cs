using BlueRiver.Character;
using BlueRiver.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.SimpleLocalization;
using Assets.SimpleLocalization.Scripts;
using BlueRiver.Items;
using UnityEngine.SceneManagement;

namespace BlueRiver
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public PlayerController player;
        public PopupPause popupPause;
        public bool playerDeath = false;
        public bool playerIgnoreSnowStorm = false;
        public bool playerCanMove = true;

        private ParticleSystem snowStormEffect;
        private bool canClickEscape = true;

        public List<TreeItemType> SelectedTreeList = new List<TreeItemType>();

        protected override void Awake()
        {
            base.Awake();

            LocalizationManager.Read();

            switch (Application.systemLanguage)
            {
                case SystemLanguage.English:
                    LocalizationManager.Language = "English";
                    break;
                case SystemLanguage.Korean:
                    LocalizationManager.Language = "Korean";
                    break;
            }
        }

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
                if (PopupManager.GetPopupCount() <= 0)
                    PopupManager.ShowPopup<UI_Popup>("Popup Pause");
                else
                {
                    if (playerDeath == false)
                        PopupManager.ClosePopup();
                }
            }
        }

        public void SetLocalization(string localization)
        {
            LocalizationManager.Language = localization;
        }

        public void AddColliderSnowParticle(Component collider)
        {
            if (snowStormEffect == null) return;
            snowStormEffect.trigger.AddCollider(collider);
        }

        public ParticleSystem GetSnowStorm()
        {
            return snowStormEffect;
        }

        public void SetSnowStormEffect(ParticleSystem snowStormEffect)
        {
            this.snowStormEffect = snowStormEffect;
        }

        public void SetClickEscape(bool value)
        {
            canClickEscape = value;
        }

        public void ChangeScene(string sceneName = null)
        {
            playerDeath = false;

            if (string.IsNullOrEmpty(sceneName))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            else
                SceneManager.LoadScene(sceneName);
        }
    }
}