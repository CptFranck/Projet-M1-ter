using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastCollHit : MonoBehaviour
{
    // Start is called before the first frame update
    public Collider LastEntered;
    public Collider LastExited;

    private void OnTriggerEnter(Collider other)
    {
        LastEntered = other;
    }

    private void OnTriggerExit(Collider other)
    {
        LastExited = other;
    }
}
