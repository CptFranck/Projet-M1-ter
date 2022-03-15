using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsDisplay : MonoBehaviour
{
    private float surface;
    private float densite;
    private float contactMoyen;

    public Text densiteTxt;
    public Text nbPersonTxt;
    public Text nbContactsTxt;

    public GameObject plane;
    public GameObject panelStats;
    private AgentSpawner agentSpawner;
    
    // Start is called before the first frame update
    void Start()
    {
        surface = 0;
        densite = 0f;
        panelStats.SetActive(true);
        agentSpawner = GameObject.FindObjectOfType(typeof(AgentSpawner)) as AgentSpawner;
    }

    // Update is called once per frame
    void Update()
    {
        //Quand le bouton i est appuyé, affice l'interface
        if(Input.GetButtonDown("Statistics")){
            panelStats.SetActive(!panelStats.activeSelf);
        }
        //update toujours l'interface
        UpdateUI();
    }

    public void UpdateUI(){
        //Change le texte de l'interface pour le nombre de personnes
        nbPersonTxt.text = "Nb de personnes : " + agentSpawner.GetPersonCount();
        densiteTxt.text = "Nombre de personnes par m² : " + CalculateDensity(plane).ToString("f2"); //Montre deux chiffres après la virgule
        nbContactsTxt.text = "Nb de contacts en moyenne : " + CalculateContactMoyen().ToString("f2");
    }

    //Fonction pour calculer la densité moyenne
    float CalculateDensity(GameObject plane){
        surface = plane.GetComponent<Renderer>().bounds.size.x * plane.GetComponent<Renderer>().bounds.size.z;
        densite =  agentSpawner.GetPersonCount() / surface;  
        return densite;
    }
    
    float CalculateContactMoyen(){
        if (agentSpawner.GetPersonCount() != 0){
            contactMoyen = (float) (agentSpawner.GetNbContacts() / (float) agentSpawner.GetPersonCount());
        }
        return contactMoyen;
    }
}
