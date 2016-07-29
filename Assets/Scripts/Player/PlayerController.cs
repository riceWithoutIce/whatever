using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float m_fSpeed = 4.0f;
    public float m_fSpeedBack = 3.0f;
    public float m_fXSensitivity = 2.0f;
    public float m_fSmoothTime = 5.0f;

    private float m_fTurnInputValue;
    private Vector3 m_v3Offset = Vector3.zero;
    private Transform m_transCurr = null;
    private Rigidbody m_rigidbody = null;
    private Animator m_anim = null;
    private Global.ePlayerState m_playerState = Global.ePlayerState.eSTATUS_IDLE;
    private Global.ePlayerState m_playerStateNew = Global.ePlayerState.eSTATUS_IDLE;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_transCurr = gameObject.transform;
        m_anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        m_fTurnInputValue = 0.0f;
    }

    private void FixedUpdate()
    {
        float fMoveHorizontal = Input.GetAxis("Horizontal");
        float fMoveVertical = Input.GetAxis("Vertical");
        KeyInput(fMoveHorizontal, fMoveVertical);
        MouseInput();
        Animating(fMoveHorizontal, fMoveVertical);
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private void KeyInput(float fH, float fV)
    {
        if (fH != 0 || fV != 0)
        {
            Vector3 v3MoveVertical = Vector3.zero;
            if (fV >= 0)
                v3MoveVertical = transform.forward * fV * m_fSpeed * Time.deltaTime;
            else
                v3MoveVertical = transform.forward * fV * m_fSpeedBack * Time.deltaTime;
            Vector3 v3MoveHorizontal = Vector3.Cross(transform.up, transform.forward);
            v3MoveHorizontal = v3MoveHorizontal / v3MoveHorizontal.magnitude;
            v3MoveHorizontal = v3MoveHorizontal * fH * m_fSpeed * Time.deltaTime;
            Vector3 v3Move = v3MoveHorizontal + v3MoveVertical;
            m_rigidbody.MovePosition(m_rigidbody.position + v3Move);
        }
    }

    private void MouseInput()
    {
        if (Input.GetKey(KeyCode.Mouse1))
            Rotate();
    }

    private void Rotate()
    {
        m_fTurnInputValue = Input.GetAxis("Mouse X");
        float fTurn = m_fTurnInputValue * m_fXSensitivity * Time.deltaTime;
        Quaternion quaRot = Quaternion.Euler(0.0f, fTurn, 0.0f);
        m_rigidbody.MoveRotation(m_rigidbody.rotation * quaRot);
    }

    private void Animating(float fH, float fV)
    {
        if (fH * fV == 0)
        {
            if (fH == 0 && fV == 0 && m_playerState != Global.ePlayerState.eSTATUS_IDLE)
            {
                m_playerStateNew = Global.ePlayerState.eSTATUS_IDLE;
            }
            else if (fV > 0 && m_playerState != Global.ePlayerState.eSTATUS_MOVE_FRONT)
            {
                m_playerStateNew = Global.ePlayerState.eSTATUS_MOVE_FRONT;
            }
            else if (fV < 0 && m_playerState != Global.ePlayerState.eSTATUS_MOVE_BACK)
            {
                m_playerStateNew = Global.ePlayerState.eSTATUS_MOVE_BACK;
            }
            else if (fH > 0 && m_playerState != Global.ePlayerState.eSTATUS_MOVE_RIGHT)
            {
                m_playerStateNew = Global.ePlayerState.eSTATUS_MOVE_RIGHT;
            }
            else if (fH < 0 && m_playerState != Global.ePlayerState.eSTATUS_MOVE_LEFT)
            {
                m_playerStateNew = Global.ePlayerState.eSTATUS_MOVE_LEFT;
            }
        }
        
        if (m_playerStateNew != m_playerState)
        {
            m_playerState = m_playerStateNew;
            m_anim.SetInteger("PlayerState", (int)m_playerState);
        }
    }
}
