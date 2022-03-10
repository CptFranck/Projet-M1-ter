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
    float surface;
    float densite;
    float contactMoyen;
    // Start is called before the first frame update
    void Start()
    {
        surface = 0;
        densite = 0f;
        
        statsUIDisplay.SetActive(false);
        personCount = GameObject.FindObjectOfType(typeof(AgentSpawner)) as AgentSpawner;
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
        // nbContactsTxt.text = "Nb de contacts en moyenne : " + CalculateContactMoyen().ToString("f2");
        densiteTxt.text = "Nombre de personnes par m² : " + CalculateDensity(floor).ToString("f2"); //Montre deux chiffres après la virgule
    }

    //Fonction pour calculer la densité moyenne
    float CalculateDensity(GameObject floor){
        surface = floor.GetComponent<Renderer>().bounds.size.x * floor.GetComponent<Renderer>().bounds.size.z;
        densite =  personCount.getPersonCount() / surface; //à deux chiffres après la virgule près        
        return densite;
    }

    
    // float CalculateContactMoyen(){
    //     if (personCount.getPersonCount() != 0){
    //         contactMoyen = personCount.getNbContacts()/personCount.getPersonCount();
    //     }
    //     Debug.Log(contactMoyen);
    //     return contactMoyen;
    // }
}
