using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleParticlePlayer : MonoBehaviour
{
    public bool startAwake;
    public ParticleSystem ps;

    private void Start()
    {
        if (startAwake)
            PlayParticle();
    }

    public void PlayParticle()
    {
        if (ps != null)
            ps.Play();

        ps.transform.SetParent(null);

        Destroy(gameObject);
    }
}
