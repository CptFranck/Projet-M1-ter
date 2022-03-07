using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentBox
{
    public float[] percent_1;
    public int[] stopDistence_1;
    public float[] percent_2;
    public int[] stopDistence_2;
    public float[] percent_3;
    public int[] stopDistence_3;
    public float[] percent_4;
    public int[] stopDistence_4;

    public PercentBox()
    {
        percent_1 = new float[6] { .40F, .30F, .20F, .5F, .3F, 2F};
        stopDistence_1 = new int[6] { 0, 1, 2, 3, 4, 5};
        percent_2 = new float[6] { .35F, .30F, .20F, .10F, .5F, .2F };
        stopDistence_2 = new int[6] { 1, 2, 3, 4, 5, 6 };
        percent_3 = new float[6] { .25F, .25F, .25F, .15F, .5F, .5F };
        stopDistence_3 = new int[6] { 2, 3, 4, 5, 6, 7 };
        percent_4 = new float[6] { .20F, .20F, .20F, .20F, .10F, .10F };
        stopDistence_4 = new int[6] { 0, 1, 2, 3, 4, 5 };
    }
}
