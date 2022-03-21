using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentControl : MonoBehaviour
{
    public AgentControl singer;

    public int id;                          // Attributs correspondant aux caractéristiques générales de l'agent
    public int state;
    //public bool priorityChanged;
    public float speed;
    public float cooldown;
    //public float countdown;

    public float distanceScene;             // Attributs correspondant distence liées aux agents
    public float distanceSinger;
    public float oldDistanceStop;
    public bool distanceStopSaved;

    public string type;                     // Attributs correspondant aux rôles des agents et leurs spécificités
    public LayerMask whatCanBeClickOn;

    public Vector3 scene;                   // Attributs correspondant aux points 3d utilisés pour l'orientation de l'agent
    public Vector3 target;
    public Vector3 oldTarget;

    public List<int> areaId;                // Attributs correspondant aux contacts de l'agent (list des contactes et les colliders propres à l'agent)
    public List<int> contactId; 
    public int contactBoxNumber;
    public int contactCapsuleNumber;

    void Start()
    {
        // initialisation des attributs généraux de l'agent (les points 3D étant initialisés par l'objet AgentSpawner
        //                      non intencié cat Start est appelé après Instantiate() dans AgentSpawner
        // singer = new AgentControl;
        // id                   instantié dans AgentSpawner
        state = 1;     
        // priorityChanged = false;

        speed = 3.5F;
        cooldown = 0;
        // countdown = 0;       brouillon
        distanceScene = 0;
        distanceSinger = 0;
        oldDistanceStop = 0;
        distanceStopSaved = true;

        // type                 instantié dans AgentSpawner

        //                      non intencié cat Start est appelé après Instantiate() dans AgentSpawner
        //whatCanBeClickOn = new LayerMask();
        //scene = new Vector3();

        //target = new Vector3();       brouillon
        //oldTarget = new Vector3();    brouillon

        areaId = new List<int>();
        contactId = new List<int>();
        contactBoxNumber = 0;
        contactCapsuleNumber = 0;
        
        this.GetComponent<NavMeshAgent>().speed = speed;
        this.GetComponent<NavMeshAgent>().avoidancePriority = 50; //Random.Range(50, 60);
    }

    void Update()
    {
        if (type == "singer")               // Si l'agent est le chanteur alors :
        {
            if (state == 1)                 // Si l'état de l'agent correspond à celui d'être actif alors il peut bouger ou l'on clik avec la souris 
            {  
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo, 100, whatCanBeClickOn))
                    {
                        target = hitInfo.point;
                    }
                }
            }
            else if (state == 0)            // Si l'état de l'agent correspond à celui d'être actif alors il retourne à ça place initiale
            {
                target = scene;
            }
        }
        else if (type == "public")          // Si l'agent fait partie du public alors :
        {
            if (/*reduction_IDE*/true)
            {
                //essai de fluidification du mvt de la foule par l'avoidancePriority
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                /*if(agent.GetComponent<NavMeshAgent>().stoppingDistance > 1 && distanceTarget > agent.GetComponent<NavMeshAgent>().stoppingDistance &&
                   agent.GetComponent<NavMeshAgent>().stoppingDistance < 6)
                {
                    agent.GetComponent<NavMeshAgent>().avoidancePriority = 45;
                } 
                else
                {
                    agent.GetComponent<NavMeshAgent>().avoidancePriority = 50;
                }*/
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            }
            if (state == 0)                 // So l'agent souhaite sortir alors :
            {
                //agent.GetComponent<NavMeshAgent>().isStopped = false;
                //this.GetComponent<NavMeshAgent>().avoidancePriority = 50;
            }
            else if(state == 1)             // Si l'état de l'agent correspond à rester dans la salle alors :
            {
                distanceSinger = Vector3.Distance(this.GetComponent<NavMeshAgent>().transform.position, singer.GetComponent<NavMeshAgent>().transform.position);
                if (distanceSinger < 3)     // Si la distence séparant l'agent du chanteur est infèrieur à 3m, alors il le suit
                {
                    target = singer.GetComponent<NavMeshAgent>().transform.position;
                }
                else
                {                           // Sinon
                    distanceScene = Vector3.Distance(this.GetComponent<NavMeshAgent>().transform.position, scene);
                    if (distanceScene <= this.GetComponent<NavMeshAgent>().stoppingDistance)
                    {                       // Si l'agent est arrivé à la distence à laquelle il peut s'arrèter ou moins alors :
                        if (distanceStopSaved)
                        {                   // Si sa distence d'arret n'a pas été sauvegarder alors: on l'enregistre
                            oldDistanceStop = this.GetComponent<NavMeshAgent>().stoppingDistance;
                            distanceStopSaved = false;
                        }
                        //this.GetComponent<NavMeshAgent>().avoidancePriority = 51;
                        if (cooldown > 1f)
                        {
                            Dance();
                            cooldown = 0;
                        }
                    }
                    else if (cooldown > 1f)
                    {                       // Sinon il se dirige vers la scène, comme initailemant prévu
                        if (!distanceStopSaved)
                        {
                            this.GetComponent<NavMeshAgent>().stoppingDistance = oldDistanceStop;
                            distanceStopSaved = true;
                        }
                        target = this.scene;  
                    }
                }
                if (/*countdown != -1*/true)
                {
                    /*if (countdown == 5){
                        
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
                    }*/
                }
            }

            //agent.GetComponent<NavMeshAgent>().speed += .5F;
            //agent.GetComponent<NavMeshAgent>().radius = (float)(1.28*Mathf.Exp(distanceTarget - 10) + 0.22);
            cooldown += Time.deltaTime;
        }
        
        // Modification de la vitesse en temps réel
        this.GetComponent<NavMeshAgent>().speed = (float)Speed(contactCapsuleNumber);
        if (oldTarget != target)    // Si l'objectif de l'agent a été modifié alors : on met à jour sa destination
        {
            this.GetComponent<NavMeshAgent>().SetDestination(target);
        }
        oldTarget = target;
        
    }

    // Dance cherche à changer l'objectif de l'agent pour ue destination situé à X mètre
    // dans une direction aléatoire (Nd, Sd, Et, Ost) 
    public void Dance()
    {
        var aléatoire = Random.Range(0, 4);
        var distence = .5f;
        this.target = this.GetComponent<NavMeshAgent>().transform.position;
        switch (aléatoire)
        {
            case 0:
                target.z += distence;
                break;
            case 1:
                target.z -= distence;
                break;
            case 2:
                target.x += distence;
                break;
            case 3:
                target.x -= distence;
                break;
            default:
                break;
        }
        this.GetComponent<NavMeshAgent>().stoppingDistance = 0;
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
            var elementToDelelet = collision.gameObject.GetComponent<AgentSpawner>().agentCloneTodelete.Find(x => x.id == id);
            collision.gameObject.GetComponent<AgentSpawner>().agentCloneTodelete.Remove(elementToDelelet);
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