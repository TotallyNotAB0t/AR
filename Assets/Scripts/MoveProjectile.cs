using System;
using System.Collections;
using UnityEngine;

public class MoveProjectile : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DestroyAfterTime());
    }

    private void Update()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, transform.forward * 50, Time.deltaTime);
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(20);
        Destroy(gameObject);
    }
}
