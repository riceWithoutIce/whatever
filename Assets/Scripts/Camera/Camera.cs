using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour
{
    public float m_fSpeed;
    private Rigidbody m_rigidbody;

    private void Start()
    {
        m_rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float fMoveHorizontal = Input.GetAxis("Horizontal");
        float fMoveVertical = Input.GetAxis("Vertical");

        Vector3 v3Movement = new Vector3(fMoveHorizontal, 0.0f, fMoveVertical);
        m_rigidbody.velocity = v3Movement * m_fSpeed;
    }
}
