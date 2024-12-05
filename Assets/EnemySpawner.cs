using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemySpawner : MonoBehaviour
{
	[Header("Spawner Requirements")]
	[SerializeField] private EnemyScript toSpawn;
    [SerializeField] private Pathway path;
	[SerializeField] private Transform target;

	[Header("Spawner Stats")]
	[SerializeField] public int currentWave = 0;

	public bool doneSpawning = false;
	public bool dowave = false;
	public List<GameObject> spawned = new List<GameObject>();

    void Start()
    {
		//StartCoroutine(spawnTimer());
	}

	void Update()
	{
		if (dowave && doneSpawning && spawned.Count == 0)
		{
			if (currentWave == 3)
			{
				GameManager.instance.WinGame();
			}
			else
			{
				GameManager.instance.WaveEnd();
				dowave = false;
			}
		}

		
		if (spawned.Count > 0 && spawned[0] == null)
		{
			spawned.RemoveAt(0);
		}
	}

	public void StartWave()
	{
		doneSpawning = false;
		dowave = true;
		currentWave++;
		currentWave = Mathf.Clamp(currentWave, 1, 3);
		switch (currentWave)
		{
			case 1:
				StartCoroutine(Wave1());
				break;
			case 2:
				StartCoroutine(Wave2());
				break;
			case 3:
				StartCoroutine(Wave3());
				break;
		}
	}

	/// Spawns enemies at a rate denoted by spawnTime. repeats forever after first called.
    private IEnumerator Wave1()
    {
		for (int i = 0; i < 5; i++) {
			yield return new WaitForSeconds(1.0f);

			SpawnEnemy();
		}

		doneSpawning = true;

	}
	private IEnumerator Wave2()
	{
		for (int i = 0; i < 10; i++)
		{
			yield return new WaitForSeconds(0.5f);

			SpawnEnemy();
		}

		doneSpawning = true;
	}
	private IEnumerator Wave3()
	{
		for (int i = 0; i < 25; i++)
		{
			yield return new WaitForSeconds(0.25f);

			SpawnEnemy();
		}

		doneSpawning = true;
	}

	public void SetTarget(Transform target)
	{
		this.target = target;
	}

	public void SetPath(Pathway path)
	{
		this.path = path;
	}

	private void SpawnEnemy()
	{
		EnemyScript spawned = Instantiate(toSpawn.gameObject, transform.position, Quaternion.identity).GetComponent<EnemyScript>();
		spawned.SetPath(path);
		spawned.SetTarget(target);
		this.spawned.Add(spawned.gameObject);
	}
}
