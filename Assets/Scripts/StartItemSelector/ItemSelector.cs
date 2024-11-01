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
        private int cellCount;

        private void Start()
        {
            for (int i = 0; i < cellCount; i++)
            {
                GameObject go = Instantiate(selectorCell, cellArea);
                go.name = $"Item Select Cell_{i}";
                
                var cell = go.GetComponent<ItemSelectorCell>();
                if (cell != null)
                    cell.SetItem((StartItemType)i + 1);
            }
        }
    }
}