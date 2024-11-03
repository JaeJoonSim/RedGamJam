using Assets.SimpleLocalization.Scripts;
using BlueRiver.Character;
using BlueRiver.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BlueRiver.Items
{
    public class ItemSelectorCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Button selectButton;
        private StartItemType itemType;
        public event System.Action<StartItemType> OnItemSelected;

        public TextMeshProUGUI itemName;
        public TextMeshProUGUI itemDescription;
        public Transform iconArea;

        private void Start()
        {
            selectButton = Utils.Utils.FindChild<Button>(gameObject, "Button");

            selectButton.onClick.AddListener(OnClick);

            selectButton.gameObject.SetActive(false);
        }

        public void SetItem(StartItemType itemType)
        {
            this.itemType = itemType;
            var icon = StartItemIcon.Instance.SearchItem(itemType);

            var item = StartItemIcon.Instance.SearchItems(itemType);

            itemName.SetText(LocalizationManager.Localize(item.LocalizationKey));
            itemDescription.SetText(LocalizationManager.Localize(item.DescriptionKey));
        }

        private void OnClick()
        {
            Debug.Log("ItemSelectorCell OnClick");
            OnItemSelected?.Invoke(itemType);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            selectButton.gameObject.SetActive(true);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            selectButton.gameObject.SetActive(false);
        }
    }
}