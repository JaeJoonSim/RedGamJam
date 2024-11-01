using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InDoorPlatform : DefaultPlatform
{
    protected override void BeginEvent()
    {
        player.GetTree().PauseDamageTime(true);
    }

    protected override void EndEvent()
    {
        player.GetTree().PauseDamageTime(false);
    }
}