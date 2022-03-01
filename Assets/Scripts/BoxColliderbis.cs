using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderbis : MonoBehaviour
{
    public GameObject agent;

    public string type;
    public BoxCollider BoxCollider;

    // Start is called before the first frame update
    void Start()
    {
        type = "Box";
        BoxCollider.isTrigger = true;
        BoxCollider.center = new Vector3(0,0,0);
        BoxCollider.size = new Vector3(1, 2, 1);
    }
    private void OnTriggerEnter(Collider collision)
    {
        agent.GetComponent<AgentControl>().TriggerEnter(collision, type);
    }

    private void OnTriggerExit(Collider collision)
    {
        agent.GetComponent<AgentControl>().TriggerExit(collision, type);
    }
}
