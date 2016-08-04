using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerConstruct : MonoBehaviour
{
    public Image m_imgMarkPre = null;
    public Transform m_transParent = null;

    private bool m_bLandMark = false;
    private int m_nFloorMask = 0;
    private int m_nWidth = 0;
    private float m_fCamRayLength = 100.0f;
    private float m_fInc = 0.0f;
    private Camera m_camera = null;
    private Image m_imgMark = null;
    private Transform m_transMark = null;
    private Global m_global = null;

    private void Start()
    {
        m_nFloorMask = LayerMask.GetMask("Floor");
        m_camera = Camera.main;
        if (m_imgMarkPre != null && m_transParent != null)
        {
            m_imgMark = Instantiate(m_imgMarkPre);
            m_transMark = m_imgMark.transform;
            m_transMark.parent = m_transParent;
            m_nWidth = (int)Mathf.Floor(m_imgMark.rectTransform.rect.width);
        }
        m_global = Global.GetInstance();
        m_bLandMark = m_global.bLandMark;
    }

    private void FixedUpdate()
    {
        CheckMark();
        MouseMove();
    }

    private void MouseMove()
    {
        if (m_global.bLandMark)
        {
            Ray rayCamera = m_camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitFloor;

            if (Physics.Raycast(rayCamera, out hitFloor, m_fCamRayLength, m_nFloorMask))
            {
                Vector3 v3HitPoint = hitFloor.point;
                v3HitPoint.y = 0.2f;
                if (m_imgMark != null)
                {
                    v3HitPoint = CaculatePos(v3HitPoint);
                    m_transMark.position = v3HitPoint;
                }
            }
        }
    }

    private Vector3 CaculatePos(Vector3 v3pos)
    {
        int nX = (int)Mathf.Floor(v3pos.x);
        int nZ = (int)Mathf.Floor(v3pos.z);

        nX = nX - (nX % m_nWidth);
        nZ = nZ - (nZ % m_nWidth);

        return new Vector3(nX, v3pos.y, nZ);
    }

    private void CheckMark()
    {
        if (m_bLandMark == m_global.bLandMark && Mathf.Approximately(m_fInc, 1.0f))
            return;

        if (m_global.bLandMark)
        {
            m_fInc += 1 * Time.deltaTime;
            m_imgMark.color = Color.Lerp(Color.clear, new Color(0.0f, 1.0f, 0.2f, 0.5f), m_fInc);
        }
        else
        {
            m_imgMark.color = Color.clear;
            m_fInc = 0.0f;
        }

        m_bLandMark = m_global.bLandMark;
    }
}
