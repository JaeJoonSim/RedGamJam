using BlueRiver.Character;
using BlueRiver.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlueRiver.Items
{
    public class ItemSelectorCell : MonoBehaviour
    {
        private Button selectButton;
        private StartItemType itemType;

        private void Start()
        {
            selectButton = Utils.Utils.FindChild<Button>(gameObject, "Button");

            selectButton.onClick.AddListener(OnClick);
        }

        public void SetItem(StartItemType itemType)
        {
            this.itemType = itemType;
        }

        private void OnClick()
        {
            Debug.Log("ItemSelectorCell OnClick");
            PlayerController player = FindObjectOfType<PlayerController>();
            player.SelectItem(itemType);
        }
    }
}