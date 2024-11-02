using BlueRiver.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueRiver.UI
{
    public class PopupManager
    {
        private static int _order = 10;

        static Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
        static UI_Scene _sceneUI = null;

        public static GameObject Root
        {
            get
            {
                GameObject root = GameObject.Find("Canvas");
                if (root == null)
                    root = new GameObject { name = "Canvas" };

                Utils.Utils.GetOrAddComponent<Canvas>(root);
                Utils.Utils.GetOrAddComponent<UnityEngine.UI.GraphicRaycaster>(root);
                return root;
            }
        }

        public static void SetCanvas(GameObject go, bool sort = true)
        {
            Canvas canvas = Utils.Utils.GetOrAddComponent<Canvas>(go);
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;

            if (sort)
            {
                canvas.sortingOrder = _order;
                _order++;
            }
            else
            {
                canvas.sortingOrder = 0;
            }

            Utils.Utils.GetOrAddComponent<UnityEngine.UI.GraphicRaycaster>(go);
        }

        public static T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            GameObject go = ResourceManager.Instantiate($"UI/{name}", parent);

            return Utils.Utils.GetOrAddComponent<T>(go);
        }

        public static T ShowSceneUI<T>(string name = null) where T : UI_Scene
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;
            
            GameObject go = ResourceManager.Instantiate($"UI/Scene/{name}", Root.transform);
            T sceneUI = Utils.Utils.GetOrAddComponent<T>(go);
            _sceneUI = sceneUI;

            return sceneUI;
        }

        public static T ShowPopup<T>(string name = null) where T : UI_Popup
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            GameObject go = ResourceManager.Instantiate($"UI/Popup/{name}", Root.transform);

            T popup = Utils.Utils.GetOrAddComponent<T>(go);
            _popupStack.Push(popup);

            return popup;
        }

        public static void ClosePopup(UI_Popup popup)
        {
            if (_popupStack.Count == 0)
                return;

            if (_popupStack.Peek() != popup)
            {
                Debug.Log("ClosePopup Failed");
                return;
            }

            ClosePopup();
        }

        public static void ClosePopup()
        {
            if (_popupStack.Count == 0)
                return;

            UI_Popup popup = _popupStack.Pop();
            ResourceManager.Destroy(popup.gameObject);
            popup = null;
            _order--;
        }

        public static void CloseAllPopup()
        {
            while (_popupStack.Count > 0)
                ClosePopup();
        }

        public static int GetPopupCount()
        {
            return _popupStack.Count;
        }
    }
}