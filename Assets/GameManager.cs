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

    [SerializeField] private GameObject scanQRUI;
    [SerializeField] private GameObject placeBaseUI;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;

	[SerializeField] private GameObject whatIAmPlacing;

    private bool qrScanned = false;

	private void Awake()
	{
        scanQRUI.SetActive(true);
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
        Physics.Raycast(ray, out hit, 10000.0f, LayerMask.NameToLayer("default"), QueryTriggerInteraction.Ignore);

        if (hit.point == Vector3.zero)
        {
            return Camera.main.transform.position;
        }

        return hit.point;
    }

    public void StartGame()
    {
        if (!qrScanned)
        {
            scanQRUI.SetActive(false);
            placeBaseUI.SetActive(true);

            instance = this;
            whatIAmPlacing = Instantiate(spawnerSpawn.gameObject, transform);
            path = Instantiate(pathwaySpawn.gameObject, transform).GetComponent<Pathway>();
            path.SetBeginning(whatIAmPlacing.transform);
            enemyBase = whatIAmPlacing.GetComponent<EnemySpawner>();

            qrScanned = true;
        }    
    }

    public void SpawnSpawner()
    {
        placeBaseUI.transform.GetChild(0).gameObject.SetActive(false);
        placeBaseUI.transform.GetChild(1).gameObject.SetActive(false);
		whatIAmPlacing = Instantiate(playerSpawn.gameObject, transform);
		path.SetEnding(whatIAmPlacing.transform);
        
        playerBase = whatIAmPlacing.GetComponent<PlayerBase>();

        placeBaseUI.transform.GetChild(2).gameObject.SetActive(true);
        placeBaseUI.transform.GetChild(3).gameObject.SetActive(true);
    }

    public void SpawnBase()
    {
        enemyBase.SetTarget(whatIAmPlacing.transform);
        enemyBase.SetPath(path);
		enemyBase.gameObject.transform.LookAt(playerBase.transform.localPosition);

		//path.gameObject.transform.position = enemyBase.transform.position/2;
		//path.gameObject.transform.LookAt(enemyBase.transform.position);
		//path.gameObject.transform.position = enemyBase.transform.position + (path.gameObject.transform.forward * Vector3.Distance(enemyBase.transform.position, playerSpawn.gameObject.transform.position)/2.0f);

		path.resetPath = true;
		whatIAmPlacing = null;
		playerBase.gameObject.transform.LookAt(enemyBase.transform.position);
        placeBaseUI.SetActive(false);
        gameUI.SetActive(true);
	}

    public void StartWave()
    {
        gameUI.transform.GetChild(0).gameObject.SetActive(false);
        gameUI.transform.GetChild(1).gameObject.SetActive(false);
		enemyBase.StartWave();
    }

    public void WaveEnd()
    {
        gameUI.transform.GetChild(0).gameObject.SetActive(true);
        gameUI.transform.GetChild(1).gameObject.SetActive(true);
        path.resetPath = true;
	}

    public void WinGame()
    {
        gameUI.SetActive(false);
        winUI.SetActive(true);
		//startWaveButton.gameObject.SetActive(false);
	}

    public void LoseGame()
    {
        gameUI.SetActive(false);
        loseUI.SetActive(true);
		//startWaveButton.gameObject.SetActive(false);
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
