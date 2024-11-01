using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentPlatform : DefaultPlatform
{
    protected override void BeginEvent()
    {
        player.GetTree().PauseDamageTime(true);
        player.GetTree().RecoverByMaxTemperature();
    }

    protected override void EndEvent()
    {
        player.GetTree().PauseDamageTime(false);
    }
}
