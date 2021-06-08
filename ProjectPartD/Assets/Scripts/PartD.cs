using UnityEngine;
using System.Collections;

public class PartD : MonoBehaviour
{
    private ParticleSystem m_System;
    private ParticleSystem.Particle[] particles;

    public Vector3 target;
    public Vector3 dist;

    public Vector3 vel;
    public Vector3 vel_sum;

    public Vector3 alignment;
    public Vector3 cohension;
    public Vector3 separation;

    public float force;
    public int count;
    public float neighbordist;

    void Start()
    {
        m_System = GetComponent<ParticleSystem>();
        var main = m_System.main;

        main.maxParticles = 200;
        main.startLifetime = 10;
        main.startSpeed = 0;

        neighbordist = 50;
    }

    void Update()
    {

        InitializeIfNeeded();
        int numParticles = m_System.GetParticles(particles);
        float t;



        for (int i = 0; i < numParticles; ++i)
        {
            t = Time.time;
            vel_sum = new Vector3(0, 0, 0);

            // When starting and having full lifetime
            if (particles[i].remainingLifetime == 20)
            {
                particles[i].position = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            }


            //Alignment
            vel_sum = new Vector3(0, 0, 0);
            count = 0;


            for (int j = 0; j < numParticles; j++)
            {
                if (j != i)
                {
                    dist = particles[j].position - particles[i].position;

                    if (dist.magnitude < neighbordist)
                    {
                        vel_sum += particles[j].velocity;
                        count += 1;
                    }

                    
                }
            }

	    if (count == 0)
                        vel_sum = new Vector3(0, 0, 0);
                    else
                    {
                        vel_sum /= count;
                        vel_sum = vel_sum.normalized;
                    }
	    }

            alignment = vel_sum;


            // Cohesion
            vel_sum = new Vector3(0, 0, 0);
            count = 0;

            for (int j = 0; j < numParticles; ++j)
            {
                if (j != i)
                {
                    dist = particles[i].position - particles[j].position;

                    if (dist.magnitude < neighbordist)
                    {
                        vel_sum += particles[j].position;
                        count += 1;
                    }

                }
            }
		
                                if (count == 0)
                        vel_sum = new Vector3(0, 0, 0);
                    else
                    {
                        vel_sum /= count;
                        vel_sum -= particles[i].position;
                        vel_sum = vel_sum.normalized;
                    }
                    cohension = vel_sum;
            
	    //Separation
            vel_sum = new Vector3(0, 0, 0);
            count = 0;
            for (int j = 0; j < numParticles; j++)
            {
                if (j != i)
                {

                    dist = particles[i].position - particles[j].position;

                    if (dist.magnitude < neighbordist)
                    {
                        vel_sum += particles[j].position - particles[i].position;
                        count += 1;
                    }


                }
            }
	    

                    if (count == 0)
                        vel_sum = new Vector3(0, 0, 0);
                    else
                    {
                        vel_sum /= count;
                        vel_sum *= -1;
                        vel_sum = vel_sum.normalized;
                    }

                    separation = vel_sum;




            // also adding weights 0.95
            particles[i].velocity += 0.95f * alignment + 0.95f * cohension + 0.95f * separation;

            if (t < 20)
                target = new Vector3(0.0f, -100.0f, 100.0f);

            if (t < 40 && t > 20)
                target = new Vector3(100.0f, 0.0f, 100.0f);

            if (t < 80 && t > 40)
                target = new Vector3(100.0f, -100.0f, 0.0f);

            if (t < 120 && t > 80)
                target = new Vector3(0.0f, -100.0f, 0.0f);


            dist = target - particles[i].position; // vector of velocity
            vel = particles[i].velocity + dist.normalized * t;

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