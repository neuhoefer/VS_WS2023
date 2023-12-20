using UnityEngine;
using UnityEngine.UI;

public class ModeButton : MonoBehaviour
{
    [SerializeField] private GameObject m_droneView;

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        m_droneView.SetActive(!m_droneView.activeSelf);
    }
}
