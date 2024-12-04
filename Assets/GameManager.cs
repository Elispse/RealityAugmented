using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [SerializeField] private EnemySpawner spawnerSpawn;
    [SerializeField] private PlayerBase playerSpawn;
    [SerializeField] private Pathway pathwaySpawn;

    private EnemySpawner enemyBase;
    private PlayerBase playerBase;
    private Pathway path;

    [SerializeField] private Button enemyspawnerButton;
    [SerializeField] private Button playerbaseButton;
    [SerializeField] private Button startWaveButton;
    [SerializeField] private GameObject WinUI;
    [SerializeField] private GameObject LoseUI;

	[SerializeField] private GameObject whatIAmPlacing;

	private void Start()
	{
		instance = this;
		whatIAmPlacing = Instantiate(spawnerSpawn.gameObject, transform);
		path = Instantiate(pathwaySpawn.gameObject, transform).GetComponent<Pathway>();
        path.SetBeginning(whatIAmPlacing.transform);
        enemyspawnerButton.gameObject.SetActive(true);
		enemyBase = whatIAmPlacing.GetComponent<EnemySpawner>();
        
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
		enemyspawnerButton.gameObject.SetActive(false);
		whatIAmPlacing = Instantiate(playerSpawn.gameObject, transform);
		path.SetEnding(whatIAmPlacing.transform);
		playerbaseButton.gameObject.SetActive(true);
		playerBase = playerSpawn.GetComponent<PlayerBase>();
	}

    public void SpawnBase()
    {
		playerbaseButton.gameObject.SetActive(false);
        enemyBase.SetTarget(whatIAmPlacing.transform);
        enemyBase.SetPath(path);
		enemyBase.gameObject.transform.LookAt(playerBase.transform.position);

		//path.gameObject.transform.position = enemyBase.transform.position/2;
		//path.gameObject.transform.LookAt(enemyBase.transform.position);
		//path.gameObject.transform.position = enemyBase.transform.position + (path.gameObject.transform.forward * Vector3.Distance(enemyBase.transform.position, playerSpawn.gameObject.transform.position)/2.0f);

		path.resetPath = true;
		whatIAmPlacing = null;
        startWaveButton.gameObject.SetActive(true);
		playerBase.gameObject.transform.LookAt(enemyBase.transform.position);
	}

    public void StartWave()
    {
		startWaveButton.gameObject.SetActive(false);
		enemyBase.StartWave();
    }

    public void WaveEnd()
    {
		startWaveButton.gameObject.SetActive(true);
		path.resetPath = true;
	}

    public void WinGame()
    {
        WinUI.SetActive(true);
		startWaveButton.gameObject.SetActive(false);
	}

    public void LoseGame()
    {
		LoseUI.SetActive(true);
		startWaveButton.gameObject.SetActive(false);
	}

    public void EndGame()
    {
        Destroy(this.gameObject);
    }

	private void OnDrawGizmos()
	{
        //Gizmos.DrawSphere(GetSeenPosition(), 1.0f);
        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.forward * 10000.0f);
	}
}
