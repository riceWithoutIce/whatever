using System;
using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public float m_fSmoothing = 5.0f;
    public float m_fXLimitMin = 80.0f;
    public float m_fXLimitMax = 80.0f;
    public float m_fOffsetXMin = 40.0f;
    public float m_fOffsetXMax = 40.0f;

    private float m_fSensitivity = 0.0f;
    private float m_fRadius = 0.0f;
    private float m_fOffsetX = 0.0f;
    private bool m_bLeftBtnDown = false;                        //鼠标左键按下
    private bool m_bRotate = false;
    private bool m_bMouseLock = false;
    private Camera m_camera = null;
    private GameObject m_objPlayer = null;
    private Transform m_transPlayer = null;
    private Vector3 m_v3Offset = Vector3.zero;
    private Vector3 m_v3OffsetLeftBtnDown = Vector3.zero;       //鼠标左键按下 offset
    private Quaternion m_quaRot = Quaternion.identity;          //鼠标右键 方向键 rot
    private Quaternion m_quaRotRot = Quaternion.identity;       //鼠标左键 rot

    private void Start()
    {
        m_bRotate = false;
        m_camera = GetComponent<Camera>();
        m_objPlayer = GameObject.FindGameObjectWithTag("Player");
        if (m_objPlayer)
        {
            PlayerController playerController = m_objPlayer.GetComponent<PlayerController>();
            if (playerController)
                m_fSensitivity = playerController.m_fXSensitivity;
            m_transPlayer = m_objPlayer.transform;
            m_v3Offset = m_transPlayer.position - transform.position;
            m_fRadius = Vector3.Distance(transform.position, m_transPlayer.position);
        }
        transform.LookAt(m_transPlayer);
    }

    private void LateUpdate()
    {
        MouseInput();
        KeyInput();
        AdaptPosition();
        MouseLock();
    }


    private void KeyInput()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (m_bLeftBtnDown)
            {
                m_bRotate = true;
                m_bMouseLock = true;
                //计算 x轴 上的旋转量
                float fDesiredAngleX = Input.GetAxis("Mouse Y") * m_fSensitivity * Time.deltaTime;
                //计算 y轴 上的旋转量
                float fDesiredAngleY = Input.GetAxis("Mouse X") * m_fSensitivity * Time.deltaTime;

                m_v3OffsetLeftBtnDown.x += fDesiredAngleX;
                m_v3OffsetLeftBtnDown.x = Mathf.Clamp(m_v3OffsetLeftBtnDown.x, m_fOffsetXMin, m_fOffsetXMax);

                m_v3OffsetLeftBtnDown.y += fDesiredAngleY;
                m_quaRotRot = Quaternion.Euler(m_v3OffsetLeftBtnDown);
                transform.position = m_transPlayer.position - (m_quaRotRot * m_v3Offset);

                transform.LookAt(m_transPlayer);
            }
            else
            {
                StartCoroutine(BackToDefault());
                if (!m_bRotate)
                {
                    transform.position = m_transPlayer.position - (m_quaRot * m_v3Offset);
                    transform.LookAt(m_transPlayer);
                }
            }
        }
    }

    private void MouseInput()
    {
        if (!Input.GetKey(KeyCode.Mouse1) && !Input.GetKey(KeyCode.Mouse0))
            m_bMouseLock = false;

        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (Input.GetKey(KeyCode.Mouse0))
                return;
            m_bMouseLock = true;  
            LookAt();
        }
        else
        {
            //处理鼠标左键
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                m_v3OffsetLeftBtnDown.x = m_fOffsetX;
                m_v3OffsetLeftBtnDown.y = m_transPlayer.eulerAngles.y;
            }
            if (Input.GetKey(KeyCode.Mouse0))
            {
                m_bLeftBtnDown = true;
                Rotate();
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
                m_bLeftBtnDown = false;
        }


        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (m_camera.fieldOfView <= 100)
                m_camera.fieldOfView += 2;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (m_camera.fieldOfView > 10)
                m_camera.fieldOfView -= 2;
        }
    }

    private void LookAt()
    {
        StartCoroutine(BackToDefault());
        if (!m_bRotate)
        {
            //计算 x轴 上的旋转量
            float fDesiredAngleX = Input.GetAxis("Mouse Y") * m_fSensitivity * Time.deltaTime;
            float fDesiredAngleY = m_transPlayer.eulerAngles.y;

            m_fOffsetX += fDesiredAngleX;
            m_fOffsetX = Mathf.Clamp(m_fOffsetX, m_fOffsetXMin, m_fOffsetXMax);

            m_quaRot = Quaternion.Euler(m_fOffsetX, fDesiredAngleY, 0.0f);
            transform.position = m_transPlayer.position - (m_quaRot * m_v3Offset);

            transform.LookAt(m_transPlayer);
        }
    }

    private void Rotate()
    {
        m_bRotate = true;
        float fYRot = Input.GetAxis("Mouse X") * m_fSensitivity * 2.0f;

        transform.RotateAround(m_transPlayer.position, Vector3.up, fYRot * Time.deltaTime);
        RotateByY();
            
        float fZAngle = transform.eulerAngles.z;
        transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), -fZAngle);
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

    private void AdaptPosition()
    {
        Vector3 v3Target = m_transPlayer.position;
        Vector3 v3Dir = (v3Target - transform.position).normalized;
        float fYAngle = transform.eulerAngles.y;
        v3Target -= fYAngle * v3Dir;

        //Debug.DrawLine(m_transPlayer.position, v3Target, Color.red);

        RaycastHit hit;
        if (Physics.Linecast(m_transPlayer.position, v3Target, out hit))
        {
            string strTag = hit.collider.gameObject.tag;
            if (strTag != "MainCamera")
            {
                Vector3 v3Point = hit.point;
                if (m_fRadius < hit.distance)
                    v3Point = transform.position;
                transform.position = v3Point;
            }
        }
    }

    private void RotateByY()
    {
        float fXRot = Input.GetAxis("Mouse Y") * m_fSensitivity * 2.0f;
        float fXAngle = transform.eulerAngles.x;
        Vector3 v3MoveHorizontal = Vector3.Cross(transform.up, transform.forward);

        if (fXAngle < 360.0f - m_fXLimitMin && fXAngle > m_fXLimitMax)
        {
            if (fXAngle > 180 && fXRot < 0)
                return;
            if (fXAngle < 180 && fXRot > 0)
                return;
        }

        v3MoveHorizontal = v3MoveHorizontal / v3MoveHorizontal.magnitude;
        transform.RotateAround(m_transPlayer.position, v3MoveHorizontal, fXRot * Time.deltaTime);
    }

    private void MouseLock()
    {
        if (m_bMouseLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
