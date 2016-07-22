using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float m_fSpeed = 4.0f;
    public float m_fXSensitivity = 2.0f;
    public float m_fYSensitivity = 2.0f;
    public float m_fSmoothTime = 5.0f;

    private bool m_bLockMouse = false;
    private Vector3 m_v3Offset = Vector3.zero;
    private Quaternion m_quaTargetRot = Quaternion.identity;
    private Quaternion m_quaCameraRot = Quaternion.identity;
    private Transform m_transCurr = null;
    private Transform m_transCamera = null;
    private Rigidbody m_rigidbody = null;
    private Camera m_camera;

    private void Start()
    {
        m_quaTargetRot = transform.localRotation;
        m_rigidbody = GetComponent<Rigidbody>();
        m_transCurr = gameObject.transform;
        m_camera = Camera.main;
        if (m_camera)
        {
            m_transCamera = m_camera.transform;
            m_quaCameraRot = m_transCamera.localRotation;
        }
            
        m_v3Offset = m_transCurr.position - m_transCamera.position;
    }

    private void FixedUpdate()
    {
        KeyInput();
        MouseInput();
        //m_transCamera.position = m_transCurr.transform.position - m_v3Offset;
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private void KeyInput()
    {
        float fMoveHorizontal = Input.GetAxis("Horizontal");
        float fMoveVertical = Input.GetAxis("Vertical");

        Vector3 v3Movement = new Vector3(fMoveHorizontal, 0.0f, fMoveVertical);
        m_rigidbody.velocity = v3Movement * m_fSpeed;

        m_rigidbody.position = new Vector3(m_rigidbody.position.x, m_rigidbody.position.y, m_rigidbody.position.z);
    }

    private void MouseInput()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Rotate();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (m_camera.fieldOfView <= 100)
                m_camera.fieldOfView += 2;
            if (m_camera.orthographicSize <= 20)
                m_camera.orthographicSize += 0.5F;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (m_camera.fieldOfView > 2)
                m_camera.fieldOfView -= 2;
            if (m_camera.orthographicSize >= 1)
                m_camera.orthographicSize -= 0.5F;   
        }
    }

    private void Rotate()
    {
        float fYRot = Input.GetAxis("Mouse X") * m_fXSensitivity;
        m_quaTargetRot *= Quaternion.Euler(0.0f, fYRot, 0.0f);
        m_transCurr.localRotation = Quaternion.Slerp(m_transCurr.localRotation, m_quaTargetRot, m_fSmoothTime * Time.deltaTime);

        m_camera.transform.RotateAround(m_transCurr.position, Vector3.up, fYRot);
    }
}
