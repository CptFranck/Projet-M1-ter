using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentControl : MonoBehaviour
{
    public GameObject agent;
    
    public int id;
    public int state;
    public int comfortLevel;
    
    public float speed;
    
    public string type;
    
    public Vector3 scene;
    public Vector3 target;
    
    public List<int> contactId;
    public int contactBoxNumber;
    public int oldcontactBoxNumber;
    public int contactCapsuleNumber;

    void Start()
    {
        state = 1;
        comfortLevel = Random.Range(5, 15);
        contactBoxNumber = 0;
        oldcontactBoxNumber = 0;
        contactCapsuleNumber = 0;
        speed = this.GetComponent<NavMeshAgent>().speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (type == "public") 
        {
            agent.GetComponent<NavMeshAgent>().SetDestination(target);
            this.GetComponent<NavMeshAgent>().speed = (float)Speed(contactCapsuleNumber);
        }
    }
    
    public double Speed(double density)
    {
        var x = density;
        return (.9/Mathf.Exp((float)x)+.05)*speed;
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