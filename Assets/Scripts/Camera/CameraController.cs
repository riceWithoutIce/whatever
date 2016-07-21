using System;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.Utility;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private MouseLook m_mouseLook;
    [SerializeField]
    private FOVKick m_fovKick = new FOVKick();
    public GameObject m_objPlayer;
    private Vector3 m_v3Offset;

    void Start()
    {
        m_v3Offset = transform.position - m_objPlayer.transform.position;
    }

    void LateUpdate()
    {
        transform.position = m_objPlayer.transform.position + m_v3Offset;
    }
}
