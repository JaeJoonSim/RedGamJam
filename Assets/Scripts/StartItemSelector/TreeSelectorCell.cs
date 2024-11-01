using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BlueRiver.Items
{ 
    public class TreeSelectorCell : MonoBehaviour
    {
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