using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTrail : MonoBehaviour {
    private ParticleSystem particles;

    public bool killSwitch = false;

    void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }

	// Use this for initialization
	void OnEnable () {
        particles.Play();
        ParticleSystem.EmissionModule emit = particles.emission;
        emit.enabled = true;
    }
	
    void StopParticles()
    {
        particles.Stop();
        ParticleSystem.EmissionModule emit = particles.emission;
        emit.enabled = false;
        killSwitch = true;
    }

    void Update()
    {
        if (killSwitch)
        {
            if (!particles.IsAlive())
                gameObject.SetActive(false);
        }
    }
	// Update is called once per frame
	void OnDisable () {
        particles.Stop();
        ParticleSystem.EmissionModule emit = particles.emission;
        emit.enabled = false;
    }
}
