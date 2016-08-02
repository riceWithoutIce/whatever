using UnityEngine;
using System.Collections;

public class PlayerConstruct : MonoBehaviour
{
    private int m_nFloorMask = 0;
    private float m_fCamRayLength = 100.0f;
    private Camera m_camera = null;

    private void Start()
    {
        m_nFloorMask = LayerMask.GetMask("Floor");
        m_camera = Camera.main;
    }

    private void FixedUpdate()
    {
        MouseMove();
    }

    private void MouseMove()
    {
        Ray rayCamera = m_camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitFloor;

        if (Physics.Raycast(rayCamera, out hitFloor, m_fCamRayLength, m_nFloorMask))
        {
            Vector3 v3HitPoint = hitFloor.point;
            //Debug.Log("x: " + v3HitPoint.x + "y: " + v3HitPoint.y + "z: " + v3HitPoint.z);
        }
    }
}
