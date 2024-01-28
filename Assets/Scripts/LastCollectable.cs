using System;
using UnityEngine;

public class LastCollectable : MonoBehaviour
{
    private bool done;
    private GameObject lastSecret;

    private void Start()
    {
        lastSecret = transform.GetChild(0).gameObject;
        lastSecret.SetActive(false);
    }

    private void Update()
    {
        if (done) return;
        if (LastTreasure.secretActivated == 3)
        {
            done = true;
            lastSecret.SetActive(true);
        }
    }
}
