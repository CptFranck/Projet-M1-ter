using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentControl : MonoBehaviour
{
    public GameObject agent;

    public int id; 
    public int state;

    public float speed;
    public float distanceTarget;

    public string type;
    
    public Vector3 scene;
    public Vector3 target;
    
    public List<int> contactId;
    public int contactBoxNumber;
    public int contactCapsuleNumber;

    void Start()
    {
        state = 1;
        contactBoxNumber = 0;
        contactCapsuleNumber = 0;
        speed = 3.5F;
        this.GetComponent<NavMeshAgent>().speed = speed;
        //agent.GetComponent<NavMeshAgent>().avoidancePriority = Random.Range(50,65);
    }

    void Update()
    {
        if (type == "public") 
        {
            //distanceTarget = Vector3.Distance(agent.GetComponent<NavMeshAgent>().transform.position,target);
            //agent.GetComponent<NavMeshAgent>().radius = (float)(1.28*Mathf.Exp(distanceTarget - 10) + 0.22);
            agent.GetComponent<NavMeshAgent>().SetDestination(target);
            this.GetComponent<NavMeshAgent>().speed = (float)Speed(contactCapsuleNumber);

        }
    }
    public double Speed(double density)
    {
        var x = density;
        return ((.95/Mathf.Exp((float)x))+.05)*speed;
        // 0.95/exp(x) + 0.05 évolution de la vitesse en m/s
        // Au dela de 5 contacts, la vitesse stagne à 0.2 m/s
    }

    public void TriggerEnter(Collider collision, string type)
    {
        if (collision.gameObject.tag == "Spawner" && state == 0)
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Agent")
        {
            int idbis = collision.gameObject.GetComponentInParent<AgentControl>().id; //GetComponent<AgentControl>().id;

            if (idbis != id)
            {
                if (type == "Box" && collision.GetType() == typeof(CapsuleCollider))
                {
                    contactBoxNumber++;
                }
                if (type == "Caps" && collision.GetType() == typeof(CapsuleCollider))
                {
                    if (!contactId.Contains(idbis))
                    {
                        contactId.Add(idbis);
                        contactCapsuleNumber++;
                    }
                }
            }
        }
    }
    public void TriggerExit(Collider collision, string type)
    {
        if (collision.gameObject.tag == "Agent")
        {
            int idbis = collision.gameObject.GetComponentInParent<AgentControl>().id; //.GetComponent<AgentControl>().id;

            if (idbis != id)
            {
                if (type == "Box" && collision.GetType() == typeof(CapsuleCollider))
                {
                    contactBoxNumber--;
                }
                if (type == "Caps" && collision.GetType() == typeof(CapsuleCollider))
                {
                    if (contactId.Contains(idbis))
                    {
                        contactId.RemoveAt(contactId.IndexOf(idbis));
                        contactCapsuleNumber--;
                    }
                }
            }
        }
    }
}