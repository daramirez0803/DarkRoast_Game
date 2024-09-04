// Animates the position in an arc between sunrise and sunset.

using UnityEngine;

public class MovingLight : MonoBehaviour
{
    public float rotationSpeed = 20.0f;

    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}