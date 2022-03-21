using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

public class AgentSpawner : MonoBehaviour
{
    public int id;                          // Attribut correspondant à l'attribution des id de manière unique
    public int index;                       // Attributs correspondant aux informations générales des agents
    public int maxNumberAgent;

    private int nbTotalContacts;

    public float flow;                      // Attribut correspondant au flux d'agent lors de leur création
    public float desiredSeparation;

    public GameObject textPrefab;           // Attributs correspondants à l'interface
    public InputField flowInput;
    public InputField inputNumberAgents;

    public AgentControl agentPrefab;        // Attribut correspondants à l'objet associés au prefab des agents
    public AgentControl agentSinger;
    public List<AgentControl> agentClone;
    public List<AgentControl> agentCloneTodelete;

    //public PercentBox percentTable;

    public GameObject spawnerDoor;          // Attributs correspondants aux point 3D pour les différentes destinations
    public GameObject spawnerScene;
    public GameObject[] pointOfInterest;
    
    void Start()
    {
        // initialisation des paramètres de base du spawner
        id = 0;
        index = 0;
        maxNumberAgent = 300;

        nbTotalContacts = 0;

        flow = 0;
        desiredSeparation = .5f;

        // textPrefab               instantié dans Unity
        // flowInput
        // inputNumberAgents

        // agentPrefab              instantié dans Unity
        // agentSinger
        agentClone = new List<AgentControl>();

        // spawnerDoor              instantié dans Unity
        // spawnerScene
        // pointOfInterest

        // initialisation de l'agent chanteur ("singer") qui spawnera sur la scene et dont la destination restera sa posistion
        // percentTable = new PercentBox();
        agentSinger = Instantiate(agentPrefab, spawnerScene.transform.position, Quaternion.identity);
        agentSinger.id = -1;
        agentSinger.type = "singer";
        agentSinger.whatCanBeClickOn = LayerMask.GetMask("Scene");
        agentSinger.scene = spawnerScene.transform.position;
        agentSinger.target = spawnerScene.transform.position;

        // initialisation des variables lié à l'interface
        inputNumberAgents = GameObject.Find("InputFieldAgent").GetComponent<InputField>();
        flowInput = GameObject.Find("InputFlow").GetComponent<InputField>();

    }    

    void Update(){
        nbTotalContacts = 0;
        for (int i = 0; i < agentClone.Count; i++)
        {
            nbTotalContacts += agentClone[i].contactCapsuleNumber;
            
            //boids repulsion
            /*
            var found = 0;
            var average = Vector3.zero;
            //var notAlreadyReset = false;
            for (int j = 0; j < agentClone[i].areaId.Count; j++)
            {
               var distence = agentClone[i].GetComponent<Transform>().position - agentClone[agentClone[i].areaId[j]].GetComponent<Transform>().position;
                if(desiredSeparation > distence.magnitude)
                {
                    average += distence;
                    found++;
                }
            }
            if (found > 0)
            {
                average /= (found* agentClone[i].GetComponent<NavMeshAgent>().speed);
                //agentClone[i].gameObject.transform.position = Vector3.Lerp(transform.localPosition, average, Time.deltaTime / totalRunningTime);
                agentClone[i].gameObject.transform.position += (average*0.07f);
                //StartCoroutine(AddForceBis(i , average));
            }/*
                
                agentClone[i].GetComponent<Rigidbody>().AddForce(average);
                agentClone[i].GetComponentInChildren<Rigidbody>().AddForce(average);
                notAlreadyReset = true;
            }
            else 
            if(notAlreadyReset){
                notAlreadyReset = true;
                agentClone[i].GetComponent<Rigidbody>().velocity = agentPrefab.GetComponent<Rigidbody>().velocity;
                agentClone[i].GetComponentInChildren<Rigidbody>().velocity = agentPrefab.GetComponentInChildren<Rigidbody>().velocity;
            }*/
        }
    }
    
    public IEnumerator AddForceBis(int i, Vector3 average)
    {
        Vector3 start = agentClone[i].GetComponent<Transform>().position;
        Vector3 end = average;
        float speed = agentClone[i].GetComponent<NavMeshAgent>().speed;
        //total time this has been running
        float runningTime = 0;
        //the longest it would take to get to the destination at this speed
        float totalRunningTime = Vector3.Distance(start, end) / speed;
        //for the length of time it takes to get to the end position
        while (runningTime < totalRunningTime)
        {
            //keep track of the time each frame
            runningTime += Time.deltaTime;
            //lerp between start and end, based on the current amount of time that has passed
            // and the total amount of time it would take to get there at this speed.
            agentClone[i].gameObject.transform.position = Vector3.Lerp(start, end, runningTime / totalRunningTime);
            yield return 0;
        }
    }

    // SelectWithPurcent permet l'attribution d'une distence de stoppage en fonction d'un tableau de pourcentage attribuant des
    // poids à des probabilités
    /*public int SelectWithPurcent(PercentBox percentTable)
    {
        float r;
        int i = 0;

        do
        {
            r = Random.value;
        } while (r == 1 || r == 0);

        while (r > 0)
        {

            r -= percentTable.percent[i];
            i++;
        }
        i--;

        return percentTable.stopDistence[i];

    }*/

    // ChangeFlow permet de changer la valeurs de flux lors de la création des agents
    public void ChangeFlow()
    {
        var flowtest = 0.1f;
        if (float.TryParse(flowInput.text, out flowtest))
        {
            if(flowtest < 0)
            {
                DisplayText("Input not valid, flow can't be lower than 0 !");
            } else if(flowtest > 2)
            {
                DisplayText("Input not valid, flow can't be bigger than 2 !");
            } else
            {
                DisplayText("Flow has been changed !");
                flow = flowtest;
            }       
        }
        else
        {
            DisplayText("Input not valid, try with a ',' and not with '.' !");
        }
       
    }

    // DisplayText permet l'affichage d'un message
    public void DisplayText(string message)
    {
        GameObject floatingText = Instantiate(textPrefab, GameObject.FindGameObjectWithTag("Canvas").transform);
        floatingText.GetComponent<TextMeshProUGUI>().text = message;
    }

    // DeleteAgent chercher à créer un agent si le nombre d'agent n'est pas dépassé, en lui
    // donnant comme destination un point d'interet aléatoire comme cible et en lui donnant
    // un distance de d'arrêt aléatoire comprise entre 0 et 8
    public void AddAgent()
    {
        if (index < maxNumberAgent)
        {
            int randNumbrer = Random.Range(0, pointOfInterest.Length);
            agentClone.Add(Instantiate(agentPrefab, spawnerDoor.transform.position, Quaternion.identity));
            agentClone[index].singer = agentSinger;
            agentClone[index].id = id;
            agentClone[index].type = "public";
            agentClone[index].scene = pointOfInterest[randNumbrer].transform.position;
            agentClone[index].target = pointOfInterest[randNumbrer].transform.position;
            agentClone[index].GetComponent<NavMeshAgent>().stoppingDistance = Random.Range(.1f, 8);  //SelectWithPurcent(percentTable);
            agentClone[index].GetComponent<NavMeshAgent>().SetDestination(agentClone[index].target);
            index++;
            id++;
        }
        else
        {
            DisplayText("You can't add more agent !");
        }     
    }

    // Add50Agent chercher � faire "spawner" 50 agents, en suivant le m�me processus que la AddAgent
    public void Add50Agent(bool useInput = false)
    {
        if (useInput)
        {
            StartCoroutine(WaitAddAgent(int.Parse(inputNumberAgents.text)));
        }
        else
        {
            StartCoroutine(WaitAddAgent());
        }   
    }

    // AddInputAgents chercher � faire "spawner" un nombre n agents, en suivant le m�me processus que la AddAgent
    public void AddInputAgents()
    {
        Add50Agent(true);
    }
    public IEnumerator WaitAddAgent(int inputNumberAgents = 50)
    {
        var oneTime = true;
        for (int i = 0; i < inputNumberAgents; i++)
        {
            if (index < maxNumberAgent)
            {
                if (flow != 0)
                {
                    yield return new WaitForSeconds(flow);
                }
                AddAgent();
            }
            else if (oneTime)
            {
                oneTime = false;
                DisplayText("You can't add more agent !");
            }
        }
    }

    // DeleteAgent chercher � supprimer un agent si le nombre d'agent n'a pas d�j� nulle, en lui
    // changeant son �tat pour le mettre � 0 et sa distnce de stoppage (afin de permettre sa des-
    // truction au contact de la porte), puis en l'enlevant de la liste des agents cr��s
    public void DeleteAgent()
    {
        if (index > 0) {
            var indexbis = Random.Range(0, index);
            agentClone[indexbis].state = 0;
            agentClone[indexbis].target = spawnerDoor.transform.position;
            agentClone[indexbis].GetComponent<NavMeshAgent>().stoppingDistance = 0;
            agentCloneTodelete.Add(agentClone[indexbis]);  
            agentClone.RemoveAt(indexbis);
            index--;
        }
        else
        {
            DisplayText("You can't delete agent !");
        }
    }
    // Delete50Agent chercher � d�truire 50 agents en les faisant sorir de la salle, en suivant le m�me processus que DeleteAgent
    public void Delete50Agent(bool useInput = false)
    {
        if (useInput)
        {
            StartCoroutine(WaitDeleteAgent(int.Parse(inputNumberAgents.text)));
        }
        else
        {
            StartCoroutine(WaitDeleteAgent());
        }
    }

    // Delete50Agent chercher � d�truire N agents en les faisant sorir de la salle, en suivant le m�me processus que DeleteAgent
    public void DeleteInputAgent()
    {
        Delete50Agent(true);
    }

    public IEnumerator WaitDeleteAgent(int nbAgentsAdded = 50)
    {
        var oneTime = true;
        for (int i = 0; i < nbAgentsAdded; i++)
        {
            if (index > 0)
            {
                if (flow != 0)
                {
                    yield return new WaitForSeconds(flow);
                }
                DeleteAgent();
            }
            else if (oneTime)
            {
                oneTime = false;
                DisplayText("You can't add more agent !");
            }
        }
    }

    public void ResetAllAgents()
    {
        var nb = agentClone.Count;
        for (int i = 0; i < nb; i++)
        {
            Destroy(agentClone[i].gameObject);
            index--;
        }
        nb = agentCloneTodelete.Count;
        for (int i = 0; i < nb; i++)
        {
            Destroy(agentCloneTodelete[i].gameObject);
        }
        agentClone.Clear();
        agentCloneTodelete.Clear();
    }

    // Getters
    public int GetPersonCount(){
        return index;
    }

    public int GetNbContacts(){
        return nbTotalContacts;
    }
}