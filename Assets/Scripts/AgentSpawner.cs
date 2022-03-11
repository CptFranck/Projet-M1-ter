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

    //private int nbContacts;
    private int nbTotalContacts;
    private int nbAgentInAsphyxiaDanger;

    public float flow;                      // Attribut correspondant au flux d'agent lors de leur création          

    public GameObject textPrefab;           // Attributs correspondants à l'interface
    public InputField flowInput;
    public InputField inputNumberAgents;

    public AgentControl agentPrefab;        // Attribut correspondants à l'objet associés au prefab des agents
    public AgentControl agentSinger;
    public List<AgentControl> agentClone;

    //public PercentBox percentTable;

    public GameObject spawnerDoor;          // Attributs correspondants aux point 3D pour les différentes destinations
    public GameObject spawnerScene;
    public GameObject[] pointOfInterest;
    
    void Start()
    {
        // initialisation des paramètres de base du spawner
        id = 0;
        flow = 0;
        index = 0;
        maxNumberAgent = 300;
        nbAgentInAsphyxiaDanger = 0;

        // initialisation de l'agent chanteur ("singer") qui spawnera sur la scene et dont la destination restera sa posistion
        // percentTable = new PercentBox();
        agentSinger = Instantiate(agentPrefab, spawnerScene.transform.position, Quaternion.identity);
        agentSinger.type = "singer";
        agentSinger.scene = agentSinger.transform.position;
        agentSinger.target = agentSinger.transform.position;
        agentSinger.GetComponent<NavMeshAgent>().SetDestination(agentSinger.target);

        inputNumberAgents = GameObject.Find("InputFieldAgent").GetComponent<InputField>();
        flowInput = GameObject.Find("InputFlow").GetComponent<InputField>();

    }    

    void Update(){
        //reset à chaque frame pour avoir une mise à jour toute les frames 
        nbTotalContacts = 0;
        nbAgentInAsphyxiaDanger = 0;
        //parcours la liste d'agent
        for (int i = 0; i < index; i++){
            nbTotalContacts += agentClone[i].contactCapsuleNumber; //ajoute le nombre de contact de l'agent 
            //si l'agent a plus de 8 contacts dans sa box de 1m². La valeur 8 peut bien-sûr être changée
            if (agentClone[i].contactBoxNumber > 8){
                nbAgentInAsphyxiaDanger++;
            }
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
        Debug.Log(flow);
        flow = float.Parse(flowInput.text);
        Debug.Log(flow);
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
            agentClone[index].id = id;
            agentClone[index].type = "public";   
            agentClone[index].scene = pointOfInterest[randNumbrer].transform.position;
            agentClone[index].target = pointOfInterest[randNumbrer].transform.position;
            agentClone[index].agent.GetComponent<NavMeshAgent>().stoppingDistance = Random.Range(0,8);  //SelectWithPurcent(percentTable);
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

    public void ResetListAgents(){
        for (int i = 0; i < index; i++){
            agentClone[i].ResetAgent();
        }
        index = 0;
        nbTotalContacts = 0;
        nbAgentInAsphyxiaDanger = 0;
    }

    //GETTERS
    public int GetPersonCount(){
        return index;
    }

    public int GetNbContacts(){
        return nbTotalContacts;
    }

    public int GetNbContactsInBox(){
        return nbAgentInAsphyxiaDanger;
    }
}