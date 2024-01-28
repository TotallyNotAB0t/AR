using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockEscalator : MonoBehaviour
{
    [SerializeField] private Transform up;
    [SerializeField] private Transform floor;
    [SerializeField] private float speed = 1.0f;
    private Transform target;
    public static bool Activated;

    private void Start()
    {
        target = up;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Activated) return;
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        if (Vector3.Distance(transform.position, target.position) < 0.001f)
        {
            target = target == up ? floor : up;
        }
    }
}
