using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public int id;
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 last_position;
    public PopulationManager population;

    [Header("Speeds")] 
    public float global_speed;
    public float speed;
    public float attraction_speed;
    // ------- flocking speeds -----
    public float separation_speed;
    public float cohesion_speed;
    public float alignment_speed;

    [Header("Weights")] 
    public float randomness_weight;
    public float attraction_weight;
    // ------- flocking weights -----
    public float separation_weight;
    public float cohesion_weight;
    public float alignment_weight;

    // ------- flocking distance thresholds -----
    [Header("Flocking")] 
    public float separation_threshold;
    public float cohesion_threshold;
    public float alignment_threshold;
    
    // ------- MOVEMENT FORCES ------
    private Vector3 random_force;
    private Vector3 attraction_force;
    // ------- flocking ---------
    private Vector3 separation_force;
    private Vector3 cohesion_force;
    private Vector3 alignment_force;
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateValues();
    }

    public Vector3 RandomMove()
    {
        float x = Random.Range(-1.0f, 1.0f);
        float z = Random.Range(-1.0f, 1.0f);

        Vector3 existing_position = transform.position;
        Vector3 new_position = new Vector3(x,0,z);
        Vector3 normalized_new_position = Vector3.Normalize(new_position);
        
        return  normalized_new_position * speed * global_speed ;
    }

    public Vector3 Attract()
    {
        var attractor_pos = population.attractor.position;
        var attraction_vector = attractor_pos - position;
        //var normalized_attraction = Vector3.Normalize(attraction_vector);
        return attraction_vector * attraction_speed * global_speed;
    }

    public Vector3 Separate()
    {
        Vector3 separation = Vector3.zero; // new Vector3(0,0,0)
        int separation_counter = 0;

        int numAgents = population.numAgents;
        var agentPopulation = population.agentPopulation;
        
        for (int i = 0; i < numAgents; i++)
        {
            Vector3 diff = position - agentPopulation[i].position;
            float dist = diff.magnitude;

            if (dist < separation_threshold && dist > 0)
            {
                diff = Vector3.Normalize(diff);
                separation += diff;
                separation_counter++;
            }
        }
        
        if (separation_counter == 0)
        {
            return Vector3.zero;
        }
        else
        {
            return (separation / separation_counter) * separation_speed *global_speed;
        }

    }

    public Vector3 Cohesion()
    {
        Vector3 cohesion = Vector3.zero;
        int cohesion_counter = 0;
        
        int numAgents = population.numAgents;
        var agentPopulation = population.agentPopulation;

        for (int i = 0; i < numAgents; i++)
        {
            Vector3 diff = agentPopulation[i].position - position;
            float dist = diff.magnitude;

            if (dist > 0 && dist <= cohesion_threshold)
            {
                //diff = Vector3.Normalize(diff);
                cohesion += diff;
                cohesion_counter++;
            }
        }

        if (cohesion_counter > 0)
        {
            cohesion = (cohesion / cohesion_counter) * cohesion_speed *global_speed;
        }
        
        return cohesion;
    }

    public Vector3 Align()
    {
        Vector3 alignment = Vector3.zero;
        int alignment_counter = 0;
        
        int numAgents = population.numAgents;
        var agentPopulation = population.agentPopulation;

        for (int i = 0; i < numAgents; i++)
        {
            Vector3 diff = position - agentPopulation[i].position;
            float dist = diff.magnitude;

            if (dist > 0 && dist <= alignment_threshold)
            {
                Vector3 vel = agentPopulation[i].velocity;
                alignment += vel;
                alignment_counter++;
            }
        }

        if (alignment_counter > 0)
        {
            alignment = (alignment / alignment_counter) * alignment_speed *global_speed;
        }

        return alignment;
    }

    public void AddForce(Vector3 force, float weight)
    {
        transform.position += force * weight * Time.deltaTime;
    }

    public void CalculateForces()
    {
        random_force = RandomMove();
        attraction_force = Attract();
        
        // ---- flocking forces
        separation_force = Separate();
        cohesion_force = Cohesion();
        alignment_force = Align();
    }

    public void MoveAgent()
    {
        // move randomly
        AddForce(random_force,randomness_weight);
        // attract
        AddForce(attraction_force,attraction_weight);
        
        // ----- flocking
        AddForce(separation_force,separation_weight);
        AddForce(cohesion_force,cohesion_weight);
        AddForce(alignment_force,alignment_weight);
        
        // make sure to update position property
        position = transform.position;
        
        UpdateVelocity();
    }

    public void UpdateVelocity()
    {
        velocity = position - last_position;
        last_position = position;
    }

    public void UpdateValues()
    {
        global_speed = population.global_speed;
        speed = population.speed;
        attraction_speed = population.attraction_speed;
        // ---- flocking speeds
        separation_speed = population.separation_speed;
        cohesion_speed = population.cohesion_speed;
        alignment_speed = population.alignment_speed;

        randomness_weight = population.randomness_weight;
        attraction_weight = population.attraction_weight;
        // ----- flocking weights
        separation_weight = population.separation_weight;
        cohesion_weight = population.cohesion_weight;
        alignment_weight = population.alignment_weight;

        // ----- flocking thresholds
        separation_threshold = population.separation_threshold;
        cohesion_threshold = population.cohesion_threshold;
        alignment_threshold = population.alignment_threshold;
    }
}
