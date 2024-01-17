using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DroneView : MonoBehaviour
{
    [SerializeField] private Camera m_droneCamera;
    [SerializeField] private Automation m_automation;

    private RectTransform m_rectTransform = null;
    private RawImage m_rawImage = null;
    private Vector2 m_droneViewMousePos = new Vector2();
    
    void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_rawImage = GetComponent<RawImage>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rectTransform, Input.mousePosition, null, out m_droneViewMousePos);

            //print("Rect coordinates " + m_droneViewMousePos.x + " || " + m_droneViewMousePos.y);

            m_droneViewMousePos.x = (m_droneViewMousePos.x / m_rectTransform.rect.width) + m_rectTransform.pivot.x;
            m_droneViewMousePos.y = (m_droneViewMousePos.y / m_rectTransform.rect.height) + m_rectTransform.pivot.y;

            m_droneViewMousePos.x += m_rawImage.uvRect.x;
            m_droneViewMousePos.x *= m_rawImage.uvRect.width;

            //print("Viewport coordinates " + m_droneViewMousePos.x + " || " + m_droneViewMousePos.y);

            if(m_droneViewMousePos.x > 0.0f && m_droneViewMousePos.x < 1.0f && m_droneViewMousePos.y > 0.0f && m_droneViewMousePos.y < 1.0f)
            {
                Ray ray = m_droneCamera.ViewportPointToRay(m_droneViewMousePos);

                Plane plane = new Plane(Vector3.up, Vector3.zero);

                plane.Raycast(ray, out float enterDistance);

                Vector3 hit = ray.GetPoint(enterDistance);

                m_automation.setAgentDestination(hit);
            }
        }
    }
}
