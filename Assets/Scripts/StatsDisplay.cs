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
    AgentSpawner personCount;
    AgentControl personContacts;
    
    float surface;
    float densite;
    // Start is called before the first frame update
    void Start()
    {
        surface = 0;
        densite = 0f;
        
        statsUIDisplay.SetActive(false);
        personCount = GameObject.FindObjectOfType(typeof(AgentSpawner)) as AgentSpawner;
        personContacts = GameObject.FindObjectOfType(typeof(AgentControl)) as AgentControl;
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
        nbPersonTxt.text = "Nb de personnes : " + personCount.getPersonCount();
        // nbContactsTxt.text = "Nb de contacts en moyenne : " + personContacts.getNbCollisions();
        densiteTxt.text = "Nombre de personnes par m² : " + CalculateDensity(floor);
    
    }

    float CalculateDensity(GameObject floor){
        surface = floor.GetComponent<Renderer>().bounds.size.x * floor.GetComponent<Renderer>().bounds.size.z;
        densite =  Mathf.Round((personCount.getPersonCount() / surface) *100f) / 100f; //à deux chiffres après la virgule près        
        return densite;
    }
}
