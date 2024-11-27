using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner spawnerSpawn;
    [SerializeField] private PlayerBase playerSpawn;
    [SerializeField] private Pathway pathwaySpawn;

    [SerializeField] private Button enemyspawnerButton;
    [SerializeField] private Button playerbaseButton;

	[SerializeField] private GameObject whatIAmPlacing;

	private void Start()
	{
        whatIAmPlacing = Instantiate(spawnerSpawn.gameObject);
	}

	private void Update()
	{
        if (whatIAmPlacing == null)
        {
            return;
        }

		Vector3 spwanPos = GetSeenPosition();

        whatIAmPlacing.transform.position = spwanPos;
	}

	public Vector3 GetSeenPosition()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(ray, out hit, 10000.0f);

        if (hit.point == Vector3.zero)
        {
            return Camera.main.transform.position;
        }

        return hit.point;
    }

    public void SpawnSpawner()
    {
        whatIAmPlacing = Instantiate(playerSpawn.gameObject);
    }

    public void SpawnBase()
    {
        whatIAmPlacing = null;
    }

    public void SpawnPathway()
    {

    }

	private void OnDrawGizmos()
	{
        Gizmos.DrawSphere(GetSeenPosition(), 1.0f);
        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.forward * 10000.0f);
	}
}
