using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    [Header("Population Settings")] 
    public int numAgents;
    public GameObject agent_obj;
    public Vector2 x_range;
    public Vector2 z_range;
    public List<Agent> agentPopulation;

    [Header("Agent Settings")] 
    [Header("Speeds")]
    public float global_speed;
    public float speed;
    public float attraction_speed;
    // -------- flocking speeds
    public float separation_speed;
    public float cohesion_speed;
    public float alignment_speed;

    [Header("Weights")] 
    public float randomness_weight;
    public float attraction_weight;
    // -------- flocking weights
    public float separation_weight;
    public float cohesion_weight;
    public float alignment_weight;

    [Header("External Elements")] 
    public Transform attractor;
    
    [Header("Flocking Thresholds")] 
    public float separation_threshold;
    public float cohesion_threshold;
    public float alignment_threshold;

    public Material trail_mat;
    public Gradient trail_color;
    
    // Start is called before the first frame update
    void Start()
    {
        InitializeAgentPopulation();
    }

    // Update is called once per frame
    void Update()
    {
        // first calculate agent forces
        for (int i = 0; i < numAgents; i++)
        {
            var agent = agentPopulation[i];
            agent.UpdateValues();
            agent.CalculateForces();
        }

        // then move agent
        for (int i = 0; i < numAgents; i++)
        {
            var agent = agentPopulation[i];
            agent.MoveAgent();
        }
    }

    public void InitializeAgentPopulation()
    {
        agentPopulation = new List<Agent>();
        
        for (int i = 0; i < numAgents; i++)
        {
            // init agent object
            var new_agent = Instantiate(agent_obj, transform);
            // random position for each agent
            float x = Random.Range(x_range.x, x_range.y);
            float z = Random.Range(z_range.x, z_range.y);

            float xv = Random.Range(-x_range.y, x_range.y);
            float zv = Random.Range(-z_range.y, z_range.y);
            
            Vector3 new_pos = new Vector3(x,0,z);
            Vector3 new_velocity = new Vector3(xv,0,zv);
            
            new_agent.transform.position = new_pos;
            new_agent.name = "agent_" + i.ToString();
            
            // attach agent functionality
            new_agent.AddComponent<Agent>();
            var agent_f = new_agent.GetComponent<Agent>();
            agentPopulation.Add(agent_f);
            agent_f.id = i;
            agent_f.position = new_pos;
            agent_f.last_position = new_pos;
            agent_f.velocity = new_velocity;
            agent_f.population = this;

            // SPEED
            agent_f.global_speed = global_speed;
            agent_f.speed = speed;
            agent_f.attraction_speed = attraction_speed;
            // ----- flocking speeds
            agent_f.separation_speed = separation_speed;
            agent_f.cohesion_speed = cohesion_speed;
            agent_f.alignment_speed = alignment_speed;

            // WEIGHT
            agent_f.randomness_weight = randomness_weight;
            agent_f.attraction_weight = attraction_weight;
            // ----- flocking weights
            agent_f.separation_weight = separation_weight;
            agent_f.cohesion_weight = cohesion_weight;
            agent_f.alignment_weight = alignment_weight;

            // DISTANCE THRESHOLDS
            // ----- flocking thresholds
            agent_f.separation_threshold = separation_threshold;
            agent_f.cohesion_threshold = cohesion_threshold;
            agent_f.alignment_threshold = alignment_threshold;


            new_agent.AddComponent<TrailRenderer>();
            var trail = new_agent.GetComponent<TrailRenderer>();
            trail.startWidth = 0.05f;
            trail.endWidth = 0.05f;
            trail.time = 35;
            trail.material = trail_mat;
            trail.colorGradient = trail_color;
        }
    }
}
