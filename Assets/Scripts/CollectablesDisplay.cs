using TMPro;
using UnityEngine;

public class CollectablesDisplay : MonoBehaviour
{
    private TextMeshPro _text;
    public static int CollectedItemsNumber;
    private Transform _cameraTransform;

    private void Start()
    {
        _text = GetComponent<TextMeshPro>();
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        _text.text = $"Items collected : {CollectedItemsNumber} / 6";
        transform.parent.LookAt(_cameraTransform);
    }
}
