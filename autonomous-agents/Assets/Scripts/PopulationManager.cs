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
    public float speed;
    public float attraction_speed;
    public float separation_speed;

    [Header("Weights")] 
    public float randomness_weight;
    public float attraction_weight;
    public float separation_weight;

    [Header("External Elements")] 
    public Transform attractor;
    
    [Header("Flocking")] 
    public float separation_threshold;
    
    // Start is called before the first frame update
    void Start()
    {
        InitializeAgentPopulation();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < numAgents; i++)
        {
            var agent = agentPopulation[i];
            agent.UpdateValues();
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
            Vector3 new_pos = new Vector3(x,0,z);
            new_agent.transform.position = new_pos;
            new_agent.name = "agent_" + i.ToString();
            
            // attach agent functionality
            new_agent.AddComponent<Agent>();
            var agent_f = new_agent.GetComponent<Agent>();
            agentPopulation.Add(agent_f);
            agent_f.id = i;
            agent_f.position = new_pos;
            agent_f.population = this;

            agent_f.speed = speed;
            agent_f.attraction_speed = attraction_speed;
            agent_f.separation_speed = separation_speed;

            agent_f.randomness_weight = randomness_weight;
            agent_f.attraction_weight = attraction_weight;
            agent_f.separation_weight = separation_weight;

            agent_f.separation_threshold = separation_threshold;

        }
    }
}
