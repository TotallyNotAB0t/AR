using System;
using System.Collections;
using UnityEngine;

public class CollectiblesAnimation : MonoBehaviour
{

    public Vector3 rotationAngle;
    public float rotationSpeed;
    private bool used;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime * rotationAngle);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (used) return;
        used = true;
        GetComponent<AudioSource>().Play();
        CollectablesDisplay.CollectedItemsNumber++;
        StartCoroutine(DestroyDelayed());
    }

    private IEnumerator DestroyDelayed()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
