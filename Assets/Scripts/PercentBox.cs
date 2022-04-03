using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentBox
{
    public float[] percent;
    public int[] eventCode;
    
    // Constructeur de notre objet servant à associé des poids de probabilité, à des distances d'arrets
    public PercentBox()
    {
        percent = new float[3] { .8f, .1f, .1f };
        eventCode = new int[3] { 0, 1, 2};
    }
}
