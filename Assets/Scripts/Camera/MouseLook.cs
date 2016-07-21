using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class MouseLook
{
    public bool m_bClampVerticalRotation = true;
    public bool m_bSmooth = true;
    public bool m_bLockCursor = true;
    public float m_fXSensitivity = 2.0f;
    public float m_fYSensitivity = 2.0f;
    public float m_fMinX = -90.0f;
    public float m_fMaxX = 90.0f;
    public float m_fSmoothTime = 5.0f;

    private Quaternion m_quaCharacterTargetRot;
    private Quaternion m_quaCameraTargetRot;
    private bool m_bCursorIsLocked = true;

    public void Init(Transform transCharacter, Transform transCamera)
    {
        m_quaCharacterTargetRot = transCharacter.localRotation;
        m_quaCameraTargetRot = transCamera.localRotation;
    }

    public void LookRotation(Transform transCharacter, Transform transCamera)
    {
        float fYRot = Input.GetAxis("Mouse X") * m_fXSensitivity;
        float fXRot = Input.GetAxis("Mouse Y") * m_fYSensitivity;

        m_quaCharacterTargetRot *= Quaternion.Euler(0.0f, fYRot, 0.0f);
        m_quaCameraTargetRot *= Quaternion.Euler(-fXRot, 0.0f, 0.0f);

        if (m_bClampVerticalRotation)
            m_quaCameraTargetRot = ClampRotationAroundXAxis(m_quaCameraTargetRot);

        if (m_bSmooth)
        {
            transCharacter.localRotation = Quaternion.Slerp(transCharacter.localRotation, m_quaCharacterTargetRot, m_fSmoothTime * Time.deltaTime);
            transCamera.localRotation = Quaternion.Slerp(transCamera.localRotation, m_quaCameraTargetRot, m_fSmoothTime * Time.deltaTime);
        }
        else
        {
            transCharacter.localRotation = m_quaCharacterTargetRot;
            transCamera.localRotation = m_quaCameraTargetRot;
        }

        UpdateCursorLock();
    }

    public void SetCursorLock(bool bLock)
    {
        m_bLockCursor = bLock;
        if (!m_bLockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {
        if (m_bLockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            m_bCursorIsLocked = false;
        else if (Input.GetMouseButtonUp(0))
            m_bCursorIsLocked = true;

        if (m_bCursorIsLocked)
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

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, m_fMinX, m_fMaxX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}
