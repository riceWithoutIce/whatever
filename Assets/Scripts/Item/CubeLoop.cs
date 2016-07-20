using UnityEngine;
using System.Collections;

public class CubeLoop : MonoBehaviour
{
    public float m_fForce = 0.0f;
    public float m_fTumble = 0.0f;
    private Rigidbody m_rigidbody = null;

    CubeLoop()
    {
        m_fForce = 0.0f;
        m_fTumble = 0.0f;
        m_rigidbody = null;
    }

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.angularVelocity = Random.insideUnitSphere * m_fTumble;
    }

    void FixedUpdate()
    {
        m_rigidbody.AddForce(Vector3.up * m_fForce);
    }
}
