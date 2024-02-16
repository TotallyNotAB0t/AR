using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SecretDisplay : MonoBehaviour
{
    private TextMeshPro text;

    void Start()
    {
        text = GetComponent<TextMeshPro>();
    }

    void Update()
    {
        text.text = $"Secrets collected : {LastTreasure.secretActivated} / 3";
    }
}
