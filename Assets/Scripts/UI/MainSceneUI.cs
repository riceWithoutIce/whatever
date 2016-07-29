using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainSceneUI : MonoBehaviour
{
    private bool m_bDisplay = false;
    private Camera m_camera = null;
    private DrawGrid m_drawGrid = null;

    private void Start()
    {
        m_bDisplay = false;
        m_camera = Camera.main;
        if (m_camera)
            m_drawGrid = m_camera.GetComponent<DrawGrid>();
    }

    public void DisplayGrid()
    {
        m_bDisplay = !m_bDisplay;
        m_drawGrid.Display = m_bDisplay;
    }
}
