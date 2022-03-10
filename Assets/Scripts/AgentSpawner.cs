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
    public int index;//number of agent added
    public int MaxNumberAgent;
    public GameObject TextPrefab;
    
    public AgentControl agentPrefab;
    public AgentControl agentSinger;
    public List<AgentControl> agentClone;

    public GameObject[] PointOfInterest;
    public GameObject spawnerDoor;
    public GameObject spawnerScene;

    int nbAgentsAdded;//to get the input of the canva
    int nbContacts;
    int nbTotalContacts;
    InputField addAgentInput;
    // Start is called before the first frame update
    void Start()
    {
        id = 0;
        nbAgentsAdded = 0;
        nbContacts = 0;
        index = 0;
        MaxNumberAgent = 500;
        agentSinger = Instantiate(agentPrefab, spawnerScene.transform.position, Quaternion.identity);
        agentSinger.type = "singer";
        agentSinger.scene = agentSinger.transform.position;
        agentSinger.target = agentSinger.transform.position;
        agentSinger.GetComponent<NavMeshAgent>().SetDestination(agentSinger.target);
        
        addAgentInput = GameObject.Find("AddAgentInputField").GetComponent<InputField>();

    }
    
    public void AddAgent()
    {
        if (index < MaxNumberAgent)
        {
            agentClone.Add(Instantiate(agentPrefab, spawnerDoor.transform.position, Quaternion.identity));
            agentClone[index].id = id;
            agentClone[index].type = "public";
            int randNumbrer = Random.Range(0, PointOfInterest.Length);
            agentClone[index].scene = PointOfInterest[randNumbrer].transform.position;
            agentClone[index].target = PointOfInterest[randNumbrer].transform.position;
            index++;
            id++;

        }
        else
        {
            GameObject floatingText = GameObject.Instantiate(TextPrefab, GameObject.FindGameObjectWithTag("Canvas").transform);
            floatingText.GetComponent<TextMeshProUGUI>().text = "You can't add more agent !";
        }
        
    }

    public void Add50Agent()
    {
        var OneTime = true;
        for (int i = 0; i < 50; i++)
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

    public void DeleteAgent()
    {
        if(index > 0) { 
            var indexbis = Random.Range(0, index);
            agentClone[indexbis].state = 0;
            agentClone[indexbis].target = spawnerDoor.transform.position;
            agentClone.RemoveAt(indexbis);
            index--;
        }
        else
        {
            GameObject floatingText = GameObject.Instantiate(TextPrefab, GameObject.FindGameObjectWithTag("Canvas").transform);
            floatingText.GetComponent<TextMeshProUGUI>().text = "You can't delete agent !";
        }
    }

    public void Delete50Agent()
    {
        var OneTime = true;
        for (int i = 0; i < 50; i++)
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

    void Update(){
        for (int i = 0; i < index; i++){
            setNbContacts(agentClone[i].contactCapsuleNumber);
            nbTotalContacts += agentClone[i].contactCapsuleNumber;
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