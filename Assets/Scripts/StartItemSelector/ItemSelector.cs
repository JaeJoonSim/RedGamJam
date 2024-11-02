using BlueRiver.Character;
using BlueRiver.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueRiver.Items
{
    public class ItemSelector : MonoBehaviour
    {
        [SerializeField]
        private Transform cellArea;

        [SerializeField]
        private GameObject selectorCell;

        [SerializeField]
        private int cellCount = 4;

        [SerializeField]
        private int maxSelectionCount = 1;

        private PlayerController player;
        private int currentSelectionCount = 0;

        private void Start()
        {
            player = GameManager.Instance.player;

            GameManager.Instance.SetClickEscape(false);

            for (int i = 0; i < cellCount; i++)
            {
                GameObject go = Instantiate(selectorCell, cellArea);
                go.name = $"Item Select Cell_{i}";
                
                var cell = go.GetComponent<ItemSelectorCell>();
                if (cell != null)
                {
                    cell.SetItem((StartItemType)i + 1);
                    cell.OnItemSelected += OnItemSelected;
                }
            }
        }

        private void OnItemSelected(StartItemType itemType)
        {
            if (currentSelectionCount < maxSelectionCount)
            {
                player.SelectItem(itemType);
                currentSelectionCount++;

                if (currentSelectionCount == maxSelectionCount)
                {
                    var UIPopup = GetComponent<UI_Popup>();
                    PopupManager.ClosePopup(UIPopup);
                }
            }
        }
    }
}