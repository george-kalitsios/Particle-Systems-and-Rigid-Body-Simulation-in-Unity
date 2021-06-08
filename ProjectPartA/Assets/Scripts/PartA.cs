using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]

public class PartA : MonoBehaviour
{
    private ParticleSystem m_System;
    private ParticleSystem.Particle[] m_particles;
    float g; // gravity 
    float a; // factor
    float cube;
    Vector3 vel;

    // Start is called before the first frame update
    void Start()
    {
        m_System = GetComponent<ParticleSystem>();
        var main = m_System.main;
        main.maxParticles = 30;
        main.startSpeed = 1;
        a = 0.98f;
        g = 9.8f;
        cube = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        InitializeIfNeeded();

        var main = m_System.main;
        main.startLifetime = 15;
        main.startSpeed = UnityEngine.Random.Range(1, 10);

        int num_m_particles = m_System.GetParticles(m_particles);

        for (int i = 0; i < num_m_particles; ++i)
        {

            vel = m_particles[i].velocity;

            if (m_particles[i].position[0] < -cube || m_particles[i].position[0] > cube)
            {
                vel[0] = -a * m_particles[i].velocity[0];
                vel[1] = m_particles[i].velocity[1];
                vel[2] = m_particles[i].velocity[2];
            }
            else if (m_particles[i].position[1] < -cube || m_particles[i].position[1] > cube)
            {
                vel[0] = m_particles[i].velocity[0];
                vel[1] = -a * m_particles[i].velocity[1];
                vel[2] = m_particles[i].velocity[2];
            }

            else if (m_particles[i].position[2] < -cube || m_particles[i].position[2] > cube)
            {
                vel[0] = m_particles[i].velocity[0];
                vel[1] = m_particles[i].velocity[1];
                vel[2] = -a * m_particles[i].velocity[2];
            }

            if (m_particles[i].position[0] > (cube + 0.3f) || m_particles[i].position[1] > (cube + 0.3f) || m_particles[i].position[2] > (cube + 0.3f))
            {
                m_particles[i].remainingLifetime = -2.0f; // destroy this particle, lifetime below zero
            }

            if (m_particles[i].position[0] < (-cube - 0.3f) || m_particles[i].position[1] < (-cube - 0.3f) || m_particles[i].position[2] < (-cube - 0.3f))
            {
                m_particles[i].remainingLifetime = -2.0f;
            }

            //gravity only in direction y.
            vel[1] = vel[1] - g * Time.deltaTime;

            // assign new velocity vector
            m_particles[i].velocity = new Vector3(vel[0], vel[1], vel[2]);

        }
        m_System.SetParticles(m_particles, num_m_particles);
    }

    void InitializeIfNeeded()
    {
        if (m_System == null)
            m_System = GetComponent<ParticleSystem>();

        if (m_particles == null || m_particles.Length < m_System.main.maxParticles)
            m_particles = new ParticleSystem.Particle[m_System.main.maxParticles];
    }
}
