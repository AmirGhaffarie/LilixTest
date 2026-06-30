using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] Vector3 rotationPerSecond;
    [SerializeField] Space space;
    private void Update()
    {
        transform.Rotate(rotationPerSecond * Time.deltaTime, space);
    }
}
