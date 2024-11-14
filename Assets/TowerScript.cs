using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
    [SerializeField] private float attackSpeed = 1.0f;
    [SerializeField] private float attackTimer = 1.0f;
    [SerializeField] private float damage = 1.0f;

    [SerializeField] private List<EnemyScript> enemies;

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

			if (theenemy == null)
            {
                enemies.RemoveAt(0);
            } 
            else
            {
                transform.LookAt(theenemy.transform.position);

				attackTimer -= Time.deltaTime;
                if (attackTimer <= 0)
                {
					resetAttack();
					theenemy.TakeDamage(damage);
				}
			}
        }
    }

    private void resetAttack()
    {
        attackTimer = 1.0f / attackSpeed;
    }

    private EnemyScript getEnemyTarget()    // Get the Enemy that the tower should be targetting
	{
        return enemies[0];
    }


	private void OnTriggerEnter(Collider other)
	{
        //Debug.Log(other.name);
        if (enemies.Count == 0)
        {
            attackTimer = 0.0f;
		}

        EnemyScript toadd;

        other.attachedRigidbody.TryGetComponent<EnemyScript>(out toadd);

        if (toadd)
        {
			enemies.Add(toadd);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		EnemyScript toremove;

		other.attachedRigidbody.TryGetComponent<EnemyScript>(out toremove);

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
			if (getEnemyTarget() != null)
			{
				Gizmos.DrawLine(transform.position, getEnemyTarget().transform.position);
			}
			
        }
        else
        {
			Gizmos.DrawSphere(detectionSphere.transform.position, detectionSphere.radius);
		}
	}
}
