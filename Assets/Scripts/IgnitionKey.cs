using UnityEngine;
using UnityEngine.UI;
using VehiclePhysics.Specialized;

public class IgnitionKey : MonoBehaviour
{
    [SerializeField] private VPHydraulicTrackedVehicleControllerInput m_vehicleControllerInput;
    [SerializeField] private ParticleSystem m_particleSystem;

    private int m_state;

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(TaskOnClick);
        m_state = m_vehicleControllerInput.ignitionKey;
        setRotationAndParticleSystem();
    }

    private void TaskOnClick()
    {
        if(m_state == -1)
        {
            m_state = 0;
        }
        else if(m_state == 0)
        {
            m_state = -1;
        }
        else if (m_state == 1)
        {
            m_state = 0;
        }
        m_vehicleControllerInput.ignitionKey = m_state;
        setRotationAndParticleSystem();
    }

    private void setRotationAndParticleSystem()
    {
        switch(m_state)
        {
            case -1:
                transform.eulerAngles = new Vector3(0, 0, 0);
                m_particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                break;
            case 0:
                transform.eulerAngles = new Vector3(0, 0, -30);
                m_particleSystem.Play(true);
                break;
            case 1:
                transform.eulerAngles = new Vector3(0, 0, -60);
                break;
        }
    }

}
