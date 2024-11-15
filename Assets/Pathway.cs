using UnityEngine;
using UnityEngine.Splines;

public class Pathway : MonoBehaviour
{
    [SerializeField] private int pathSegments = 1;
    [SerializeField] private SplineContainer thePath;
    [SerializeField] private Transform beginning;
    [SerializeField] private Transform ending;

    void Start()
    {
        thePath = GetComponent<SplineContainer>();

        thePath.Spline.Add(new BezierKnot(beginning.position));

        for (int i = 0; i < pathSegments; ++i)
        {

        }

		thePath.Spline.Add(new BezierKnot(ending.position));
	}
}
