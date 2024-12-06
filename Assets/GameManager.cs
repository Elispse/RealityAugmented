using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [SerializeField] private EnemySpawner spawnerSpawn;
    [SerializeField] private PlayerBase playerSpawn;
    [SerializeField] private Pathway pathwaySpawn;
    [SerializeField] private TowerScript towerSpawn;

    private EnemySpawner enemyBase;
    private PlayerBase playerBase;
    private Pathway path;
    private GameObject movingTower;

    [SerializeField] private LayerMask placementLayerMask;
    [SerializeField] private LayerMask towerLayerMask;

	[SerializeField] private GameObject scanQRUI;
    [SerializeField] private GameObject placeBaseUI;
    [SerializeField] private GameObject gameUI;
	[SerializeField] private GameObject towerPlaceUI;
	[SerializeField] private GameObject towerGrabUI;
	[SerializeField] private GameObject towerReleaseUI;
	[SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;

	[SerializeField] private GameObject whatIAmPlacing;

    private bool qrScanned = false;
    private bool gamePaused = false;
    private string sceneToLoad;

	private void Awake()
	{
        scanQRUI.SetActive(true);
        //StartGame();
	}

	private void Update()
	{
        IsOnTower();
        if (whatIAmPlacing == null)
        {
            return;
        }

		Vector3 spwanPos = GetSeenPosition();

        if (spwanPos != Vector3.zero)
        {
			whatIAmPlacing.transform.position = spwanPos;
		}

	}

	public Vector3 GetSeenPosition()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(ray, out hit, 10000.0f, placementLayerMask, QueryTriggerInteraction.Ignore);

		if (hit.point == Vector3.zero)
        {
            return Camera.main.transform.position;
        }

        return hit.point;
    }

    public void IsOnTower()
    {
        if (whatIAmPlacing != null)
        {
            return;
        }

		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		RaycastHit hit = new RaycastHit();
		Physics.Raycast(ray, out hit, 10000.0f, towerLayerMask, QueryTriggerInteraction.Ignore);

        if (hit.rigidbody)
        {
            if (hit.rigidbody.GetComponent<TowerScript>() != null && whatIAmPlacing == null)
            {
                towerGrabUI.SetActive(true);
            }
        }
        else
        {
            towerGrabUI.SetActive(false);
        }

        Debug.Log("Testing");
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

    public void PlaceTower()
    {
        Vector3 spwanPos = GetSeenPosition();

		if (spwanPos != Vector3.zero)
        {
			Instantiate(towerSpawn.gameObject, spwanPos, Quaternion.identity, transform);
		}

		towerPlaceUI.SetActive(false);
	}

    public void GrabTower()
    {
		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		RaycastHit hit = new RaycastHit();
		Physics.Raycast(ray, out hit, 10000.0f, towerLayerMask);
        Debug.Log(hit.collider.gameObject.name);    
		whatIAmPlacing = hit.rigidbody.gameObject;
        towerReleaseUI.SetActive(true);
        towerGrabUI.SetActive(false);
	}
	public void ReleaseTower()
	{
        whatIAmPlacing = null;
		towerReleaseUI.SetActive(false);
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
		towerPlaceUI.SetActive(true);
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

    public void PauseGame()
    {
        if (!gamePaused)
        {
            pauseUI.SetActive(true);
            gameUI.SetActive(false);
            gamePaused = true;
            Time.timeScale = 0;
        }
        else
        {
            pauseUI.SetActive(false);
            gameUI.SetActive(true);
            gamePaused = false;
            Time.timeScale = 1;
        }
    }

    public void PlayAgain()
    {
        sceneToLoad = "MainScene";
        StartCoroutine(LoadSceneASync());
    }

    public void QuitGame()
    {
        sceneToLoad = "MainMenu";
        StartCoroutine(LoadSceneASync());
    }

    private IEnumerator LoadSceneASync()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }

    private void OnDrawGizmos()
	{
        //Gizmos.DrawSphere(GetSeenPosition(), 1.0f);
        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.forward * 10000.0f);
	}
}
