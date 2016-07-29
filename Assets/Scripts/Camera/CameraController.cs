using System;
using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public float m_fSmoothing = 5.0f;

    private float m_fSensitivity = 0.0f;
    private bool m_bRotate = false;
    private bool m_bMove = false;
    private Camera m_camera = null;
    private GameObject m_objPlayer = null;
    private Transform m_transPlayer = null;
    private Vector3 m_v3Offset = Vector3.zero;
    private Quaternion m_quaRot = Quaternion.identity;

    private void Start()
    {
        m_bRotate = false;
        m_bMove = false;
        m_camera = GetComponent<Camera>();
        m_objPlayer = GameObject.FindGameObjectWithTag("Player");
        if (m_objPlayer)
        {
            PlayerController playerController = m_objPlayer.GetComponent<PlayerController>();
            if (playerController)
                m_fSensitivity = playerController.m_fXSensitivity;
            m_transPlayer = m_objPlayer.transform;
            m_v3Offset = m_transPlayer.position - transform.position;
        }
        transform.LookAt(m_transPlayer);
    }

    private void LateUpdate()
    {
        MouseInput();
        KeyInput();
    }

    private void KeyInput()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            StartCoroutine(BackToDefault());
            if (!m_bRotate)
            {
                m_bMove = true;
                transform.position = m_transPlayer.transform.position - (m_quaRot * m_v3Offset);
                transform.LookAt(m_transPlayer);
            }
        }
        else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            m_bMove = false;
    }

    private void MouseInput()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            LookAt();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (m_bMove)
                return;
            Rotate();
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (m_camera.fieldOfView <= 100)
                m_camera.fieldOfView += 2;
            if (m_camera.orthographicSize <= 20)
                m_camera.orthographicSize += 0.5f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (m_camera.fieldOfView > 2)
                m_camera.fieldOfView -= 2;
            if (m_camera.orthographicSize >= 1)
                m_camera.orthographicSize -= 0.5f;
        }
    }

    private void LookAt()
    {
        StartCoroutine(BackToDefault());
        if (!m_bRotate)
        {
            float fDesiredAngle = m_transPlayer.eulerAngles.y;
            m_quaRot = Quaternion.Euler(0, fDesiredAngle, 0);
            transform.position = m_transPlayer.position - (m_quaRot * m_v3Offset);
            transform.LookAt(m_transPlayer);
        }
    }

    private void Rotate()
    {
        m_bRotate = true;
        float fYRot = Input.GetAxis("Mouse X") * m_fSensitivity * 1.5f;
        transform.RotateAround(m_transPlayer.position, Vector3.up, fYRot * Time.deltaTime);
    }

    private IEnumerator BackToDefault()
    {
        Vector3 v3Pos = m_transPlayer.position - (m_quaRot * m_v3Offset);
        while (m_bRotate)
        {
            transform.position = Vector3.Lerp(transform.position, v3Pos, m_fSmoothing * Time.deltaTime);
            transform.LookAt(m_transPlayer);
            if (Vector3.Distance(transform.position, v3Pos) < 0.5f)
            {
                m_bRotate = false;
                break;
            }
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.0f);
    }
}
