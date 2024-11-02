using BlueRiver;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreSnowStorm : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.AddColliderSnowParticle(this);
    }
}
