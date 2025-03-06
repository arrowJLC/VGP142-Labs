using UnityEngine;

public class RotatePond : MonoBehaviour
{
    [SerializeField] private int rotationSpeed = 50;
    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
