using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float m_fSpeed;
    private Rigidbody m_rigidbody;

    public void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        m_rigidbody.AddForce(movement * m_fSpeed);
    }

    void OnTriggerEnter(Collider other)
    {

    }
}
