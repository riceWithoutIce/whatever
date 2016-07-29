using UnityEngine;
using System.Collections;

public class DrawGrid : MonoBehaviour
{
    public Shader m_shader = null;
    public Color m_colorStart = Color.white;
    public Color m_colorEnd = Color.black;

    private bool m_bShowMain = false;
    private int m_nGridSizeX = 30;
    private int m_nGridSizeZ = 30;
    private int m_nSteps = 2;
    private int m_nLerpFlg = 0;
    private float m_fStartX = 0.0f;
    private float m_fStartZ = 0.0f;
    private float m_fLerp = 0.0f;
    private Material m_materialLine = null;
    private Color m_color = new Color(0.6f, 0.6f, 0.6f, 0.6f);
    private Transform m_transPlayer = null;

    #region getset
    public bool Display
    {
        get { return m_bShowMain; }
        set { m_bShowMain = value; }
    }

    #endregion

    private void Start()
    {
        m_nLerpFlg = 1;
        m_fStartX = 0.0f;
        m_fStartZ = 0.0f;
        m_fLerp = 0.0f;
        GameObject objPLayer = GameObject.FindGameObjectWithTag("Player");
        if (objPLayer)
            m_transPlayer = objPLayer.transform;
        CreateLineMaterial();
    }

    private void Update()
    {
        if (m_fLerp >= 1)
            m_nLerpFlg = -1;
        else if (m_fLerp <= 0)
            m_nLerpFlg = 1;
        m_fLerp = m_fLerp + m_nLerpFlg * Time.deltaTime;
    }

    private void CreateLineMaterial()
    {
        if (!m_materialLine)
        {
            m_materialLine = new Material(m_shader);
            m_materialLine.hideFlags = HideFlags.HideAndDontSave;
            m_materialLine.shader.hideFlags = HideFlags.HideAndDontSave;
        }
    }

    private void OnPostRender()
    {
        CaculateStartPoint();
        m_materialLine.SetPass(0);

        GL.Begin(GL.LINES);

        if (m_bShowMain)
        {
            m_color = Color.Lerp(m_colorStart, m_colorEnd, m_fLerp);
            GL.Color(m_color);

            //x axis lines
            for (float j = 0; j <= m_nGridSizeZ; j += m_nSteps)
            {
                GL.Vertex3(m_fStartX - m_nSteps, 0, m_fStartZ + j);
                GL.Vertex3(m_fStartX + m_nGridSizeX + m_nSteps, 0, m_fStartZ + j);
            }
            //z axis lines
            for (float j = 0; j <= m_nGridSizeX; j += m_nSteps)
            {
                GL.Vertex3(m_fStartX + j, 0, m_fStartZ - m_nSteps);
                GL.Vertex3(m_fStartX + j, 0, m_fStartZ + m_nGridSizeZ + m_nSteps);
            }
        }

        GL.End();
    }

    private void CaculateStartPoint()
    {
        float fX = m_transPlayer.position.x;
        float fZ = m_transPlayer.position.z;

        int nX = (int)Mathf.Floor(fX);
        int nZ = (int)Mathf.Floor(fZ);

        nX = nX - (nX % m_nSteps);
        nZ = nZ - (nZ % m_nSteps);

        m_fStartX = nX - m_nGridSizeX / 2;
        m_fStartZ = nZ - m_nGridSizeZ / 2;
    }
}
