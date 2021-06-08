using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]

public class PartB : MonoBehaviour
{
    public GameObject sp;

    private ParticleSystem m_System;
    private ParticleSystem.Particle[] particles;

    float g;
    float a;
    float attract_force;
    float cube;

    Vector3 vel;
    Vector3 dist;

    // Start is called before the first frame update
    void Start()
    {
        m_System = GetComponent<ParticleSystem>();


        GameObject sp1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sp1.transform.position = new Vector3(-2.0f, -2.0f, -2.0f);

        GameObject sp2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sp2.transform.position = new Vector3(0.0f, -2.5f, 1.5f);

        var main = m_System.main;
        main.maxParticles = 20;
        main.startSpeed = 1;


        a = 0.95f;
        g = 9.8f;
        cube = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {

        InitializeIfNeeded();
        sp = GameObject.Find("sp");

        var main = m_System.main;
        main.startLifetime = 20;
        main.startSpeed = Random.Range(1, 10);

        int numParticles = m_System.GetParticles(particles);

        for (int i = 0; i < numParticles; i++)
        {
            if (particles[i].remainingLifetime > 19.98f)
            {
                particles[i].velocity = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));
            }


            if (particles[i].position[0] < -cube || particles[i].position[0] > cube)
            {
                vel[0] = -a * particles[i].velocity[0];
                vel[1] = particles[i].velocity[1];
                vel[2] = particles[i].velocity[2];
            }
            else if (particles[i].position[1] < -cube || particles[i].position[1] > cube)
            {
                vel[0] = particles[i].velocity[0];
                vel[1] = -a * particles[i].velocity[1];
                vel[2] = particles[i].velocity[2];
            }

            else if (particles[i].position[2] < -cube || particles[i].position[2] > cube)
            {
                vel[0] = particles[i].velocity[0];
                vel[1] = particles[i].velocity[1];
                vel[2] = -a * particles[i].velocity[2];
            }

            else
            {
                vel = particles[i].velocity;
            }

            if (particles[i].position[0] > 3.3f || particles[i].position[1] > 3.3f || particles[i].position[2] > 3.3f)
                particles[i].remainingLifetime = -1.0f;// destroy

            if (particles[i].position[0] < -3.3f || particles[i].position[1] < -3.3f || particles[i].position[2] < -3.3f)
                particles[i].remainingLifetime = -1.0f;

            vel[1] = vel[1] - g * Time.deltaTime;


            dist = new Vector3(0.0f, -2.5f, 1.5f) - particles[i].position;
            attract_force = 10.0f / (dist.magnitude * dist.magnitude);

            if (dist.magnitude > 3.0f)
                attract_force = 0;

            vel = vel + dist / dist.magnitude * attract_force * Time.deltaTime;

            dist = new Vector3(-2.0f, -2.0f, -2.0f) - particles[i].position;
            attract_force = 100.0f / (dist.magnitude * dist.magnitude);

            if (dist.magnitude > 3.0f)
                attract_force = 0;

            vel = vel + dist / dist.magnitude * attract_force * Time.deltaTime;

            particles[i].velocity = vel;

        }



        m_System.SetParticles(particles, numParticles);
    }

    void InitializeIfNeeded()
    {
        if (m_System == null)
            m_System = GetComponent<ParticleSystem>();

        if (particles == null || particles.Length < m_System.main.maxParticles)
            particles = new ParticleSystem.Particle[m_System.main.maxParticles];
    }
}