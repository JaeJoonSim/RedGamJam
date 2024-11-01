using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueRiver
{
    [CreateAssetMenu(fileName = "SnowStormStats", menuName = "SnowStormStats")]
    public class SnowStormStats : ScriptableObject
    {
        public float Event_Snow_Spd;

        public float Event_Snow_Jump_Spd;

        public float Event_Snow_Move_Spd;

        public float Event_Snow_Damage;

        public float Event_Snow_Time;

        public float Event_Snow_Start;

        public float Event_Snow_Cool;
    }
}