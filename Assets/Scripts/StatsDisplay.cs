using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsDisplay : MonoBehaviour
{
    public GameObject statsUIDisplay;
    public GameObject floor;
    public Text nbPersonTxt;
    public Text nbContactsTxt;
    public Text densiteTxt;
    AgentSpawner agentSpawner;
    float surface;
    float densite;
    float contactMoyen;
    // Start is called before the first frame update
    void Start()
    {
        surface = 0;
        densite = 0f;
        
        statsUIDisplay.SetActive(true);
        agentSpawner = GameObject.FindObjectOfType(typeof(AgentSpawner)) as AgentSpawner;
      }

    // Update is called once per frame
    void Update()
    {
        //Quand le bouton i est appuyé, affice l'interface
        if(Input.GetButtonDown("Statistics")){
            statsUIDisplay.SetActive(!statsUIDisplay.activeSelf);
        }
        //update toujours l'interface
        UpdateUI();
    }

    public void UpdateUI(){
        //Change le texte de l'interface pour le nombre de personnes
        nbPersonTxt.text = "Nb de personnes : " + agentSpawner.getPersonCount();
        densiteTxt.text = "Nombre de personnes par m² : " + CalculateDensity(floor).ToString("f2"); //Montre deux chiffres après la virgule
        nbContactsTxt.text = "Nb de contacts en moyenne : " + CalculateContactMoyen().ToString("f2");
    }

    //Fonction pour calculer la densité moyenne
    float CalculateDensity(GameObject floor){
        surface = floor.GetComponent<Renderer>().bounds.size.x * floor.GetComponent<Renderer>().bounds.size.z;
        densite =  agentSpawner.getPersonCount() / surface;  
        return densite;
    }
    
    float CalculateContactMoyen(){
        if (agentSpawner.getPersonCount() != 0){
            contactMoyen = (float) (agentSpawner.getNbContacts() / agentSpawner.getPersonCount());
        }
        Debug.Log("Nb contact total " + agentSpawner.getNbContacts());
        Debug.Log("contact moyen " +contactMoyen);
        return contactMoyen;
    }
}
