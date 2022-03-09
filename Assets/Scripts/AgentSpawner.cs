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
        // initialisation des param�tres de base du spawner
        id = 0;
        index = 0;
        nbAgents = 0;
        maxNumberAgent = 500;

        // initialisation de l'agent chanteur ("singer") qui spawnera sur la scene et dont la destination restera sa posistion
        percentTable = new PercentBox();
        agentSinger = Instantiate(agentPrefab, spawnerScene.transform.position, Quaternion.identity);
        agentSinger.type = "singer";
        agentSinger.scene = agentSinger.transform.position;
        agentSinger.target = agentSinger.transform.position;
        agentSinger.GetComponent<NavMeshAgent>().SetDestination(agentSinger.target);

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

    // AddAgent chercher � faire "spawner" un agent si le nombre d'agent n'a pas �t� d�pass�, en
    // le faisant apparaitre pr�s de la porte, avec un destination al�atoire dirig� vers la sc�ne
    // si le nombre d'agent est d�j� atteint, alors un message apparait sur le canvas
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
            agentClone[index].agent.GetComponent<NavMeshAgent>().stoppingDistance = Random.Range(1,8);//SelectWithPurcent(percentTable);
            nbAgents++;
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
            nbAgents--;
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

    // GetPersonCount renvoie la veuleur du nombre d'agent encore existant dans la simulation
    public int GetPersonCount(){
        return nbAgents;
    }
}
