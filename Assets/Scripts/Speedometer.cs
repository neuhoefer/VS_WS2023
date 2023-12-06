using UnityEngine;
using VehiclePhysics.Specialized;

public class Speedometer : MonoBehaviour
{

    [SerializeField] private VPHydraulicTrackedVehicleController m_vehicleController;
    [SerializeField] private Transform m_needle;

    private float m_speed;
    private const float MAX_SPEED = 3.5f;
    private const float MAX_SPEED_ANGLE = -90.0f;

    void Start()
    {
        m_speed = 0.0f;
    }

    void Update()
    {
        m_speed = m_vehicleController.speed * 3.6f;
        float needleAngle = m_speed / MAX_SPEED * MAX_SPEED_ANGLE;
        m_needle.eulerAngles = new Vector3(0, 0, needleAngle);
    }
}
