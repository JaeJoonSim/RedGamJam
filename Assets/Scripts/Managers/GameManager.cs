using BlueRiver.Character;
using BlueRiver.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.SimpleLocalization;
using Assets.SimpleLocalization.Scripts;
using BlueRiver.Items;
using UnityEngine.SceneManagement;
using System;
using DialogueEditor;
using System.Linq;
using static BlueRiver.GameManager;

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
        private string prevSceneName = "";

        public List<TreeItemType> SelectedTreeList = new List<TreeItemType>();
        public StartItemType startItemType = StartItemType.None;
        public List<DialogueDatas> caveDialogue = new List<DialogueDatas>();

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

        private void Start()
        {
            prevSceneName = SceneManager.GetActiveScene().name;
        }

        private void OnEnable()
        {
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            SceneManager.sceneLoaded += OnSceneLoaded;

            prevSceneName = SceneManager.GetActiveScene().name;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            // 이전 씬의 이름을 저장합니다.
            prevSceneName = scene.name;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (player != null)
                PopupManager.ShowPopup<UI_Popup>("Item Selector");
            SelectedTreeList.Clear();

            NPCConversation dialogueData = null;
            if (caveDialogue.Count > 0)
                dialogueData = caveDialogue[0].NPCConversation;

            if (caveDialogue.Count == 1)
                ConversationManager.OnConversationEnded += ConversationEnd;

            if (SceneManager.GetActiveScene().name == "Cave")
            {
                if (dialogueData != null)
                {
                    ConversationManager.Instance.StartConversation(dialogueData);

                    caveDialogue.Remove(caveDialogue[0]);
                }
            }
        }

        public void ConversationEnd()
        {
            ConversationManager.Instance.StartConversation(caveDialogue[0].NPCConversation);
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

        [Serializable]
        public class DialogueDatas
        {
            public NPCConversation NPCConversation;
            public string sceneName;
        }

    }
}