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
    public string type;
    public Vector3 target;
    
    public List<int> contactId;
    public int contactBoxNumber;
    public int contactCapsuleNumber;

    void Start()
    {
        contactBoxNumber = 0;
        contactCapsuleNumber = 0;
        state = 1;
        speed = this.GetComponent<NavMeshAgent>().speed;
    }
        // Update is called once per frame
        void Update()
    {

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
        return (.9/Mathf.Exp((float)x)+.05)*speed;
        //var x = -density / 2.5;
        //return (-1/ (1 + (Mathf.Exp((float)x) + 4.5)))+1.2;
    }

    public void TriggerEnter(Collider collision, string type)
    {
        
        if (collision.gameObject.tag == "Spawner" && state == 0)
        {
            //Debug.Log("MuffinTime");
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Agent")
        {
            int idbis = collision.gameObject.GetComponent<AgentControl>().id;

            if (type == "Box" /*&& collision.gameObject.GetComponent<BoxColliderbis>().type == "Box"*/)
            {
                //Debug.Log(" Box de " + id + " a hit capsule de " + idbis);
                contactBoxNumber++;

            } else if (type == "Caps" /*&& collision.gameObject.GetComponent<CapsuleColliderbis>().type == "Caps"*/)
            {
                if (!contactId.Contains(idbis))
                {
                    //Debug.Log(" Capsule de " + id + " a hit capsule de " + idbis);
                    contactId.Add(idbis);
                    contactCapsuleNumber++;
                    
                }
            }
        }
    }
    public void TriggerExit(Collider collision, string type)
    {
        if (collision.gameObject.tag == "Agent")
        {
            
            int idbis = collision.gameObject.GetComponent<AgentControl>().id;

            if (type == "Box" /*&& collision.gameObject.GetComponent<BoxColliderbis>().type == "Box"*/)
            {
                //Debug.Log(" box de " + id + " a hit " + idbis);
                contactBoxNumber--;

            }
            else if (type == "Caps" /*&& collision.gameObject.GetComponent<CapsuleColliderbis>().type == "Caps"*/)
            {
                
                if (contactId.Contains(idbis))
                {
                    //Debug.Log(" capsule de " + id + " a hit " + idbis);
                    contactId.RemoveAt(contactId.IndexOf(idbis));
                    contactCapsuleNumber--;
                }
            }
        }
    }
}