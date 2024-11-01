using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentPlatform : DefaultPlatform
{
    protected override void BeginEvent()
    {
        MyCuteTree.PauseDamageTime(true);
        MyCuteTree.RecoverByMaxTemperature();
    }

    protected override void EndEvent()
    {
        MyCuteTree.PauseDamageTime(false);
    }
}
