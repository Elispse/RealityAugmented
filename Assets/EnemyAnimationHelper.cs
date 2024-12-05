using UnityEngine;

public class EnemyAnimationHelper : MonoBehaviour
{
	[SerializeField] private EnemyScript myself;

	public void Attack()
	{
		myself.AttackBase();
	}
}
