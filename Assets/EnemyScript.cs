using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float health = 1;
    [SerializeField] private float speed = 2;

    [SerializeField] private Transform target; // what the enemy will move towards. will change to a spline at some point

	void Update()
    {
        // enemy should stop existing when reaching the target
		if (Vector3.Distance(target.position, transform.position) < 1.0f)
		{
			death();
		}

		// the enemy moves towards it's target at a speed proportional to the distance from the target (this makes it constant)
		transform.Translate((target.position - transform.position) / Vector3.Distance(target.position, transform.position) * Time.deltaTime * speed);
    }

    // setter for target so the spawner can set it
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log(" I have " + health + " health left");
        if (health <= 0)
        {
            death();
        }
    }

    public void death()
    {
		Destroy(gameObject);
	}
}
