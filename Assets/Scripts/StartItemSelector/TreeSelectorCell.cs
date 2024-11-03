using Assets.SimpleLocalization.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BlueRiver.Items
{ 
    public class TreeSelectorCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Transform iconArea;

        public TextMeshProUGUI title;
        public TextMeshProUGUI weight;
        public TextMeshProUGUI temperature;
        public TextMeshProUGUI damaged;

        private Button selectButton;
        private TreeItemType treeType;
        public event System.Action<TreeItemType> OnItemSelected;

        private void Start()
        {
            selectButton = Utils.Utils.FindChild<Button>(gameObject, "Button");

            if (selectButton != null)
            {
                selectButton.onClick.AddListener(OnClick);
                selectButton.gameObject.SetActive(false);
            }
        }

        public void SetItem(TreeItemType itemType)
        {
            this.treeType = itemType;

            var tree = TreeItemIcon.Instance.SearchTree(itemType);

            if (tree != null)
            {
                Tree go = Instantiate(tree, iconArea);
                go.name = "Tree Icon";

                title.SetText(LocalizationManager.Localize(tree.LocalizationKey));
                weight.SetText($"{tree.DisplayWeight} kg");
                temperature.SetText($"{tree.GetMaxTemperature()}");
                damaged.SetText(LocalizationManager.Localize(tree.treeDamageKey));
            }
        }

        private void OnClick()
        {
            Debug.Log("TreeSelectorCell OnClick");
            OnItemSelected?.Invoke(treeType);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (selectButton != null)
                selectButton.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (selectButton != null)
                selectButton.gameObject.SetActive(false);
        }
    }
}