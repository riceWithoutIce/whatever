using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Core : MonoBehaviour
{
    public int m_nHeight = 0;
    public float m_fHeightUnit = 0.0f;
    public float m_fRadiusUnit = 0.0f;
    public GameObject m_objCube = null;
    public Transform m_transParent = null;
    public Slider m_slider = null;
    public Color m_colorFull = Color.green;
    public Color m_colorEmpty = Color.red;

    private float m_fFullHealth = 100.0f;
    private float m_fCurrHealth = 0.0f;

    private void Start()
    {
        m_fCurrHealth = m_fFullHealth;
        if (m_nHeight < 0 || m_fHeightUnit < 0)
            return;
        Init();
    }

    private void Init()
    {
        float m_fRadius = 0.0f;
        for (int i = 0; i < m_nHeight; i++)
        {
            if (i <= m_nHeight / 2)
                m_fRadius = m_fRadiusUnit * i;
            else
                m_fRadius -= m_fRadiusUnit;
            GameObject m_obj = Instantiate(m_objCube) as GameObject;
            m_obj.transform.localPosition = new Vector3(0.0f, m_fHeightUnit * i + 0.5f, 0.0f);
            Quaternion rotation = new Quaternion();
            rotation.eulerAngles = new Vector3(45.0f, 45.0f, 45.0f);
            m_obj.transform.rotation = rotation;
            m_obj.transform.SetParent(m_transParent);
            if (m_fRadius == 0)
                continue;
            else
            {
                int nCount = (int)Mathf.Floor(Mathf.PI * m_fRadius * 2.0f / 1.0f);
                float fRadiansUnit = Mathf.PI * 2 / nCount;
                float fRadians = 0.0f;
                rotation.eulerAngles = new Vector3(30.0f * i, 30.0f * i, 30.0f * i);
                for (int j = 0; j < nCount; j++)
                {
                    fRadians += fRadiansUnit;
                    GameObject m_objTmp = Instantiate(m_objCube) as GameObject;
                    m_objTmp.transform.localPosition = new Vector3(m_fRadius * Mathf.Cos(fRadians), m_fHeightUnit * i + 0.5f, m_fRadius * Mathf.Sin(fRadians));
                    m_objTmp.transform.rotation = rotation;
                    if (i == m_nHeight / 2)
                    {
                        CubeCore cubeCore = m_objTmp.GetComponent<CubeCore>();
                        if (cubeCore)
                        {
                            cubeCore.fRadius = m_fRadius;
                            cubeCore.bRotate = true;
                            cubeCore.fRotateTime = 240.0f;
                            cubeCore.fAngle = fRadians;
                        }
                    }
                    m_objTmp.transform.SetParent(m_transParent);
                }
            }
        }
    }

    private void SetHealthUI()
    {
        m_slider.value = m_fCurrHealth;
        m_slider.GetComponent<Image>().color = Color.Lerp(m_colorEmpty, m_colorFull, m_fCurrHealth / m_fFullHealth);
    }
}
