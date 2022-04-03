using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleColliderbis : MonoBehaviour
{
    public GameObject agent;

    public string type;
    public CapsuleCollider CapsuleCollider;

    // Start is called before the first frame update
    void Start()
    {
        // initialisation de notre gameobject
        type = "Caps";
        CapsuleCollider.isTrigger = true;
        CapsuleCollider.center = new Vector3(0, 0, 0);
        CapsuleCollider.height = 1.85F;
        CapsuleCollider.radius = 0.22F;
        CapsuleCollider.direction = 1; //Y
    }

    // Les fonctions OnTrigger Enter et Exit font la passe au
    // fontions Trigger Enter et Exit de l'agent auxquels ces
    // scriptes sont associés (architecture necessaire à la
    // différentiation des trigger event)
    private void OnTriggerEnter(Collider collision)
    {
        agent.GetComponent<AgentControl>().TriggerEnter(collision, type);
    }
    private void OnTriggerStay(Collider collision)
    {
        agent.GetComponent<AgentControl>().TriggerEnter(collision, type);
    }
    private void OnTriggerExit(Collider collision)
    {
        agent.GetComponent<AgentControl>().TriggerExit(collision, type);
    }
}
