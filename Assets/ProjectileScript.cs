using System.Collections;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [Header("Projectile Stats")]
    [SerializeField] private float speed = 2;
    [SerializeField] private float lifetime = 2;
    [SerializeField] private float damage = 2;

    public EnemyScript enemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Lifetime());
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(enemy.gameObject.transform.position);
        transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
    }

    IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.GetComponent<EnemyScript>() != null)
		{
			collision.gameObject.GetComponent<EnemyScript>().TakeDamage(damage);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<EnemyScript>() != null)
        {
            other.GetComponent<EnemyScript>().TakeDamage(damage);
        }
	}
}
