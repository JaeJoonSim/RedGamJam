using BlueRiver.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueRiver.UI
{
    public class UI_Popup : UI_Base
    {
        public virtual void Init()
        {
            PopupManager.SetCanvas(gameObject, true);
        }

        public virtual void ClosePopup()
        {
            PopupManager.ClosePopup(this);
        }

        private void OnEnable()
        {
            Init();
        }

        private void OnDestroy()
        {
            if (this != null)
                PopupManager.ClosePopup(this);
        }
    }
}