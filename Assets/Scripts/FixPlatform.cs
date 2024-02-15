using UnityEngine;

public class FixPlatform : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}
