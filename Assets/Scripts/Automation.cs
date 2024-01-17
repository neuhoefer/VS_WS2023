using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using VehiclePhysics.Specialized;

public class Automation : MonoBehaviour
{
    [SerializeField] private GameObject m_destination;
    [SerializeField] private NavMeshAgent m_agent;
    [SerializeField] private VPHydraulicTrackedVehicleController m_vehicle;

    [SerializeField] private PIDController m_rotationController;
    [SerializeField] private PIDController m_distanceController;
    [SerializeField] private float m_distanceTolerance;

    private TextMeshProUGUI m_label = null;
    private bool m_followingActive = false;
    
    void Start()
    {
        m_label = GetComponentInChildren<TextMeshProUGUI>();
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        if(m_label.color == Color.white)
        {
            m_agent.transform.position = m_vehicle.transform.position;
            m_agent.destination = m_destination.transform.position;
            m_label.color = new Color(255, 190, 0);
            m_followingActive = true;
        } 
        else if (m_followingActive)
        {
            m_agent.destination = m_agent.transform.position;
            m_label.color = Color.white;
            m_followingActive = false;
        }
    }

    private void FixedUpdate()
    {
        if (m_followingActive)
        {
            var vehiclePosition = m_vehicle.transform.position;
            var targetPosition = m_agent.transform.position;
            targetPosition.y = vehiclePosition.y;
            var forwardDirection = m_vehicle.transform.rotation * Vector3.forward;
            var targetDirection = (targetPosition - vehiclePosition).normalized;
            var currentAngle = Vector3.SignedAngle(Vector3.forward, forwardDirection, Vector3.up);
            var targetAngle = Vector3.SignedAngle(Vector3.forward, targetDirection, Vector3.up);
            var currentDistance = (targetPosition - vehiclePosition).magnitude;

            float rotationalInput = m_rotationController.UpdateAngle(Time.fixedDeltaTime, currentAngle, targetAngle);
            float translationalInput = m_distanceController.Update(Time.fixedDeltaTime, currentDistance, 0.0f);

            if(currentDistance > m_distanceTolerance)
            {
                m_vehicle.leftTrackInput = rotationalInput + Mathf.Abs(translationalInput);
                m_vehicle.rightTrackInput = -rotationalInput + Mathf.Abs(translationalInput);
            }
        }
    }

    public void setAgentDestination(Vector3 pos)
    {
        m_destination.SetActive(true);
        m_destination.transform.position = new Vector3(pos.x, m_destination.transform.position.y, pos.z);

        if(m_followingActive)
        {
            m_agent.destination = m_destination.transform.position;
        } else
        {
            m_label.color = Color.white;
        }
    }
}
