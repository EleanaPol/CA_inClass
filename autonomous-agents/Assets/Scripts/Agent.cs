using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public int id;
    public Vector3 position;
    public Vector3 velocity;
    public PopulationManager population;
    
    [Header("Speeds")]
    public float speed;
    public float attraction_speed;
    public float separation_speed;

    [Header("Weights")] 
    public float randomness_weight;
    public float attraction_weight;
    public float separation_weight;

    [Header("Flocking")] 
    public float separation_threshold;
    
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
        
        return  normalized_new_position * speed ;
    }

    public Vector3 Attract()
    {
        var attractor_pos = population.attractor.position;
        var attraction_vector = attractor_pos - position;
        //var normalized_attraction = Vector3.Normalize(attraction_vector);
        return attraction_vector * attraction_speed;
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
            return (separation / separation_counter) * separation_speed;
        }

    }

    public void AddForce(Vector3 force, float weight)
    {
        transform.position += force * weight * Time.deltaTime;
    }

    public void MoveAgent()
    {
        // move randomly
        Vector3 random = RandomMove();
        AddForce(random,randomness_weight);
        
        // attract
        Vector3 attraction = Attract();
        AddForce(attraction,attraction_weight);
        
        //separate
        Vector3 separation = Separate();
        AddForce(separation,separation_weight);
        
        // make sure to update position property
        position = transform.position;
    }

    public void UpdateValues()
    {
        speed = population.speed;
        attraction_speed = population.attraction_speed;
        separation_speed = population.separation_speed;

        randomness_weight = population.randomness_weight;
        attraction_weight = population.attraction_weight;
        separation_weight = population.separation_weight;

        separation_threshold = population.separation_threshold;
    }
}
