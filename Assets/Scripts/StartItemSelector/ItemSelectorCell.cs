using BlueRiver.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlueRiver.ItemSelector
{
    public class ItemSelectorCell : MonoBehaviour
    {
        private Button selectButton;

        private void Start()
        {
            selectButton = Utils.Utils.FindChild<Button>(gameObject, "Button");

            selectButton.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            Debug.Log("ItemSelectorCell OnClick");
        }
    }
}