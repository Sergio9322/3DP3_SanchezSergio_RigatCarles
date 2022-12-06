using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraCollision))]
public class CameraController : MonoBehaviour, IRestartGameElement
{
	public Transform m_LookAt;
	public float m_YawRotationalSpeed;
	public float m_PitchRotationalSpeed;
	public float m_MinPitch=-45.0f;
	public float m_MaxPitch=75.0f;

	[SerializeField] float minCamDist= 5;
	[SerializeField] float maxCamDist = 10;
	[SerializeField] float m_InterpolationSpeed = 5f;
	[SerializeField] bool m_InterpolationActive = true;

	Vector3 m_Direction = Vector3.zero;

	float m_StartPitch, m_StartYaw;
	Vector3 m_StartPosition;
	
	[Header("Debug")]
	public KeyCode m_DebugLockAngleKeyCode=KeyCode.I;
	public KeyCode m_DebugLockKeyCode=KeyCode.O;
	bool m_AngleLocked=false;
	bool m_CursorLocked=true;

	[SerializeField] CameraCollision m_CameraCollision;


	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		m_CursorLocked = true;

		m_StartPitch = transform.eulerAngles.x;
		m_StartYaw = transform.eulerAngles.y;
		m_StartPosition = m_LookAt.position - transform.position;

		GameController.GetGameController().AddRestartGameElement(this);
	}

	void OnApplicationFocus()
	{
		if(m_CursorLocked)
			Cursor.lockState=CursorLockMode.Locked;
	}
	void LateUpdate()
	{
        MoveCamera();
#if UNITY_EDITOR
		EditorDebugLock();
#endif
	}

	void MoveCamera()
	{
		float l_MouseAxisX = Input.GetAxis("Mouse X");
        float l_MouseAxisY = Input.GetAxis("Mouse Y");
        m_Direction = m_LookAt.position - transform.position;
        float l_Distance = m_Direction.magnitude;
        Vector3 l_DesiredPosition = transform.position;

		if(!m_AngleLocked)
		{
			ChangeAngle(l_MouseAxisX, l_MouseAxisY, l_DesiredPosition, l_Distance);
		}
		m_Direction/=l_Distance;

		ClampMinAndMaxDistance(ref l_Distance, ref l_DesiredPosition);
		l_DesiredPosition = BringCameraCloserIfColliding(l_Distance, l_DesiredPosition);

		transform.forward = m_Direction;
		transform.position = m_InterpolationActive ? InterpolateNewPosition(l_DesiredPosition) : l_DesiredPosition;
	}

	void ChangeAngle(float l_MouseAxisX, float l_MouseAxisY, Vector3 l_DesiredPosition, float l_Distance)
	{
		Vector3 l_EulerAngles=transform.eulerAngles;
		float l_Yaw=(l_EulerAngles.y+180.0f);
		float l_Pitch=l_EulerAngles.x;
		UpdateYawAndPitch(l_MouseAxisX, l_MouseAxisY, ref l_Yaw, ref l_Pitch);
		l_DesiredPosition = GetUpdatedDesiredPosition(l_Distance, l_Yaw, l_Pitch);
		m_Direction = m_LookAt.position - l_DesiredPosition;
	}

	void UpdateYawAndPitch(float l_MouseAxisX, float l_MouseAxisY, ref float l_Yaw, ref float l_Pitch)
	{
		l_Yaw += l_MouseAxisX * m_YawRotationalSpeed;
		l_Pitch += l_MouseAxisY * m_PitchRotationalSpeed;
		l_Pitch = Mathf.Clamp(l_Pitch, m_MinPitch, m_MaxPitch);
	}

	Vector3 GetUpdatedDesiredPosition(float l_Distance, float l_Yaw, float l_Pitch)
	{
		Vector3 l_DesiredPosition = Vector3.zero;
		l_DesiredPosition.x = m_LookAt.position.x +
			  Mathf.Sin(l_Yaw * Mathf.Deg2Rad) 
			* Mathf.Cos(l_Pitch * Mathf.Deg2Rad) 
			* l_Distance;
		l_DesiredPosition.y = m_LookAt.position.y +
			  Mathf.Sin(l_Pitch * Mathf.Deg2Rad)
			* l_Distance;
		l_DesiredPosition.z = m_LookAt.position.z +
			  Mathf.Cos(l_Yaw * Mathf.Deg2Rad)
			* Mathf.Cos(l_Pitch * Mathf.Deg2Rad)
			* l_Distance;
		return l_DesiredPosition;
	}

	void ClampMinAndMaxDistance(ref float l_Distance, ref Vector3 l_DesiredPosition)
	{
		l_Distance = Mathf.Clamp(l_Distance, minCamDist, maxCamDist);
		l_DesiredPosition = m_LookAt.position - m_Direction * l_Distance;
	}

	Vector3 BringCameraCloserIfColliding(float l_Distance, Vector3 l_DesiredPosition)
	{
		Vector3 l_NewDesiredPosition = m_CameraCollision.GetDesiredPosition(m_Direction, l_Distance, l_DesiredPosition, m_LookAt);
		l_DesiredPosition = (l_NewDesiredPosition == Vector3.zero) ? l_DesiredPosition : l_NewDesiredPosition;
		return l_DesiredPosition;
	}

	Vector3 InterpolateNewPosition(Vector3 l_DesiredPosition)
	{
		return Vector3.Lerp(transform.position, l_DesiredPosition, Time.deltaTime * m_InterpolationSpeed);
	}

	public void RepositionCamera()
	{
		transform.eulerAngles = new Vector3(m_StartPitch, m_StartYaw, 0);
		transform.position = m_LookAt.position - m_StartPosition;
	}


	
	public void RestartGame()
    {
        RepositionCamera();
    }

#if UNITY_EDITOR
	void EditorDebugLock()
	{
		if(Input.GetKeyDown(m_DebugLockAngleKeyCode))
			m_AngleLocked=!m_AngleLocked;
		if(Input.GetKeyDown(m_DebugLockKeyCode))
		{
			if(Cursor.lockState==CursorLockMode.Locked)
				Cursor.lockState=CursorLockMode.None;
			else
				Cursor.lockState=CursorLockMode.Locked;
			m_CursorLocked=Cursor.lockState==CursorLockMode.Locked;
		}
	}
#endif	
}
