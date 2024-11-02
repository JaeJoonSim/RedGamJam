using BlueRiver.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueRiver.UI
{
    public class UI_Popup : UI_Base
    {
        protected virtual void Init()
        {
            PopupManager.SetCanvas(gameObject);
        }

        public virtual void ClosePopup()
        {
            PopupManager.ClosePopup(this);
        }
        
        protected virtual void OnEnable()
        {
            Init();
        }

        protected virtual void OnDestroy()
        {
            if (this != null)
                PopupManager.ClosePopup(this);
        }
    }
}