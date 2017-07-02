using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleObject : MonoBehaviour {
    private int Runtime;
    private ParticleSystem particles;

    void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }

	void Play(int runtime = 1)
    {
        particles.Play();
        ParticleSystem.EmissionModule emit = particles.emission;
        emit.enabled = true;
        Runtime = runtime;
    }

    void Burst()
    {
        Play(1); //For a burst play, all we need is one frame to fire the burst, then it can die off
    }

    void Stop()
    {
        particles.Stop();
        ParticleSystem.EmissionModule emit = particles.emission;
        emit.enabled = false;
        Runtime = 0; //Time to end the particles
    }

    void ChangeColor(Color color)
    {
        ParticleSystem.MainModule main = particles.main;
        main.startColor = color;
    }

    void Update()
    {
        //If we're out of time and the particles aren't playing anymore
        if (Runtime <= 0 && !particles.IsAlive())
            gameObject.SetActive(false);
        else
        {
            if (Runtime == 0) Stop(); //Stop the particle emission. This is here to make sure it only gets called once
            Runtime--;
            
        }
            
        
    }
}
