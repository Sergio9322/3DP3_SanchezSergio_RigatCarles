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
	[SerializeField] float otherAngleWeight = 0.8f;


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
		{
			m_HorizontalAngles.Add(m_sideRaysAngle * i);
		}

		for (int i = 0; i < m_TopDownRays; i++)
		{
			m_VerticalAngles.Add(m_TopDownAngle * i);
		}
	}


	public Vector3 GetDesiredPosition(Vector3 l_Direction, float l_Distance, Vector3 l_DesiredPosition, Transform l_lookAt)
	{
		// TODOOOOOO: canvi: central, fer tots pels angles, i si angle < central aquell, sinó mitjana tots.
/* ---------------------------------
		// Check central ray
		Vector3 l_CentralDesiredPosition = GetCentralDesiredPosition(l_Direction, l_DesiredPosition);
		if (l_CentralDesiredPosition != Vector3.zero) return l_CentralDesiredPosition;

		Vector3 List<Vector3> l_SideDesiredPositions = GetSideDesiredPositions(l_Direction, l_DesiredPosition);
		Vector3 l_DesiredPosition = GetAverageDesiredPosition(l_CentralDesiredPosition, l_SideDesiredPositions);
--------------------------------*/

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
		{
			l_AveragePosition += l_DesiredPosition;
		}

		l_AveragePosition /= l_DesiredPositions.Count;

		return l_AveragePosition;
	}

/*
		//RaycastHit l_RaycastHit;
		//Ray l_Ray = new Ray(m_LookAt.position, -l_Direction);
		//if (Physics.Raycast(l_Ray, out l_RaycastHit, l_Distance, layerMask))  // CENTRAL RAY?
		//{
		//	return l_RaycastHit.point + l_Direction * offsetCollision;
		//}
		Vector3 new_DesiredPosition = Vector3.zero;

		List<Vector3> rightDesiredPositions = getDesiredPositions(rightAngles, l_Direction, l_DesiredPosition);
		List<Vector3> leftDesiredPositions = getDesiredPositions(leftAngles, l_Direction, l_DesiredPosition);

		if (rightDesiredPositions.Count == 0 && leftDesiredPositions.Count == 0)
			return new_DesiredPosition;
			
		// do average
		Vector3 rightAverage = GetAverage(rightDesiredPositions);
		Vector3 leftAverage = GetAverage(leftDesiredPositions);
		// check which one is larger
		Vector3 average = (rightAverage.magnitude > leftAverage.magnitude) ? rightAverage : leftAverage;
		// interpolate
		new_DesiredPosition = Vector3.Lerp(l_DesiredPosition, average, otherAngleWeight);
		return new_DesiredPosition;
	}

	// getAverage
	Vector3 GetAverage(List<Vector3> desiredPositions)
	{
		Vector3 average = Vector3.zero;
		foreach (Vector3 desiredPosition in desiredPositions)
		{
			average += desiredPosition;
		}
		average /= desiredPositions.Count;
		return average;
	}

	List<Vector3> GetDesiredPositions(List<float> angles, Vector3 l_Direction, Vector3 l_DesiredPosition)
	{
		RaycastHit l_RaycastHit;
		Ray l_Ray = new Ray(m_LookAt.position, -l_Direction);

		List<Vector3> desiredPositions = new List<Vector3>();
		foreach (float angleRay in rightAngles)
		{
			l_Ray = new Ray (m_LookAt.position, Quaternion.AngleAxis(angleRay, Vector3.up) * -l_Direction);
			if (Physics.Raycast(l_Ray, out l_RaycastHit, collisionAngleDistance, layerMask))
			{
				desiredPositions.Add(Vector3.Lerp(l_DesiredPosition, l_RaycastHit.point + l_Direction * offsetCollision, otherAngleWeight));
			}
		}
		Debug.Log(desiredPositions.Count);
		return desiredPositions;
	}

*/
	Vector3 GetSingleDesiredPosition(float l_HorizontalAngle, float l_VerticalAngle)
	{
		RaycastHit l_RaycastHit;
		Ray l_Ray = new Ray(m_LookAt.position, Quaternion.AngleAxis(l_HorizontalAngle, Vector3.up) * Quaternion.AngleAxis(l_VerticalAngle, Vector3.right) * -m_Direction);
		if (Physics.Raycast(l_Ray, out l_RaycastHit, m_Distance, layerMask))
		{
			return l_RaycastHit.point + m_Direction * m_OffsetCollision;
		}
		return Vector3.zero;
	}

	private void OnDrawGizmos() {
		foreach (float l_VerticalAngle in m_VerticalAngles)
		{
			foreach (float l_HorizontalAngle in m_HorizontalAngles)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawRay(m_LookAt.position, Quaternion.AngleAxis(l_HorizontalAngle, Vector3.up) * Quaternion.AngleAxis(l_VerticalAngle, Vector3.right) * -m_Direction * collisionAngleDistance);
			}
		}
	}
}
