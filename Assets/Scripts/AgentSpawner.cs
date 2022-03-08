using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class AgentSpawner : MonoBehaviour
{
    public int id;
    public int index;
    public int nbAgents;
    public int maxNumberAgent;
    

    public GameObject TextPrefab;
    
    public AgentControl agentPrefab;
    public AgentControl agentSinger;
    public List<AgentControl> agentClone;

    public PercentBox percentTable;

    public GameObject spawnerDoor;
    public GameObject spawnerScene;
    public GameObject[] PointOfInterest;

    void Start()
    {
        id = 0;
        index = 0;
        nbAgents = 0;     
        maxNumberAgent = 500;
        percentTable = new PercentBox();
        agentSinger = Instantiate(agentPrefab, spawnerScene.transform.position, Quaternion.identity);
        agentSinger.type = "singer";
        agentSinger.scene = agentSinger.transform.position;
        agentSinger.target = agentSinger.transform.position;
        agentSinger.GetComponent<NavMeshAgent>().SetDestination(agentSinger.target);

    }

    public int SelectWithPurcent(PercentBox percentTable)
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
        
    }

    public void AddAgent()
    {
        if (nbAgents < maxNumberAgent)
        {
            int randNumbrer = Random.Range(0, PointOfInterest.Length);
            agentClone.Add(Instantiate(agentPrefab, spawnerDoor.transform.position, Quaternion.identity));
           
            agentClone[index].id = id;
            agentClone[index].type = "public";   
            agentClone[index].scene = PointOfInterest[randNumbrer].transform.position;
            agentClone[index].target = PointOfInterest[randNumbrer].transform.position;
            agentClone[index].GetComponent<NavMeshAgent>().stoppingDistance = SelectWithPurcent(percentTable);
            nbAgents++;
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
        var oneTime = true;
        for (int i = 0; i < 50; i++)
        {
            if (index < maxNumberAgent)
            {
                AddAgent();
            }
            else if (oneTime)
            {
                oneTime = false;
                GameObject floatingText = GameObject.Instantiate(TextPrefab, GameObject.FindGameObjectWithTag("Canvas").transform);
                floatingText.GetComponent<TextMeshProUGUI>().text = "You can't add more agent !";
            }
        }
    }

    public void DeleteAgent()
    {
        
        if (index > 0) { 
            var indexbis = Random.Range(0, index);
            agentClone[indexbis].state = 0;
            agentClone[indexbis].target = spawnerDoor.transform.position;
            agentClone[indexbis].GetComponent<NavMeshAgent>().stoppingDistance = 0;
            agentClone.RemoveAt(indexbis);
            nbAgents--;
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
                GameObject floatingText = GameObject.Instantiate(TextPrefab, GameObject.FindGameObjectWithTag("Canvas").transform);
                floatingText.GetComponent<TextMeshProUGUI>().text = "You can't delete agent !";
            }
        }
    }
    public int GetPersonCount(){
        return nbAgents;
    }
}
