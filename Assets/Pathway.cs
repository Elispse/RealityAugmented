using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Pathway : MonoBehaviour
{
	[Header("Path Requirements")]
	[SerializeField] private SplineContainer thePath;
	[SerializeField] private Transform beginning;
	[SerializeField] private Transform ending;
	[SerializeField] private LineRenderer lineRenderer;

	[Header("Path Modification")]
	public bool resetPath = false;
    [SerializeField] private int pathSegments = 1;
	[SerializeField] private float pathVariance = 5.0f;

    void Start()
    {
        thePath = GetComponent<SplineContainer>();
		//ResetSplinePath();
		//StartCoroutine(ResetStuff());
	}

	private IEnumerator ResetStuff()
	{
		resetPath = true;
		yield return new WaitForSeconds(30);
		StartCoroutine(ResetStuff());
	}

	private void Update()
	{
		// if resetPath is true, it resets the spline path
		if (resetPath)
		{
			ResetSplinePath();
			resetPath = false;
		}
	}

	/// <summary>
	/// Gets the pathway
	/// </summary>
	/// <returns>The SplineContainer that contains the current pathway</returns>
	public SplineContainer GetPath()
	{
		return thePath;
	}

	/// <summary>
	/// Sets the Beginning of the pathway
	/// </summary>
	public void SetBeginning(Transform beginning)
	{
		this.beginning = beginning;
	}

	/// <summary>
	/// Sets the Ending of the pathway
	/// </summary>
	public void SetEnding(Transform ending)
	{
		this.ending = ending;
	}

	/// <summary>
	/// Recalculates the Spline Pathway
	/// </summary>
	public void ResetSplinePath()
	{
		thePath.Spline.Clear();

		List<Vector3> linepositions = new List<Vector3>();

		// first add the beginning position to the path
		thePath.Spline.Add(new BezierKnot(beginning.position), TangentMode.AutoSmooth);
		linepositions.Add(beginning.position);



		// I got this section of code from https://discussions.unity.com/t/perpendicular-to-a-3d-direction-vector/184973/3
		// It gets a vector perpendicular to the pathway direction
		Vector3 dir = ending.position - beginning.position;
		Vector3 left = Vector3.Cross(dir, Vector3.up).normalized;
		// for every path segment between the beginning and end, offset it perpendicular to the direction of the path
		for (int i = 1; i < pathSegments; i++)
		{
			float distanceAlongPath = ((float)i / (float)pathSegments);

			Vector3 segmentPosition = Vector3.Lerp(beginning.position, ending.position, distanceAlongPath);

			segmentPosition += left.normalized * Random.Range(-pathVariance, pathVariance);

			thePath.Spline.Add(new BezierKnot(segmentPosition), TangentMode.AutoSmooth);
			linepositions.Add(segmentPosition);
		}

		// finally, add the ending position to the path
		thePath.Spline.Add(new BezierKnot(ending.position), TangentMode.AutoSmooth);
		linepositions.Add(ending.position);

		lineRenderer.positionCount = linepositions.Count;

		for (int i = 0; i < linepositions.Count; i++)
		{
			linepositions[i] = new Vector3(linepositions[i].x, linepositions[i].y + (Vector3.Cross(left, dir).normalized * 0.01f).y, linepositions[i].z);
		}

		lineRenderer.SetPositions(linepositions.ToArray());
	}
}