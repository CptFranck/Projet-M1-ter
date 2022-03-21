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
        //percent = new float[8] { .20F, .18F, .16F, .14F, .12F, .8F, .6F, .3F};
        // 1/8 = 0.125 1/64 = 0.015625 
        //percent = new float[8] { .125F, .125F, .125F, .125F, .125F, .125F, .125F, .125F };
        stopDistence = new int[8] { 0, 1, 2, 3, 4, 5, 6, 7};
    }
}
