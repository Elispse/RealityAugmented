using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnTime = 1.0f;
    [SerializeField] private EnemyScript toSpawn;
    [SerializeField] private Transform playerbase; // this at some point will be replaced with the path, how ever that is made

    void Start()
    {
		StartCoroutine(spawnTimer());
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator spawnTimer()
    {
        yield return new WaitForSeconds(spawnTime);
		EnemyScript spawned = Instantiate(toSpawn.gameObject, transform.position, Quaternion.identity).GetComponent<EnemyScript>();
		spawned.SetTarget(playerbase);
		StartCoroutine(spawnTimer());
    }
}
