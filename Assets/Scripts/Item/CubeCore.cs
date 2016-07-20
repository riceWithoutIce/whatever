using UnityEngine;
using System.Collections;

public class CubeCore : MonoBehaviour
{
    private bool m_bRotate = false;
    private float m_fRotateTime = 0.0f;
    private float m_fAngleUnit = 0.0f;
    private float m_fAngle = 0.0f;
    private float m_fRadius = 0.0f;

    #region getset
    public bool bRotate
    {
        get { return m_bRotate; }
        set { m_bRotate = value; }
    }

    public float fRotateTime
    {
        get { return m_fRotateTime; }
        set { m_fRotateTime = value; }
    }

    public float fAngle
    {
        get { return m_fAngle; }
        set { m_fAngle = value; }
    }

    public float fRadius
    {
        get { return m_fRadius; }
        set { m_fRadius = value; }
    }

    #endregion

    private void Start()
    {
        if (m_bRotate)
        {
            if (m_fRotateTime < 0)
                return;

            m_fAngleUnit = Mathf.PI * 2 / m_fRotateTime;
        }
    }

    private void FixedUpdate()
    {
        if (m_bRotate)
        {
            m_fAngle += m_fAngleUnit;
            if (m_fAngle > Mathf.PI * 2)
                m_fAngle = m_fAngle - Mathf.PI * 2;
            Vector3 v3Pos = new Vector3(m_fRadius * Mathf.Cos(m_fAngle), gameObject.transform.position.y, m_fRadius * Mathf.Sin(m_fAngle));
            gameObject.transform.localPosition = v3Pos;
        }
    }
}
