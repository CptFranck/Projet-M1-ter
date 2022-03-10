using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

public class AgentSpawner : MonoBehaviour
{
    public int id;
    public int index;
    public int maxNumberAgent;
    
    public GameObject TextPrefab;
    
    public AgentControl agentPrefab;
    public AgentControl agentSinger;
    public List<AgentControl> agentClone;

    public PercentBox percentTable;

    public GameObject spawnerDoor;
    public GameObject spawnerScene;
    public GameObject[] PointOfInterest;

    int nbAgentsAdded;//to get the input of the canva
    int nbContacts;
    int nbTotalContacts;
    InputField addAgentInput;
    InputField addFlowInput;
    // Start is called before the first frame update

    void Start()
    {
        // initialisation des param�tres de base du spawner
        id = 0;
        nbAgentsAdded = 0;
        nbContacts = 0;
        index = 0;
        index = 0;
        maxNumberAgent = 500;

        // initialisation de l'agent chanteur ("singer") qui spawnera sur la scene et dont la destination restera sa posistion
        percentTable = new PercentBox();
        agentSinger = Instantiate(agentPrefab, spawnerScene.transform.position, Quaternion.identity);
        agentSinger.type = "singer";
        agentSinger.scene = agentSinger.transform.position;
        agentSinger.target = agentSinger.transform.position;
        agentSinger.GetComponent<NavMeshAgent>().SetDestination(agentSinger.target);
        
        addAgentInput = GameObject.Find("AddAgentInputField").GetComponent<InputField>();
        addFlowInput = GameObject.Find("InputFlow").GetComponent<InputField>();

    }

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
    
    void Update(){
        nbTotalContacts = 0;
        for (int i = 0; i < index; i++){
            // setNbContacts(agentClone[i].contactCapsuleNumber);
            nbTotalContacts += agentClone[i].contactCapsuleNumber;
        }
    }    
        
    public void AddAgent()
    {
        if (index < maxNumberAgent)
        {

            int randNumbrer = Random.Range(0, PointOfInterest.Length);
            agentClone.Add(Instantiate(agentPrefab, spawnerDoor.transform.position, Quaternion.identity));
            agentClone[index].id = id;
            agentClone[index].type = "public";   
            agentClone[index].scene = PointOfInterest[randNumbrer].transform.position;
            agentClone[index].target = PointOfInterest[randNumbrer].transform.position;
            agentClone[index].agent.GetComponent<NavMeshAgent>().stoppingDistance = Random.Range(1,8);//SelectWithPurcent(percentTable);
            index++;
            id++;
        }
        else
        {
            GameObject floatingText = Instantiate(TextPrefab, GameObject.FindGameObjectWithTag("Canvas").transform);
            floatingText.GetComponent<TextMeshProUGUI>().text = "You can't add more agent !";
        }
        
    }

    // Add50Agent chercher � faire "spawner" 50 agents, en suivant le m�me processus que la AddAgent

    public void Add50Agent()
    {
        StartCoroutine(WaitAddAgent());
    }

    public IEnumerator WaitAddAgent(float flow = 0.25F)
    {
        var oneTime = true;
        for (int i = 0; i < 50; i++)
        {
            if (index < maxNumberAgent)
            {
                yield return new WaitForSeconds(flow);
                AddAgent();
            }
            else if (oneTime)
            {
                oneTime = false;
                GameObject floatingText = Instantiate(TextPrefab, GameObject.FindGameObjectWithTag("Canvas").transform);
                floatingText.GetComponent<TextMeshProUGUI>().text = "You can't add more agent !";
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
            GameObject floatingText = Instantiate(TextPrefab, GameObject.FindGameObjectWithTag("Canvas").transform);
            floatingText.GetComponent<TextMeshProUGUI>().text = "You can't delete agent !";
        }
    }

    // Delete50Agent chercher � d�truire 50 agents en les faisant sorir de la salle, en suivant le m�me processus que DeleteAgent
    public void Delete50Agent()
    {
        var oneTime = true;
        for (int i = 0; i < 50; i++)
        {
            if (index > 0)
            {
                DeleteAgent();
            }
            else if(oneTime)
            {
                oneTime = false;
                GameObject floatingText = Instantiate(TextPrefab, GameObject.FindGameObjectWithTag("Canvas").transform);
                floatingText.GetComponent<TextMeshProUGUI>().text = "You can't delete agent !";
            }
        }
    }
    //Add the number of agents manually 
    public void AddInputAgents(){
        //get the number inside the input field
        nbAgentsAdded = int.Parse(addAgentInput.text);
        var OneTime = true;
        for (int i = 0; i < nbAgentsAdded; i++)
        {
            if (index < MaxNumberAgent)
            {
                agentClone.Add(Instantiate(agentPrefab, spawnerDoor.transform.position, Quaternion.identity));
                int randNumbrer = Random.Range(0, PointOfInterest.Length);
                agentClone[index].id = id;
                agentClone[index].type = "public";
                agentClone[index].scene = PointOfInterest[randNumbrer].transform.position;
                agentClone[index].target = PointOfInterest[randNumbrer].transform.position;
                index++;
                id++;
            }
            else if (OneTime)
            {
                OneTime = false;
                GameObject floatingText = GameObject.Instantiate(TextPrefab, GameObject.FindGameObjectWithTag("Canvas").transform);
                floatingText.GetComponent<TextMeshProUGUI>().text = "You can't add more agent !";
            }
        }
    }
    public void DeleteInputAgent()
    {
        var OneTime = true;
        nbAgentsAdded = int.Parse(addAgentInput.text);
        for (int i = 0; i < nbAgentsAdded; i++)
        {
            if (index > 0)
            {
                var indexbis = Random.Range(0, index);
                agentClone[indexbis].state = 0;
                agentClone[indexbis].target = spawnerDoor.transform.position;
                agentClone.RemoveAt(indexbis);
                index--;
            }
            else if(OneTime)
            {
                OneTime = false;
                GameObject floatingText = GameObject.Instantiate(TextPrefab, GameObject.FindGameObjectWithTag("Canvas").transform);
                floatingText.GetComponent<TextMeshProUGUI>().text = "You can't delete agent !";
            }
        }
    }



    //get the number of agent created
    public int getPersonCount(){
        return index;
    }

    public void setNbContacts(int valueContact){
        nbContacts = valueContact;
    }
    public int getNbContacts(){
        return nbTotalContacts;
    }
}