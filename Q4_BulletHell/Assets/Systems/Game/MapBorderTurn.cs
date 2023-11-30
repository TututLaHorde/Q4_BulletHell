using UnityEngine;

public class MapBorderTurn : MonoBehaviour
{
    [SerializeField] private float m_rotationSpeed;

    private void FixedUpdate()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.z += m_rotationSpeed * Time.deltaTime;
        rotation.z %= 360f;
        transform.rotation = Quaternion.Euler(rotation);
    }
}
