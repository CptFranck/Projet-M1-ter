using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class StatsDisplay : MonoBehaviour
{
    private int id;
    private float timer;
    private float surface;
    private float densite;
    private float contactMoyen;
    private Vector3[] coordinates;
    private BoxCollider boxCollider;
    private List<MapControl> cubeList;
    // private string[,] nbOfAgentsInSquare;
    // private int densityInSquare;
    // private int i;

    public Text timerTxt;
    public Text densiteTxt;
    public Text nbPersonTxt;
    public Text nbContactsTxt;
    public Text dangerAsphyxiaTxt;

    public GameObject plane;
    public GameObject panelStats;
    public MapControl cubePrefab;

    private AgentSpawner agentSpawner;
    bool agentDetected;

    // Start is called before the first frame update
    void Start()
    {
        // i = 0;
        id = 0;
        timer = 0;
        surface = 0;
        densite = 0f;
        contactMoyen = 0;
        panelStats.SetActive(true);
        cubeList = new List<MapControl>();
        boxCollider = GetComponent<BoxCollider>();
        coordinates = plane.GetComponent<MeshFilter>().sharedMesh.vertices;
        agentSpawner = GameObject.FindObjectOfType(typeof(AgentSpawner)) as AgentSpawner;
        // nbOfAgentsInSquare = new string[100,2];
        // densityInSquare = 0;
        
        //Créer un cube pour chaque m² de la surface
        CreateDensityMap();

        //Met à jour l'interface visuelle toutes les secondes
        InvokeRepeating("UpdateUI", 1f, 1f);
        // InvokeRepeating("GetEachMeter", 1f, .05f);
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

        //Affiche la densité sur la carte
        GetEachMeter();

        //Réinitialisation des valeurs à chaque frame
        // densityInSquare = 0;
        // foreach (MapControl cube in cubeList){
        //     cube.SetContacts(0);
        // }
        // Array.Clear(nbOfAgentsInSquare, 0, nbOfAgentsInSquare.Length);
        // i = 0;
    }

    //Fonction qui update les données sur l'interface visuelle
    void UpdateUI(){
        
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
    void StartGame (){
        Time.timeScale = 1;
    }

    //Réinitialise toute la simulation
    void ResetGame (){
        densite = 0;
        timer = 0;
        Time.timeScale = 0;
        contactMoyen = 0;
        foreach (MapControl cube in cubeList){
            cube.SetContacts(0);     
            cube.GetComponent<MeshRenderer>().material.SetColor("_Color", GradientDensity(cube.GetContacts()));
        }
        if (agentSpawner.GetNbContacts() > 0){
            agentSpawner.ResetAllAgents();         
        }

    }

    //Fonction qui permet de récupérer la position de chaque mètre carré du sol
    void CreateDensityMap(){
        //Parcours tous les mètre carré de la carte
        for (float i = 0.5f; i < plane.GetComponent<Renderer>().bounds.size.x; i++){
            for (float j = 0.5f; j < plane.GetComponent<Renderer>().bounds.size.x; j++){
                //Crée un cube à chaque m²
                MapControl mapControl = Instantiate(cubePrefab, new Vector3(i, -0.5f, j), Quaternion.identity);
                //L'instantie en tant qu'enfant de plane
                mapControl.transform.parent = plane.transform;
                mapControl.transform.localPosition = new Vector3(i, -0.5f, j);
                cubeList.Add(mapControl);
                cubeList[id].SetId(id);
                id++;
            }
        }
        
        // foreach(Vector3 square in coordinates){
            // //Crée une box pour chaque mètre et détect s'il y a un contact
            // agentDetected = Physics.CheckBox(square, Vector3.one);
            // if (agentDetected){
            //     densityInSquare++; // ajoute le nombre d'agents qui sont en contacts
            //     // Debug.Log(densityInSquare);
            // }

            // //Range dans un tableau 2d les informations
            // nbOfAgentsInSquare[i,0] = densityInSquare.ToString();
            // nbOfAgentsInSquare[i,1] = square.ToString();
            // // Debug.Log("nb agents : " + nbOfAgentsInSquare[i, 0] + " location : " + nbOfAgentsInSquare[i, 1]);
            // i++;
        // }
    }

    //Change la couleur du sol pour chaque cube en fonction de la densité
    void GetEachMeter(){
        foreach (MapControl cube in cubeList){            
            //Change la couleur du cube en fonction de la densité de celui-ci
            cube.GetComponent<MeshRenderer>().material.SetColor("_Color", GradientDensity(cube.GetContacts()));
        }
    }

    //Change la couleur du sol en fonction de la densité
    Color GradientDensity(float gradientDensity){
        if (gradientDensity > 0 && gradientDensity <= 2){
            //floorMaterial.SetColor("_Color", new Color(0.517f, 0.933f, 0.534f)) ; //vert
            return new Color(0.517f, 0.933f, 0.534f); //vert
        }else if(gradientDensity > 2 && gradientDensity <= 3){
            return new Color(0.933f, 0.933f, 0.497f); //jaune
        }else if(gradientDensity > 3 && gradientDensity <= 5){
            return new Color(1, 0.607f, 0.410f); //orange
        }else if(gradientDensity > 5 && gradientDensity <= 8){
            return new Color(1, 0.212f, 0.212f); //rouge
        }else if(gradientDensity > 7){
            return new Color(0.245f, 0.128f, 0.128f); //noir
        }
        return new Color(0.435f, 0.435f, 0.435f); // gris par défaut
    }

    //Fonction permettant de sauvegarder des données à un temps t
    //Ce n'est qu'un début d'étude, cela pourrait être un point à continuer
    public void SaveFile(){
        Debug.Log("Saved");
        string destination = Application.persistentDataPath + "/save.dat";
        Debug.Log(destination);
        FileStream file;
 
        if(File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);
 
        SaveDatas data = new SaveDatas(timer, agentSpawner.GetPersonCount(), CalculateDensity(plane).ToString("f2"), CalculateContactMoyen().ToString("f2"), agentSpawner.GetNbContactsInBox());
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }
}
