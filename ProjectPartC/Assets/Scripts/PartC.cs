using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]

public class PartC : MonoBehaviour
{

    private ParticleSystem m_System;
    private ParticleSystem.Particle[] parts;


    float g;
    float force;
    public float rad;

    Vector3 vel_prev;
    Vector3 dist;
    Vector3 vel;
    public Vector3 center;



    // Start is called before the first frame update
    void Start()
    {
        m_System = GetComponent<ParticleSystem>();
        var main = m_System.main;
        main.maxParticles = 50;
        //main.maxParticles = 25;
        main.startLifetime = 20f;
        main.startSpeed = 0;

        g = 9.8f;
        rad = 20.0f;

        center = new Vector3(0.0f, 0.0f, 0.0f);
        vel = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        InitializeIfNeeded();

        int nums = m_System.GetParticles(parts);

        for (int i = 0; i < nums; i++)
        {

            if (parts[i].remainingLifetime > 19.99f)

            {
                parts[i].position = new Vector3(5.0f, 0.0f, 0.0f);
            }


            if ((parts[i].position - center).sqrMagnitude > rad * rad)
            {
                vel_prev = parts[i].velocity;
                vel_prev *= 0.9f;

                parts[i].position = center + (parts[i].position - center).normalized * rad;

                parts[i].velocity = Vector3.Reflect(vel_prev, ((parts[i].position / rad) - center));
            }
            else
            {
                parts[i].velocity = parts[i].velocity;
            }

            vel = parts[i].velocity;
            //vel[0] = vel[0] + g * Time.deltaTime;
            //vel[1] = vel[1] + g * Time.deltaTime;
            vel[2] = vel[2] - g * Time.deltaTime;

            parts[i].velocity = vel;

            for (int j = 0; j < nums; j++)
            {
                if (i != j && parts[i].remainingLifetime < 19.99)
                {

                    dist = parts[i].position - parts[j].position;

                    if (dist.magnitude == 0.0f)
                        force = 0;
                    else
                        force = 5.0f / (dist.magnitude * dist.magnitude);

                    vel = parts[i].velocity + dist / dist.magnitude * force * Time.deltaTime;

                    parts[i].velocity = vel;
                }
            }
        }
        m_System.SetParticles(parts, nums);
    }

    void InitializeIfNeeded()
    {
        if (m_System == null)
            m_System = GetComponent<ParticleSystem>();

        if (parts == null || parts.Length < m_System.main.maxParticles)
            parts = new ParticleSystem.Particle[m_System.main.maxParticles];
    }
}
