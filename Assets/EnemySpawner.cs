using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[Header("Spawner Requirements")]
	[SerializeField] private EnemyScript toSpawn;
    [SerializeField] private Pathway path;

	[Header("Spawner Stats")]
	[SerializeField] private float spawnTime = 1.0f;

    void Start()
    {
		StartCoroutine(spawnTimer());
	}

	/// Spawns enemies at a rate denoted by spawnTime. repeats forever after first called.
    private IEnumerator spawnTimer()
    {
        yield return new WaitForSeconds(spawnTime);

		EnemyScript spawned = Instantiate(toSpawn.gameObject, transform.position, Quaternion.identity).GetComponent<EnemyScript>();
		spawned.SetPath(path);

		StartCoroutine(spawnTimer());
    }
}
