using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlueRiver.Items
{
    public enum StartItemType
    {
        None,
        Lighter,
        TemperatureRecovery,
        SnowStormResist,
        NoWeightPenalty
    }

    public class StartItemIcon : MonoSingleton<StartItemIcon>
    {
        public List<Items> items = new List<Items>();

        public GameObject SearchItem(StartItemType type)
        {
            var icon = items.FirstOrDefault(items => items.itemType == type) ?? items.FirstOrDefault(items => items.itemType == StartItemType.None);

            return icon.icon;
        }

        public Items SearchItems(StartItemType type)
        {
            var icon = items.FirstOrDefault(items => items.itemType == type) ?? items.FirstOrDefault(items => items.itemType == StartItemType.None);

            return icon;
        }

        [Serializable]
        public class Items
        {
            public StartItemType itemType;
            public GameObject icon;
            public string LocalizationKey;
            public string DescriptionKey;
        }
    }

    

}