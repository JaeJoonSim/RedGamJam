using BlueRiver.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueRiver.UI
{
    public class UI_Scene : UI_Base
    {
        public virtual void Init()
        {
            PopupManager.SetCanvas(gameObject, false);
        }
    }
}