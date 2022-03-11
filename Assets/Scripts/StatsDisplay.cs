using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsDisplay : MonoBehaviour
{
    private float timer;
    private float surface;
    private float densite;
    private float contactMoyen;

    public Text timerTxt;
    public Text densiteTxt;
    public Text nbPersonTxt;
    public Text nbContactsTxt;
    public Text dangerAsphyxiaTxt;

    public GameObject plane;
    public GameObject panelStats;
    private AgentSpawner agentSpawner;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        surface = 0;
        densite = 0f;
        contactMoyen = 0;
        panelStats.SetActive(true);
        agentSpawner = GameObject.FindObjectOfType(typeof(AgentSpawner)) as AgentSpawner;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //affiche temps en minutes et secondes (FloorToInt permet d'arondir la valeur)
        timerTxt.text = "Timer : " + (Mathf.FloorToInt(timer / 60)).ToString("00")  + ":" + (Mathf.FloorToInt(timer % 60)).ToString("00");
        //Ouvre l'interface lorsque le bouton i est appuyé
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
        dangerAsphyxiaTxt.text = "Personnes en danger d'asphyxie : " + agentSpawner.GetNbContactsInBox();
    }

    //Fonction pour calculer la densité moyenne
    float CalculateDensity(GameObject plane){
        surface = plane.GetComponent<Renderer>().bounds.size.x * plane.GetComponent<Renderer>().bounds.size.z; //Récupère la largeur et longueur du plane pour calculer la surface
        densite =  agentSpawner.GetPersonCount() / surface;  
        return densite;
    }
    
    //Fonction pour calculer le nombre de contact en moyenne
    float CalculateContactMoyen(){
        //si le nombre de personnes est différent de 0
        if (agentSpawner.GetPersonCount() != 0){
            contactMoyen = (float) (agentSpawner.GetNbContacts()) / (float) (agentSpawner.GetPersonCount());
        }
        return contactMoyen;
    }

    //Quand le bouton pause est cliqué, la simulation est gelée
    void PauseGame (){
        Time.timeScale = 0;
    }
    //Reprend la simulation
    void ResumeGame (){
        Time.timeScale = 1;
    }
}
