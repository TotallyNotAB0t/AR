using UnityEngine;

public class ActivateEscalator : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        RockEscalator.Activated = true;
    }
}
