using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutDoorPlatform : DefaultPlatform
{
    //0.00 ~ 1.00
    [SerializeField] float ReduceDamagePercent;

    protected override void BeginEvent()
    {
        base.BeginEvent();
        MyCuteTree.SetOutDoor(true);
        MyCuteTree.SetReduceDamagePercent(ReduceDamagePercent);
    }

    protected override void EndEvent()
    {
        MyCuteTree.SetOutDoor(false);
    }
}
