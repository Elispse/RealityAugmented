using UnityEngine;
using UnityEngine.Splines;

public class EnemyScript : MonoBehaviour
{
	[SerializeField] private Animator animator;

	[Header("Enemy Stats")]
    [SerializeField] private float health = 1;
    [SerializeField] private float speed = 2;
	[SerializeField] private float damage = 1;
	[SerializeField] private float range = 1;
	[SerializeField] private float attackSpeed = 1;
	private float attackTimer;

	[Header("Pathway Stuff")]
	[SerializeField] private Pathway path; // pathway the enemy follows towards the target
	[SerializeField] private Transform target; // the target the enemy will hit if close enough
	// This code is taken from my modified maple spline follower code
	[Range(0, 1)] public float tdistance = 0;
	public float length { get { return path.GetPath().CalculateLength(); } }
	public float distance { get { return tdistance * length; } set { tdistance = value / length; } }

	void Update()
    {
		if (Vector3.Distance(transform.position, target.position) < range)
		{
			// attack base on timer once in range
			attackTimer -= Time.deltaTime;
			if (attackTimer <= 0)
			{
				animator.SetTrigger("Attack");
			}

			return;
		}
		
		// update distance based on speed
		distance += speed * Time.deltaTime;

		// calculate where the enemy should be along the pathway using distance
		UpdateTransform(tdistance - Mathf.Floor(tdistance));

		// when the enemy reachest the end of the pathway (aka, the base)
		if (tdistance >= 1)
		{
			death();
			PlayerBase.instance.DamageBase(damage);
		}
    }

	public void AttackBase()
	{
		PlayerBase.instance.DamageBase(damage);
		resetAttack();
	}

	private void resetAttack()
	{
		attackTimer = 1.0f / attackSpeed;
	}

	/// <summary>
	/// Updates the position of the enemy along the spline based on t
	/// </summary>
	/// <param name="t">distance along the spline (clamped from 0-1)</param>
	void UpdateTransform(float t)
	{
		// gets the placement and rotation of the position along the path
		Vector3 position = path.GetPath().EvaluatePosition(t);
		Vector3 up = path.GetPath().EvaluateUpVector(t);
		Vector3 forward = Vector3.Normalize(path.GetPath().EvaluateTangent(t));

		// check for backwards movement, will rotate the enemy accordingly
		if (speed < 0)
		{
			forward *= -1;
		}

		transform.position = position;
		transform.rotation = Quaternion.LookRotation(forward, up);
	}

	/// <summary>
	/// setter for the path variable
	/// </summary>
	/// <param name="target">Pathway the enemy will follow</param>
	public void SetPath(Pathway path)
    {
        this.path = path;
    }

	/// <summary>
	/// setter for the target variable
	/// </summary>
	/// <param name="target">Pathway the enemy will follow</param>
	public void SetTarget(Transform target)
	{
		this.target = target;
	}

	/// <summary>
	/// Deal damage to enemy
	/// </summary>
	/// <param name="damage">Amount of damage taken</param>
	public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log(" I have " + health + " health left");
        if (health <= 0)
        {
            death();
        }
    }

	/// <summary>
	/// function called upon enemy death
	/// </summary>
    public void death()
    {
		Destroy(gameObject);
	}
}
