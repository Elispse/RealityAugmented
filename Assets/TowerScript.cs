using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
	[Header("Tower Stats")]
	[SerializeField] private float attackSpeed = 1.0f;
    [SerializeField] private float attackTimer = 1.0f;
    [SerializeField] private float damage = 1.0f;

    [SerializeField] private List<EnemyScript> enemies;

	[Header("Debug stuff for now")]
	// Variables here are for debugging and Gizmos purposes, and can probably be removed later
	public SphereCollider detectionSphere;
    public Color detectionColor;

	private void Start()
	{
		resetAttack();
	}

	void Update()
    {
        if (enemies.Count > 0)
        {
            // get the currently targeted enemy
			EnemyScript theenemy = getEnemyTarget();

			if (theenemy == null) // if a null enemy was returned, remove the null from the list 
            {
                enemies.RemoveAt(0);
            } 
            else // else, look at the enemy and attack them
            {
                transform.LookAt(theenemy.transform.position);

				// When attack timer reaches 0, attack
				attackTimer -= Time.deltaTime;
                if (attackTimer <= 0)
                {
					attack(theenemy);
				}
			}
        }
    }

	/// <summary>
	/// The Tower attacks the enemy
	/// </summary>
	private void attack(EnemyScript theenemy)
	{
		resetAttack();

		// currently is just directly damages the enemy. Could spawn a projectile here instead.
		theenemy.TakeDamage(damage);
	}

	/// <summary>
	/// Resets the attack timer of the tower
	/// </summary>
    private void resetAttack()
    {
        attackTimer = 1.0f / attackSpeed;
    }

	/// <summary>
	/// Used to determine which enemy will get targeted within range
	/// </summary>
	/// <returns>The enemy that will be targeted</returns>
    private EnemyScript getEnemyTarget()    // Get the Enemy that the tower should be targetting
	{
        return enemies[0];
    }


	private void OnTriggerEnter(Collider other)
	{
        EnemyScript toadd;

        other.attachedRigidbody.TryGetComponent<EnemyScript>(out toadd);

		// check if other is an enemy
        if (toadd)
        {
			// reset attack timer if the incoming enemy is the first one
			if (enemies.Count == 0)
			{
				attackTimer = 0.0f;
			}

			enemies.Add(toadd);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		EnemyScript toremove;

		other.attachedRigidbody.TryGetComponent<EnemyScript>(out toremove);

		// check if other is an enemy
		if (toremove)
		{
			enemies.Remove(toremove);
		}
	}


	// Gizmos so I can see what is going on
	private void OnDrawGizmos()
	{
		
        Gizmos.color = detectionColor;
        if (enemies.Count > 0)
		{
			// debug gizmos that show me who the tower is attacking.
			if (getEnemyTarget() != null)
			{
				Gizmos.DrawLine(transform.position, getEnemyTarget().transform.position);
			}
        }
        else
        {
			// debug gizmos that show me attack range because there are no enemies in the range
			Gizmos.DrawSphere(detectionSphere.transform.position, detectionSphere.radius);
		}
	}
}
