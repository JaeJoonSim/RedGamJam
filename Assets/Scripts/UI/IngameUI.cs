using BlueRiver.Character;
using BlueRiver.Items;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlueRiver.UI
{
    public class IngameUI : MonoBehaviour
    {
        private PlayerController player;

        [SerializeField]
        private Image tempFillImage;

        [SerializeField]
        private TextMeshProUGUI tempText;

        [SerializeField]
        private Transform itemIconArea;

        private GameObject itemIcon;

        private void Start()
        {
            player = GameManager.Instance.player;
            tempFillImage.fillAmount = 1;
        }

        private void Update()
        {
            if (player.GetTree() != null)
            {
                tempFillImage.fillAmount = player.GetTree().GetTemperature();
                tempText.SetText($"{(int)player.GetTree().GetTemperature()}");
            }

            if (GameManager.Instance.startItemType != Items.StartItemType.None)
            {
                if (itemIcon == null)
                {
                    var itemIconObj = StartItemIcon.Instance.SearchItem(GameManager.Instance.startItemType);
                    if (itemIconObj != null && itemIcon == null)
                        itemIcon = Instantiate(itemIconObj, itemIconArea);
                }
            }
            else
            {
                if (itemIcon != null)
                    Destroy(itemIcon);
                itemIcon = null;
            }
        }
    }
}