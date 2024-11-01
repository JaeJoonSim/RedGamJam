using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InDoorPlatform : DefaultPlatform
{
    protected override void BeginEvent()
    {
        MyCuteTree.PauseDamageTime(true);
    }

    protected override void EndEvent()
    {
        MyCuteTree.PauseDamageTime(false);
    }
}