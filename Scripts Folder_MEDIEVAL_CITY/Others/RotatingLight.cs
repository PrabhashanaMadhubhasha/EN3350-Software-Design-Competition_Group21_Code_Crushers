using UnityEngine;

public class RotatingLight : MonoBehaviour
{
    public float rotationSpeed = 10.0f;

    void Update() // Rotating the Light house light 
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
