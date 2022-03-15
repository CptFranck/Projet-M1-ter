using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentControl : MonoBehaviour
{
    public GameObject agent;                // Attribut correspondant au prefab correspondant à l'agent

    public int id;                          // Attributs correspondant aux caractéristiques générales de l'agent
    public int state;
    //public int weight;
    //public bool priorityChanged;

    public float speed;
    //public float cooldown;
    //public float countdown;
    //public float distanceTarget;
    //public float oldDistanceTarget;

    public string type;
    
    public Vector3 scene;                   // Attributs correspondant aux points 3d utilisés pour l'orientation de l'agent
    public Vector3 target;
    public Vector3 oldTarget;

    public List<int> contactId;             // Attributs correspondant aux contacts de l'agent (list des contactes et les colliders propres à l'agent)
    public List<int> areaId;
    public int contactBoxNumber;
    public int contactCapsuleNumber;

    void Start()
    {
        // initialisation des attributs généraux de l'agent (les points 3D étant initialisé par l'objet AgentSpawner
        state = 1;
        //weight = 0;
        //cooldown = 0;
        //countdown = 0;
        //oldDistanceTarget = 0;
        //priorityChanged = false;
        speed = 3.5F;
        contactBoxNumber = 0;
        contactCapsuleNumber = 0;
        agent.GetComponent<NavMeshAgent>().speed = speed;
        agent.GetComponent<NavMeshAgent>().avoidancePriority = 50; //Random.Range(50, 60);
    }

    void Update()
    {
        if (type == "public")           // Si l'agent fait partie du public alors :
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
            
            if(state == 1)              // Si l'état de l'agent correspond à rester dans la salle alors :
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
            else                        // So l'agent souhaite sortir alors :
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

            // Modification de la vitesse en temps réel
            agent.GetComponent<NavMeshAgent>().speed = (float)Speed(contactCapsuleNumber /*+ weight*/);

            if (oldTarget != target)    // Si l'objectif de l'agent a été modifié alors : on met à jour sa destination
            {
                agent.GetComponent<NavMeshAgent>().SetDestination(target); 
            }
            oldTarget = target;

            //agent.GetComponent<NavMeshAgent>().speed += .5F;
            //agent.GetComponent<NavMeshAgent>().radius = (float)(1.28*Mathf.Exp(distanceTarget - 10) + 0.22);
        }
    }

    // Speed cherche à renvoyer une vitesse que doit avoir un agent en fonction du
    // nombre de contact direct qu'il a avec d'autres agents
    public double Speed(double density)
    {
        var x = density;
        return ((.95/Mathf.Exp((float)x))+.05)*speed;
        // 0.95/exp(x) + 0.05 �volution de la vitesse en m/s
        // Au dela de 5 contacts, la vitesse stagne à 0.2 m/s
    }

    // TriggerEnter correspond � la r�action d'un agent lors d'un contact avec un
    // gameObeject, ici son but et de detecter la sortie si son �tant est 0, ou de
    // detecter un autre agent et tout de suite apr�s l'enregistrer afin de ne pas
    // comptabiliser plusieur fois le m�me agents alors qu'il est tjrs en contact,
    // le tout en mettant � jours le nombre de contact direct et indirect des agents
    // entre eux
    
    public void TriggerEnter(Collider collision, string type)
    {
        if (collision.gameObject.tag == "Spawner" && state == 0)
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Agent")
        {
            int idbis = collision.gameObject.GetComponentInParent<AgentControl>().id; 
            if (idbis != id)
            {
                if (type == "Box" && collision.GetType() == typeof(CapsuleCollider))
                {
                    areaId.Add(idbis);
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

    // TriggerExit correspond � la r�action d'un agent lors d'un contact "de sortie"
    // avec un gameObeject, ici son but et de detecter la sortie d'un autre agent
    // d�j� enregistrer afin de pouvoir le recomptabiliser si jamais il revenait au
    // contact de cet agent ayant d�j� quitter le collider le tout en mettant � jours
    // le nombre de contact direct et indirect des agents entre eux
    public void TriggerExit(Collider collision, string type)
    {
        if (collision.gameObject.tag == "Agent")
        {
            int idbis = collision.gameObject.GetComponentInParent<AgentControl>().id; //.GetComponent<AgentControl>().id;

            if (idbis != id)
            {
                if (type == "Box" && collision.GetType() == typeof(CapsuleCollider))
                {
                    areaId.RemoveAt(areaId.IndexOf(idbis));
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