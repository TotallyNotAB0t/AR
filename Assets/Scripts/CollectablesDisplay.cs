using TMPro;
using UnityEngine;

public class CollectablesDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshPro _timeText;
    private float timer;
    private TextMeshPro _text;
    public static int CollectedItemsNumber;
    private Transform _cameraTransform;
    private GameObject[] Supports = new GameObject[3];
    private int minutes;
    private int seconds;
    private string betterTime;

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
        if (CollectedItemsNumber < 6)
        {
            timer += Time.deltaTime;
            minutes = Mathf.FloorToInt(timer / 60F);
            seconds = Mathf.FloorToInt(timer - minutes * 60);
        }

        betterTime = $"{minutes:0}:{seconds:00}";
        _timeText.text = betterTime;
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
