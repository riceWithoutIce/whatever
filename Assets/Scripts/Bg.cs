using UnityEngine;
using System.Collections;

public class Bg : MonoBehaviour
{
    public float m_fMinX = 0.0f;
    public float m_fMaxX = 0.0f;
    public float m_fDelay = 0.0f;
    public GameObject m_objCube = null;

    Bg()
    {
        m_fMinX = 0.0f;
        m_fMaxX = 0.0f;
        m_fDelay = 0.0f;
        m_objCube = null;
    }

    void Start()
    {
        StartCoroutine(CubeLoop());
    }

    IEnumerator CubeLoop()
    {
        float fX = 0.0f;
        Quaternion qua = Quaternion.identity;
        Vector3 v3Pos = Vector3.zero;
        while (true)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3 - i; j++)
                {
                    fX = Random.Range(m_fMinX, m_fMaxX);
                    v3Pos = new Vector3(fX, -5.0f, j * 3.0f);
                    Instantiate(m_objCube, v3Pos, qua);
                }
            }
            yield return new WaitForSeconds(m_fDelay);
        }
    }
}
