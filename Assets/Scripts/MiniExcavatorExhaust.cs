using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehiclePhysics.Specialized;

public class MiniExcavatorExhaust : MonoBehaviour
{
    [SerializeField] private VPHydraulicTrackedVehicleControllerInput m_vehicleControllerInput;
    [SerializeField] private float m_minAlpha = 30.0f;
    [SerializeField] private float m_maxAlpha = 255.0f;

    private ParticleSystem m_particleSystem;

    private void Start()
    {
        m_particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if(m_vehicleControllerInput.ignitionKey == -1)
        {
            m_particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        else
        {
            if (m_particleSystem.isStopped)
            {
                m_particleSystem.Play(true);
            }

            float alpha = m_minAlpha + (m_maxAlpha - m_minAlpha) * m_vehicleControllerInput.idleControlInput;

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
}
