using System;
using UnityEngine;

public class InstantiateProjectile : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    private void Start()
    {
        InvokeRepeating(nameof(InstantiateProj), 0, 5);
    }

    private void InstantiateProj()
    {
        Instantiate(projectile, transform.position, transform.rotation);
    }
}
