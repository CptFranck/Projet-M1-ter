using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentBox
{
    public float[] percent;
    public int[] stopDistence;
    
    // Constructeur de notre objet servant à associé des poids de probabilité, à des distances d'arrets
    public PercentBox()
    {
        percent = new float[10] { .20F, .18F, .16F, .14F, .12F, .8F, .6F, .3F, .2F, 1F};
        stopDistence = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
    }
}
