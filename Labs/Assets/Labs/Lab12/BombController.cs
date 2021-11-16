using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : Movement
{
    public Material particle;
    private ParticleSystem particleSystem;

    private ParticleSystem.Burst burst;
    private AudioSource audioSource;
    private float delayTime;
    private float interval;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        particleSystem = gameObject.GetComponent<ParticleSystem>();
        if(particleSystem == null)
        {
            Debug.LogError("No Particle System found!");
        }
        audioSource = gameObject.GetComponent<AudioSource>();
        if(audioSource == null)
        {
            Debug.LogError("No Audio Source found!");
        }

        // since explosion audio to particle system burst
        burst = particleSystem.emission.GetBurst(0);
        delayTime = burst.time;
        interval = burst.repeatInterval;

        InvokeRepeating("playSound", delayTime, interval);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    private void playSound()
    {
        audioSource.Play();
    }
}
