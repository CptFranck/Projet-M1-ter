using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveDatas
{
    public float timer;
    public int nbPerson;
    public string density;
    public string contactMoy;
    public int asphyxia;

    public SaveDatas(float timerFloat, int nbPersonInt, string densityStr, string contactMoyStr, int asphyxiaInt){
        timer = timerFloat;
        nbPerson = nbPersonInt;
        density = densityStr;
        contactMoy = contactMoyStr;
        asphyxia = asphyxiaInt;
    }
}
