using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ExperienceManager : MonoBehaviour
{
    [SerializeField] private Button addChickenButton;
    [SerializeField] private ARRaycastManager arRaycastManager;
    [SerializeField] private GameObject chickenPrefab;

    private bool canAddChicken;
    private GameObject chickenPreview;
    private Vector3 detectedPosition = new Vector3();
    private Quaternion detectedRotation = Quaternion.identity;
    private ARTrackable currentTrackable = null;

	private void Start()
	{
        InputHandler.OnTap += SpawnChicken;
        chickenPreview = Instantiate(chickenPrefab);
        SetCanAddChicken(true);
	}

    private void SpawnChicken()
    {
        if (!canAddChicken) return;

        var chicken = Instantiate(chickenPrefab); // Actual spawned chicken
        chicken.GetComponent<Chicken>().PlaceChicken(currentTrackable);
        chicken.transform.position = detectedPosition;
        chicken.transform.rotation = detectedRotation;

        SetCanAddChicken(false); // User has to tap on a button to spawn new chicken
    }

	private void Update()
	{
		// Detect a position and rotation over the detected surface
		GetRaycastHitTransform();
	}

    private void GetRaycastHitTransform()
    {
        var hits = new List<ARRaycastHit>();
        var middleScreen = new Vector2(Screen.width / 2, Screen.height / 2);
        if (arRaycastManager.Raycast(middleScreen, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            detectedPosition = hits[0].pose.position;
            detectedRotation = hits[0].pose.rotation;
            chickenPreview.transform.position = detectedPosition;
            chickenPreview.transform.rotation = detectedRotation;
            currentTrackable = hits[0].trackable;
        }
    }

	private void OnDestroy()
	{
		InputHandler.OnTap -= SpawnChicken;
	}

	public void SetCanAddChicken(bool canAddChicken)
    {
        this.canAddChicken = canAddChicken;
		addChickenButton.gameObject.SetActive(!this.canAddChicken);
        chickenPreview.gameObject.SetActive(canAddChicken);
    }
}
