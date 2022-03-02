using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentControl : MonoBehaviour
{
    public int id;
    public int state;
    public float speed;
    public string type;
    public Vector3 target;
    public GameObject agent;
    
    public List<int> contactId;
    public int contactBoxNumber;
    public int contactCapsuleNumber;
    public BoxCollider BoxCollider;
    public CapsuleCollider CapsuleCollider;

    int nbCollisions;

    void Start()
    {
        nbCollisions = 0;
        contactBoxNumber = 0;
        contactCapsuleNumber = 0;
        state = 1;
        speed = this.GetComponent<NavMeshAgent>().speed;
    }
        // Update is called once per frame
        void Update()
    {
        Debug.Log(nbCollisions);
        if (type == "public") 
        {
            //\left(-1/(1+\exp(-x\cdot2.5+4.5))\right)+1.2
            agent.GetComponent<NavMeshAgent>().SetDestination(target);
            this.GetComponent<NavMeshAgent>().speed = (float)Speed(contactCapsuleNumber);
        }

    }
    
    public double Speed(double density)
    {
        var x = density;
        /*if(x > 10)
        {
            return speed;
        }*/
        return (.9/Mathf.Exp((float)x)+.05)*speed;
        //var x = -density / 2.5;
        //return (-1/ (1 + (Mathf.Exp((float)x) + 4.5)))+1.2;
    }

    private void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.tag == "Spawner" && state == 0)
        {
            Debug.Log("MuffinTime");
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Agent")
        {
            int idbis = collision.gameObject.GetComponent<AgentControl>().id;

            if (collision is BoxCollider)
            {
                //Debug.Log(id + " -> box " + idbis);
                contactBoxNumber++;

            }
            if (!contactId.Contains(idbis))
            {
                if (collision is CapsuleCollider)
                {
                    //Debug.Log(id + " -> capsule " + idbis);
                    contactId.Add(idbis);
                    contactCapsuleNumber++;
                }
            }
            nbCollisions++;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Agent")
        {
            /*if (collision is BoxCollider)
            {
                contactBoxNumber--;

            }*/

            int idbis = collision.gameObject.GetComponent<AgentControl>().id;
            contactId.RemoveAt(contactId.IndexOf(idbis));
            contactCapsuleNumber--;
            
            /*if (contactId.Contains(idbis))
            {         
                if (collision is CapsuleCollider)
                {
                    

                }
            }*/
            nbCollisions--;
        }
    }

    public int getNbCollisions(){
        return nbCollisions;
    }
}