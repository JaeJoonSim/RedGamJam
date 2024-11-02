using BlueRiver.Character;
using BlueRiver.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.SimpleLocalization;
using Assets.SimpleLocalization.Scripts;

namespace BlueRiver
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public PlayerController player;
        public PopupPause popupPause;

        private ParticleSystem snowStormEffect;
        private bool canClickEscape = true;

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
                    PopupManager.ClosePopup();
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
    }
}