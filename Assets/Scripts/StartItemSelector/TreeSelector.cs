using BlueRiver.Character;
using BlueRiver.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueRiver.Items
{
    public class TreeSelector : MonoBehaviour
    {
        [SerializeField]
        private Transform cellArea;

        [SerializeField]
        private GameObject selectorCell;

        [SerializeField]
        private int cellCount = 3;

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

                var cell = go.GetComponent<TreeSelectorCell>();
                if (cell != null)
                {
                    cell.SetItem((TreeItemType)i + 1);
                    cell.OnItemSelected += OnItemSelected;
                }
            }
        }

        private void OnItemSelected(TreeItemType treeType)
        {
            if (currentSelectionCount < maxSelectionCount)
            {
                var tree = TreeItemIcon.Instance.SearchTree(treeType);
                Debug.Log($"Selected tree: {treeType}");
                player.SelectTree(tree);
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