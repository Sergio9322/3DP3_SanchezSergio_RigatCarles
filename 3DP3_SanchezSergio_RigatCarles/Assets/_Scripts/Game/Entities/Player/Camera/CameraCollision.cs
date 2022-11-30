using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    [Header("Collision")]
	[SerializeField] LayerMask layerMask;
	[SerializeField] float offsetCollision;


	int m_sideRaysCount = 3;
	int m_sideRaysAngle = 15;
	int m_TopDownRays = 3;
	int m_TopDownAngle = 15;
	[SerializeField] float otherAngleWeight = 0.8f;
	[SerializeField] float collisionAngleDistance = 10f;




	public Vector3 GetDesiredPosition(Vector3 l_Direction, float l_Distance, Vector3 l_DesiredPosition)
	{
		// TODOOOOOO: canvi: central, fer tots pels angles, i si angle < central aquell, sinó mitjana tots.
/* ---------------------------------
		// Check central ray
		Vector3 l_CentralDesiredPosition = GetCentralDesiredPosition(l_Direction, l_DesiredPosition);
		if (l_CentralDesiredPosition != Vector3.zero) return l_CentralDesiredPosition;

		Vector3 List<Vector3> l_SideDesiredPositions = GetSideDesiredPositions(l_Direction, l_DesiredPosition);
		Vector3 l_DesiredPosition = GetAverageDesiredPosition(l_CentralDesiredPosition, l_SideDesiredPositions);
--------------------------------*/




		return Vector3.zero;


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


	private void OnDrawGizmos() {
		foreach (float angleRay in rightAngles)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawRay(m_LookAt.position, Quaternion.AngleAxis(angleRay, Vector3.up) * -l_Direction * collisionAngleDistance);
		}

		foreach (float angleRay in leftAngles)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawRay(m_LookAt.position, Quaternion.AngleAxis(angleRay, Vector3.up) * -l_Direction * collisionAngleDistance);
		}
	}
*/
}
