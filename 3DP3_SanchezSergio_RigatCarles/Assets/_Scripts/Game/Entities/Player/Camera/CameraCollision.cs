using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    [Header("Collision")]
	[SerializeField] LayerMask layerMask;
	[SerializeField] float m_OffsetCollision;


	[SerializeField] int m_sideRaysCount = 3;
	[SerializeField] int m_sideRaysAngle = 15;
	[SerializeField] int m_TopDownRays = 3;
	[SerializeField] int m_TopDownAngle = 15;
	[SerializeField] float m_OtherAngleWeight = 0.8f;


	List<float> m_HorizontalAngles = new List<float>();
	List<float> m_VerticalAngles = new List<float>();

	Transform m_LookAt;
	Vector3 m_Direction;
	float m_Distance;
	Vector3 m_DesiredPosition;

	private void Start() 
	{
		InitAngles();	
	}

	private void InitAngles()
	{
		m_HorizontalAngles.Clear();
		m_VerticalAngles.Clear();

		for (int i = 0; i < m_sideRaysCount; i++)
			m_HorizontalAngles.Add(m_sideRaysAngle * i);

		for (int i = 0; i < m_TopDownRays; i++)
			m_VerticalAngles.Add(m_TopDownAngle * i);
	}


	public Vector3 GetDesiredPosition(Vector3 l_Direction, float l_Distance, Vector3 l_DesiredPosition, Transform l_lookAt)
	{
		this.m_LookAt = l_lookAt;
		this.m_Direction = l_Direction;
		this.m_Distance = l_Distance;
		this.m_DesiredPosition = l_DesiredPosition;

		Vector3 l_CentralDesiredPosition = GetCentralDesiredPosition();
		if (l_CentralDesiredPosition != Vector3.zero) return l_CentralDesiredPosition;
		
		List<Vector3> l_DesiredPositions = GetCircumferenceDesiredPositions();
		return GetAveragePosition(l_DesiredPositions);
	}

	Vector3 GetCentralDesiredPosition()
	{
		return GetSingleDesiredPosition(0f, 0f);
	}

	List<Vector3> GetCircumferenceDesiredPositions()
	{
		List<Vector3> l_DesiredPositions = new List<Vector3>();
		foreach (float l_HorizontalAngle in m_HorizontalAngles)
		{
			foreach (float l_VerticalAngle in m_VerticalAngles)
			{
				Vector3 l_SingleDesiredPosition = GetSingleDesiredPosition(l_HorizontalAngle, l_VerticalAngle);
				if (l_SingleDesiredPosition != Vector3.zero) l_DesiredPositions.Add(l_SingleDesiredPosition);
			}
		}
		return l_DesiredPositions;
	}

	Vector3 GetAveragePosition(List<Vector3> l_DesiredPositions)
	{
		Vector3 l_AveragePosition = Vector3.zero;
		foreach (Vector3 l_DesiredPosition in l_DesiredPositions)
			l_AveragePosition += l_DesiredPosition;

		int l_Count = l_DesiredPositions.Count == 0 ? 1 : l_DesiredPositions.Count;
		return l_AveragePosition / l_Count;
	}

	Vector3 GetSingleDesiredPosition(float l_HorizontalAngle, float l_VerticalAngle)
	{
		RaycastHit l_RaycastHit;
		Ray l_Ray = new Ray(m_LookAt.position, Quaternion.AngleAxis(l_HorizontalAngle, Vector3.up) * Quaternion.AngleAxis(l_VerticalAngle, Vector3.right) * -m_Direction);
		if (Physics.Raycast(l_Ray, out l_RaycastHit, m_Distance, layerMask))
		{
			return Vector3.Lerp(m_DesiredPosition, l_RaycastHit.point + m_Direction * m_OffsetCollision, m_OtherAngleWeight);
		}
		return Vector3.zero;
	}

	private void OnDrawGizmos() {
		foreach (float l_VerticalAngle in m_VerticalAngles)
		{
			foreach (float l_HorizontalAngle in m_HorizontalAngles)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawRay(m_LookAt.position, Quaternion.AngleAxis(l_HorizontalAngle, Vector3.up) * Quaternion.AngleAxis(l_VerticalAngle, Vector3.right) * -m_Direction * m_Distance);
			}
		}
	}
}