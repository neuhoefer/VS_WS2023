using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using VehiclePhysics.Specialized;

public class IdleControlInput : MonoBehaviour
{
    [SerializeField] private VPHydraulicTrackedVehicleControllerInput m_vehicleControllerInput;
    [SerializeField] private ParticleSystem m_particleSystem;

    private Slider m_slider;
    private float m_idleControlInput;

    void Start()
    {
        m_slider = gameObject.GetComponent<Slider>();
        m_slider.onValueChanged.AddListener(TaskOnValueChanged);
        m_idleControlInput = m_vehicleControllerInput.idleControlInput;
        setParticleSystem();
    }

    private void TaskOnValueChanged(float value)
    {
        m_idleControlInput = value;
        m_vehicleControllerInput.idleControlInput = m_idleControlInput;
        setParticleSystem();
    }

    private void setParticleSystem()
    {
        float minAlpha = 30.0f;
        float maxAlpha = 170.0f;
        float alpha = minAlpha + (maxAlpha - minAlpha) * m_vehicleControllerInput.idleControlInput;

        GradientColorKey[] colorKeys = new GradientColorKey[2];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = alpha / 255.0f;
        alphaKeys[0].time = 0f;
        colorKeys[0].color = Color.black;
        colorKeys[0].time = 0f;
        alphaKeys[1].alpha = 0f;
        alphaKeys[1].time = 1f;
        colorKeys[1].color = Color.gray;
        colorKeys[1].time = 1f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(colorKeys, alphaKeys);
        ParticleSystem.ColorOverLifetimeModule colorOverLifetime = m_particleSystem.colorOverLifetime;
        colorOverLifetime.color = gradient;
    }
}
