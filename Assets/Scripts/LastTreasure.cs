using System;
using System.Collections;
using UnityEngine;

public class LastTreasure : MonoBehaviour
{
    public static int secretActivated = 0;
    private bool thisActivated;
    private Transform textParent;
    private Transform myCamera;

    private void Start()
    {
        myCamera = Camera.main.transform;
        textParent = transform.GetChild(0);
        textParent.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (thisActivated) return;
        thisActivated = true;
        secretActivated++;
        textParent.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (textParent.gameObject.activeSelf)
        {
            textParent.LookAt(myCamera);
        }
    }
}
