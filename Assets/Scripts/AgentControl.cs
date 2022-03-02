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
        speed = this.GetComponent<NavMeshAgent>().speed;
        agent.GetComponent<NavMeshAgent>().avoidancePriority = Random.Range(1,100);
    }
    
    void Update()
    {
        if (type == "public") 
        {
            //left(-1/(1+\exp(-x\cdot2.5+4.5))\right)+1.2


            /*if (contactBoxNumber > comfortLevel)
            {
                target = new Vector3(agent.GetComponent<NavMeshAgent>().transform.position.x,
                                     agent.GetComponent<NavMeshAgent>().transform.position.y,
                          -Mathf.Abs(agent.GetComponent<NavMeshAgent>().transform.position.z - 10));
                this.GetComponent<NavMeshAgent>().speed = 1;
            }
            if (contactBoxNumber == comfortLevel)
            {
                target = agent.GetComponent<NavMeshAgent>().transform.position;
            }
            if (contactBoxNumber < oldcontactBoxNumber)
            {
                target = this.scene;
            }*/
            //distanceTarget = Vector3.Distance(agent.GetComponent<NavMeshAgent>().transform.position,target);
            //agent.GetComponent<NavMeshAgent>().radius = (float)(1.28*Mathf.Exp(distanceTarget - 10) + 0.22);
            agent.GetComponent<NavMeshAgent>().SetDestination(target);
            this.GetComponent<NavMeshAgent>().speed = (float)Speed(contactCapsuleNumber);
            //oldcontactBoxNumber = contactBoxNumber;

        }
    }
    
    public double Speed(double density)
    {
        var x = density;
        return ((.95/Mathf.Exp((float)x))+.05)*speed;
        // 0.95/exp(x) + 0.05 �volution de la vitesse en m/s
        // Au dela de 5 contacts, la vitesse stagne � 0.2 m/s
        // 
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