using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsDisplay : MonoBehaviour
{
    public GameObject statsUIDisplay;
    public Text nbPersonTxt;
    AgentSpawner personCount;
    // Start is called before the first frame update
    void Start()
    {
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
    }

    float CalculateSurfaceAre(Mesh mesh){
        var triangles = mesh.triangles;
        var vertices = mesh.vertices;

        double sum = 0.0;

        for(int i = 0; i < triangles.Length; i += 3) {
            Vector3 corner = vertices[triangles[i]];
            Vector3 a = vertices[triangles[i + 1]] - corner;
            Vector3 b = vertices[triangles[i + 2]] - corner;

            sum += Vector3.Cross(a, b).magnitude;
        }

        return (float)(sum/2.0);
        }
}
