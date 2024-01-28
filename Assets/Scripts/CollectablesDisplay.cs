using TMPro;
using UnityEngine;

public class CollectablesDisplay : MonoBehaviour
{
    private TextMeshPro _text;
    public static int CollectedItemsNumber;
    private Transform _cameraTransform;
    private GameObject[] Supports = new GameObject[3];

    private void Start()
    {
        var gos = GameObject.FindGameObjectsWithTag("Secret");
        for (int i = 0; i < 3; i++)
        {
            Supports[i] = gos[i].transform.parent.gameObject;
        }

        foreach (var secret in Supports)
        {
            secret.SetActive(false);
        }
        _text = GetComponent<TextMeshPro>();
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        _text.text = $"Items collected : {CollectedItemsNumber} / 6";
        transform.parent.LookAt(_cameraTransform);
        if (CollectedItemsNumber > 4)
        {
            foreach (var secret in Supports)
            {
                secret.SetActive(true);
            }
        }
    }
}
