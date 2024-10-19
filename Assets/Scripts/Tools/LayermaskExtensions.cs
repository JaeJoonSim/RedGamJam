using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueRiver.Tool
{
    public static class LayermaskExtensions
    {
        public static bool Contains(this LayerMask mask, int layer)
        {
            return ((mask.value & (1 << layer)) > 0);
        }

        public static bool Contains(this LayerMask mask, GameObject gameobject)
        {
            return ((mask.value & (1 << gameobject.layer)) > 0);
        }
    }
}