using UnityEngine;
using System.Collections;

public class CubeLoop : MonoBehaviour
{
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


    public float m_fForce;
    public float m_fTumble;
    private Rigidbody m_rigidbody;
}
