using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SecretDisplay : MonoBehaviour
{
    private TextMeshPro text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"Secrets collected : {LastTreasure.secretActivated} / 3";
    }
}
