using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentControl : MonoBehaviour
{
    public GameObject agent;

    public int id; 
    public int state;
    public int weight;
    public bool priorityChanged;

    public float speed;
    public float cooldown;
    public float countdown;
    public float distanceTarget;
    public float oldDistanceTarget;

    public string type;
    
    public Vector3 scene;
    public Vector3 target;
    public Vector3 oldTarget;

    public List<int> contactId;
    public int contactBoxNumber;
    public int contactCapsuleNumber;

    void Start()
    {
        state = 1;
        weight = 0;
        cooldown = 0;
        countdown = 0;
        oldDistanceTarget = 0;
        priorityChanged = false;
        contactBoxNumber = 0;
        contactCapsuleNumber = 0;
        speed = 3.5F;
        agent.GetComponent<NavMeshAgent>().speed = speed;
        agent.GetComponent<NavMeshAgent>().avoidancePriority = 50; //Random.Range(50, 60);
    }

    void Update()
    {
        if (type == "public") 
        {
            
            /*if(agent.GetComponent<NavMeshAgent>().stoppingDistance > 1 && distanceTarget > agent.GetComponent<NavMeshAgent>().stoppingDistance &&
               agent.GetComponent<NavMeshAgent>().stoppingDistance < 6)
            {
                agent.GetComponent<NavMeshAgent>().avoidancePriority = 45;
            } 
            else
            {
                agent.GetComponent<NavMeshAgent>().avoidancePriority = 50;
            }*/
            
            if(state == 1)
            {
                //distanceTarget = Vector3.Distance(agent.GetComponent<NavMeshAgent>().transform.position, target);
                /*if (countdown != -1)
                {
                    if (countdown == 5){
                        
                        if (Mathf.Abs(oldDistanceTarget - distanceTarget) < 1)
                        {
                            countdown = 0;
                            target = agent.GetComponent<NavMeshAgent>().transform.position;
                            //agent.GetComponent<NavMeshAgent>().isStopped = true;
                            agent.GetComponent<NavMeshAgent>().avoidancePriority = 55;
                        }
                        else
                        {
                            countdown = 0;
                            oldDistanceTarget = distanceTarget;
                        }
                    }
                    else
                    {
                        cooldown += Time.deltaTime;
                        if(cooldown >= 1f)
                        {
                            cooldown = 0;
                            countdown++;
                            //Debug.Log("countdown : " + countdown + "s");
                        }
                        
                    }
                }*/
            } 
            else
            {
                //agent.GetComponent<NavMeshAgent>().isStopped = false;
                //agent.GetComponent<NavMeshAgent>().avoidancePriority = 50;
            }
            /*
            if (contactBoxNumber > 8)
            {
                weight = 1;
            }
            else
            {
                weight = 0;
            }*/
            agent.GetComponent<NavMeshAgent>().speed = (float)Speed(contactCapsuleNumber + weight);

            if (oldTarget != target)
            {
                agent.GetComponent<NavMeshAgent>().SetDestination(target);
            }
            oldTarget = target;
            //agent.GetComponent<NavMeshAgent>().speed += .5F;
            //agent.GetComponent<NavMeshAgent>().radius = (float)(1.28*Mathf.Exp(distanceTarget - 10) + 0.22);
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